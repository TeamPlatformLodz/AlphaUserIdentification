using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlphaUserIdentification.Data;
using AlphaUserIdentification.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AlphaUserIdentification.Models.TeamViewModels;
using AlphaUserIdentification.Extensions;

namespace AlphaUserIdentification.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeamController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Team
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.ToListAsync());
        }

        // GET: Team/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Members)
                .ThenInclude(m => m.ApplicationUser)
                .SingleOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }
            // local method
            List<ApplicationUser> GetNonMembers ()
            {
                var allUsers = _context.ApplicationUser;
                List<ApplicationUser> nonTeamUsers = new List<ApplicationUser>();
                bool isNotIn;
                foreach ( var user in allUsers)
                {
                    isNotIn = true;
                    foreach(var teamUser in team.Members)
                    {
                        if (user.Id == teamUser.ApplicationUserId)
                        {
                            isNotIn = false;
                            break;
                        }
                    }
                    if (isNotIn)
                        nonTeamUsers.Add(user);
                }
                return nonTeamUsers;
            }
            var notTeamUsers = GetNonMembers();
            DetailsViewModel viewModel = new DetailsViewModel()
            {
                IsAdmin = await IsAdmin(team.TeamId),
                Team = team,
                AlreadyTeamUsers = team.Members,
                NotTeamUsers = notTeamUsers
            };
            return View(viewModel);
        }

        // GET: Team/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Team/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ImageUrl")] Team team)
        {
            if (ModelState.IsValid)
            {
                var curUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
                team.AddAdministrator(curUser);
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Team/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.SingleOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Team/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamId,Name,Description,ImageUrl")] Team team)
        {
            if (id != team.TeamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.TeamId))
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
            return View(team);
        }

        // GET: Team/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .SingleOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.SingleOrDefaultAsync(m => m.TeamId == id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.TeamId == id);
        }


        // GET: Team/AddMember
        public async Task<IActionResult> AddMember()
        {
            var curUser = await UserHelper.GetCurrentUserById(_context, _userManager.GetUserId(User));
            var teams = new List<Team>();
            //adds administrator teams to teams list
            _context.Administrators.Where(a => a.ApplicationUserId == curUser.Id).
                Include(a => a.Team).ToList().
                ForEach(a => { Task.Run(() => teams.Add(a.Team));}); 
            //var teams = new
            var users = await _context.Users.ToListAsync();
            return View(
                new Tuple<AddMemberViewModel,AddMemberPostViewModel> (
                    new AddMemberViewModel {
                    Teams = teams,
                    Users = users
                },
                    new AddMemberPostViewModel() 
                ));
        }

        // POST: Team/AddMember
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(int id, List<string> users)
        {
            var teamId = id;
            if (ModelState.IsValid)
            {
                if (await IsAdmin(teamId))
                {
                    var teamToAdd = await _context.Teams.FirstOrDefaultAsync(t => t.TeamId == teamId);
                    if (teamToAdd != null)
                    {
                        foreach (var userId in users)
                        {
                            var userToAdd = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                            if (userToAdd != null)
                            {
                                teamToAdd.AddMember(userToAdd);
                                await _context.SaveChangesAsync();
                            }
                        }
                        return RedirectToAction(nameof(Details), new { id = teamId });
                    }
                }        
            }
            return RedirectToAction(nameof(Index));
        }

        #region Helper Methods
        /// <summary>
        /// Helper method to check if currently logged user is an Administrator of given Team.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        async Task<bool> IsAdmin(int teamId)
        {
            ApplicationUser curUser = await UserHelper.GetCurrentUserById(_context, _userManager.GetUserId(User));
            bool result = _context.Administrators.Where(a => a.TeamId == teamId).ToList().Exists(u => u.ApplicationUserId == curUser.Id);
            return result;
        }
        #endregion
    }
}
