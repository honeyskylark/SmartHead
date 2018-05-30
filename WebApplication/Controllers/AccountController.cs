using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplication.Models.Contexts;
using System.Linq;
using System;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebApplicationContext _context;

        public AccountController(WebApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationModel registrationModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == registrationModel.Login);
                if (user == null)
                {
                    user = new User
                    {
                        Login = registrationModel.Login,
                        Password = registrationModel.Password,
                        FirstName = registrationModel.FirstName,
                        LastName = registrationModel.LastName,
                        Patronymic = registrationModel.Patronymic,
                        VotesCounter = 10 
                    };

                    Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Default");
                    if (userRole != null)
                        user.Role = userRole;

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Неправильный логин и(или) пароль");
            }
            return View(registrationModel);
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                await EnsureUpdatedAsync(loginModel.Login);

                User user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Login == loginModel.Login && u.Password == loginModel.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");

                }
                ModelState.AddModelError("Password", "Неправильный логин и(или) пароль");
            }
            return View(loginModel);
        }

        private async Task Authenticate(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task EnsureUpdatedAsync(string login)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == login);
            var votes = _context.Votes.Where(m => m.UserId == user.Id && m.VoteReturned == false);
            var selectedUser = await _context.Users.SingleOrDefaultAsync(u => u.Login == login);

            foreach(var vote in votes)
            {
                var difference = (DateTime.Now - vote.Time).TotalDays;
                if(difference >= 3)
                {                    
                    // updating VoteReturned state
                    vote.VoteReturned = true;
                    _context.Update(vote);

                    // updating VotesCounter of user
                    selectedUser.VotesCounter++;
                    _context.Update(selectedUser);                  
                }
            }
            _context.SaveChanges();

        }
    }
}
