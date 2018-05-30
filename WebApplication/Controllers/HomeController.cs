using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.Models.Contexts;
using WebApplication.Models.ViewModels;
using WebApplication.Models.ViewModels.Home;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebApplicationContext _context;

        public HomeController(WebApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == User.Identity.Name);
            if(user != null)
            {
                ViewBag.UserId = user.Id;
            }          
            ViewBag.Votes = _context.Votes
                .Include(v => v.Feedback)
                .Include(v => v.User);

            int pageSize = 2;


            var feedbacks =  _context.Feedbacks.Include(u => u.User);

            var count = await feedbacks.CountAsync();
            var items = await feedbacks.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Feedbacks = items
            };

            return View(viewModel);

        }

        [HttpGet("{vote:int}/{feedback:int}")]
        [Authorize(Roles = "Administrator, Default")]
        public async Task<IActionResult> UpdateVote(int feedback, int vote)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == User.Identity.Name);

            if (user.VotesCounter > 0)
            {
                user.VotesCounter--;

                _context.Update(user);
                await _context.SaveChangesAsync();

                Vote model = new Vote
                {
                    RatingGiven = vote,
                    UserId = user.Id,
                    FeedbackId = feedback,
                    Time = DateTime.Now,
                    VoteReturned = false
                };

                _context.Votes.Add(model);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator, Default")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administration/Feedbacks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Default")]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == User.Identity.Name);
                feedback.UserId =user.Id;

                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }
    }
}
