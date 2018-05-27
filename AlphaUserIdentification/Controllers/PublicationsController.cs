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
            var publications = await _context.Publications.Where(p => IsVisible(p, _userManager.GetUserId(User))).Include(p => p.Author).ToListAsync();
            var model = new IndexViewModel { Publications = publications };
            return View(model);
        }
        public bool IsVisible(Publication p, string userId)
        {
            if (p.Visibility == PublicationVisibility.Public)
            {
                return true;
            }
            var user = _context.Users.First(u => u.Id == userId);
            var authorTeams = _context.Teams.Where(t => p.Author.Teams.Exists(t2 => t2.TeamId == t.TeamId));
            var userTeams = _context.Teams.Where(t => user.Teams.Exists(t2 => t2.TeamId == t.TeamId));
            foreach (var authorTeam in authorTeams) // check if author and user in the same team
            {
                if (userTeams.Any(t => t.TeamId == authorTeam.TeamId))
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Url,Visibility")] Publication publication)
        {
            publication.Author = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
         
            if (ModelState.IsValid)
            {
                _context.Add(publication);
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

        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.PublicationId == id);
        }
    }
}
