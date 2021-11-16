using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBoost.Context;
using ProjectBoost.Models;
using ProjectBoost.Models.ViewModels.UserModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ProjectBoost.Controllers {
    public class UsersController : Controller {
        private readonly ProjectBoostContext _context;

        public UsersController(ProjectBoostContext context) {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index() {
            //UserManager;
            if(User.Identity.IsAuthenticated)
                return View(await _context.Users.ToListAsync());

            return RedirectToAction("Login");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id) {
            if(id == null)
                return NotFound();

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (user == null)
                return NotFound();

            UserDetailsModel viewModel = new UserDetailsModel() {
                ID = user.ID,
                Nickname = user.Nickname,
                OpenFinantialHistory = user.OpenFinantialHistory,
                Restricted = user.Restricted,
                RoleID = user.RoleID
            };

            return View(viewModel);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create() {
            return await Task.FromResult(View());
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nickname,Password,OpenFinantialHistory")] UserRegisterModel user) {
            if(!ModelState.IsValid)
                return View(user);
            User dbUser = new User() {
                ID = Guid.NewGuid(),
                RoleID = 2,
                Nickname = user.Nickname,
                Password = user.Password,
                OpenFinantialHistory = user.OpenFinantialHistory,
                Restricted = false
            };
            _context.Add(dbUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Login() {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Nickname,Password")] UserLoginModel user) {
            if(!ModelState.IsValid)
                return View(user);

            var users = (await _context.Users.Where(_user =>
                _user.Nickname == user.Nickname &&
                _user.Password == user.Password
            ).ToArrayAsync());

            if(users.Length == 0) {
                ViewBag.ErrorMessage = "Некорректные логин и(или) пароль";
                return View(user);
            }

            var dbUser = users.First();
            await Authenticate(dbUser);
            return RedirectToAction("Index");
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id) {
            if(id == null)
                return NotFound();

            var user = await _context.Users.FindAsync(id);

            if(user == null)
                return NotFound();

            UserEditModel viewModel = new UserEditModel() {
                ID = user.ID,
                Nickname = user.Nickname,
                Password = "",
                OpenFinantialHistory = user.OpenFinantialHistory,
                Restricted = user.Restricted,
            };

            return View(viewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Nickname,Password,OpenFinantialHistory,Restricted")] UserEditModel user) {
            if (id != user.ID)
                return NotFound();

            User dbUser = new User() {
                ID = user.ID,
                Nickname = user.Nickname,
                Password = user.Password,
                Restricted = user.Restricted,
                OpenFinantialHistory = user.OpenFinantialHistory
            };
           

            if (ModelState.IsValid) {
                try {
                    _context.Update(dbUser);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if(!UserExists(user.ID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id) {
            if (id == null) 
                return NotFound();
            
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (user == null)
                return NotFound();
            
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id) {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id) {
            return _context.Users.Any(e => e.ID == id);
        }
        private async Task Authenticate(User user) {
            // создаем один claim
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.ID.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }
    }
}
