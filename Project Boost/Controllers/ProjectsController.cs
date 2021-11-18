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
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Demo,RequiredAmount,DeadLine")] ProjectCreateModel project) {
            Project dbProject = new Project() {
                ID = Guid.NewGuid(),
                Name = project.Name,
                Blocked = false,
                DeadLine = project.DeadLine,
                Demo = project.Demo,
                Description = project.Description,
                ReceivedAmount = 0,
                RequiredAmount = project.RequiredAmount,
                UserID = Guid.Parse(User.FindFirst(claim => claim.Type == "ID").Value)
            };
            bool valid = ModelState.IsValid; // для дебага
            { }
            if(valid) {
                _context.Add(dbProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(Guid? id) {
            if(id == null) 
                return NotFound();
            

            var project = await _context.Projects.FindAsync(id);
            if(project == null) 
                return NotFound();

            if(!await IsAllowedToChange(project.ID.ToString(), 
                User.FindFirst(claim => claim.Type == "ID").Value)) 
                return Unauthorized();
            

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

            if(!await IsAllowedToChange(id.ToString(),
                User.FindFirst(claim => claim.Type == "ID").Value))
                return Unauthorized();
            
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

            if(!await IsAllowedToChange(project.ID.ToString(),
                User.FindFirst(claim => claim.Type == "ID").Value))
                return Unauthorized();

            return View(project);
        }
        
       

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id) {
            var project = await _context.Projects.FindAsync(id);

            if(!await IsAllowedToChange(project.ID.ToString(),
                User.FindFirst(claim => claim.Type == "ID").Value))
                return Unauthorized();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        private bool ProjectExists(Guid id) {
            return _context.Projects.Any(e => e.ID == id);
        }
        [NonAction]
        private async Task<bool> IsAllowedToChange(string projectID, string userID) {
            Project proj = await _context.Projects.FindAsync(Guid.Parse(projectID));

            if(proj == null)
                throw new Exception("Project not found");

            return proj.UserID.ToString() == userID || User.IsInRole("admin");
        }
    }
}
