using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectBoost.Context;
using ProjectBoost.Models;
using ProjectBoost.Models.ViewModels.ProjectModels;

namespace ProjectBoost.Controllers {
    public class ProjectsController : Controller {
        private readonly ProjectBoostContext _context;

        public ProjectsController(ProjectBoostContext context) {
            _context = context;
        }

        // GET: Projects
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Index() {
            var projectBoostContext = _context.Projects.Include(p => p.User);
            return View(await projectBoostContext.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(Guid? id) {
            if(id == null) {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if(project == null) {
                return NotFound();
            }
                
            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create() {
            ViewData["UserNickname"] = new SelectList(_context.Users, "Nickname", "Nickname");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserNickname,Name,Description,Demo,RequiredAmount,DeadLine")] ProjectCreateModel project) {
            Project dbProject = new Project() {
                ID = Guid.NewGuid(),
                Name = project.Name,
                Blocked = false,
                DeadLine = project.DeadLine,
                Demo = project.Demo,
                Description = project.Description,
                ReceivedAmount = 0,
                RequiredAmount = project.RequiredAmount,
                UserID = (await _context
                                .Users.Where(user => user.Nickname == project.UserNickname)
                                .FirstAsync()).ID
            };
            
            if(ModelState.IsValid) {
                _context.Add(dbProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserNickname"] = new SelectList(_context.Users, "Nickname", "Nickname", project.Name);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(Guid? id) {
            if(id == null) 
                return NotFound();
            

            var project = await _context.Projects.FindAsync(id);
            if(project == null) 
                return NotFound();

            ProjectEditModel viewModel = new ProjectEditModel() {
                ID = project.ID,
                Blocked = project.Blocked,
                DeadLine = project.DeadLine,
                Demo = project.Demo,
                Description = project.Description,
                RequiredAmount = project.RequiredAmount
            };

            return View(viewModel);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,Demo,RequiredAmount,DeadLine,Blocked")] ProjectEditModel project) {
            if(!ModelState.IsValid)
                return View(project);

            if(!ProjectExists(id))
                return NotFound();

            Project dbProject = _context.Projects.Find(id);

            dbProject.Blocked           = project.Blocked;
            dbProject.RequiredAmount    = project.RequiredAmount;
            dbProject.DeadLine          = project.DeadLine;
            dbProject.Demo              = project.Demo;
            dbProject.Description       = project.Description;

            try {
                _context.Update(dbProject);
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                throw;
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(Guid? id) {
            if(id == null) {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if(project == null) {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id) {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(Guid id) {
            return _context.Projects.Any(e => e.ID == id);
        }
    }
}
