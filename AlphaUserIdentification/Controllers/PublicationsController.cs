using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlphaUserIdentification.Data;
using AlphaUserIdentification.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AlphaUserIdentification.Models.PublicationsViewModels;

namespace AlphaUserIdentification.Controllers
{
    [Authorize]
    public class PublicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PublicationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Publications
        public async Task<IActionResult> Index()
        {
            var curUserWithTeamsLoaded = await _context.Users.Include(u => u.Teams).FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            var publications = await _context.Publications.Include(p => p.Author)
                                                          .Include(p => p.Author.Teams)
                                                          .Include(p => p.PublishedFor)
                                                          .ToListAsync();
            publications = publications.Where(p => IsVisible(p, curUserWithTeamsLoaded) == true).ToList();


            var model = new IndexViewModel { Publications = publications };
            return View(model);
        }
        public bool IsVisible(Publication p, ApplicationUser user)
        {
            if(p.Author == null)
            {
                return true;
            }
            if (p.Author.Id == user.Id)
            {
                return true;
            }
            if (p.Visibility == PublicationVisibility.Public) //  || p.Visibility == PublicationVisibility.None
            {
                return true;
            }
            var commonTeams = from Member t in user.Teams
                              join PublishedFor pf in p.PublishedFor on t.TeamId equals pf.TeamId
                              select t.TeamId;
            commonTeams = commonTeams.ToList();
            if(commonTeams.Any())
            {
                return true;
            }
            return false;
        }

        // GET: Publications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications.Include(p => p.Author).SingleOrDefaultAsync(m => m.PublicationId == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        // GET: Publications/Create
        public async Task<IActionResult> Create()
        {
            var curUserId = _userManager.GetUserId(User);
            ViewData["teams"] = await _context.Member.Where(m => m.ApplicationUserId == curUserId).Select(m => m.Team).ToListAsync();
            return View();
        }

        // POST: Publications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<int> teamIds, [Bind("Description,Url,Visibility")] Publication publication)
        {
            var author = await _context.Users.Include(u => u.Teams).FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            publication.Author = author;
            if (ModelState.IsValid)
            {
                _context.Add(publication);
                await _context.SaveChangesAsync();
                foreach (var teamId in teamIds)
                {
                    if(author.Teams.Find(t => t.TeamId == teamId) != null)
                    {
                        _context.Add(new PublishedFor
                        {
                            TeamId = teamId,
                            PublicationId = publication.PublicationId
                        });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publication);
        }

        // GET: Publications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications.Include(p => p.Author).SingleOrDefaultAsync(m => m.PublicationId == id);
            if (publication == null)
            {
                return NotFound();
            }
            return View(publication);
        }

        // POST: Publications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PublicationId,Description,Url,Visibility")] Publication publication)
        {
            if (id != publication.PublicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicationExists(publication.PublicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publication);
        }

        // GET: Publications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications
                .SingleOrDefaultAsync(m => m.PublicationId == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        // POST: Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publication = await _context.Publications.SingleOrDefaultAsync(m => m.PublicationId == id);
            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async void LikePublication([Bind("PublicationId")] Publication publication)
        {
            var curUserId = GetCurrentUserId();
            var like = await _context.PublicationLikes.FirstOrDefaultAsync(l => l.ApplicationUserId == curUserId && l.PublicationId == publication.PublicationId);
            if(like != null)
            {
                //TODO: error
            }
            else
            {
                _context.PublicationLikes.Add(new PublicationLike
                {
                    PublicationId = publication.PublicationId,
                    ApplicationUserId = curUserId
                });
            }
        }

        private string GetCurrentUserId()
        {
            return _userManager.GetUserId(User);
        }

        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.PublicationId == id);
        }
    }
}
