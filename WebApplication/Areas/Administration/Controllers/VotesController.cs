using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.Models.Contexts;

namespace WebApplication.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class VotesController : Controller
    {
        private readonly WebApplicationContext _context;

        public VotesController(WebApplicationContext context)
        {
            _context = context;
        }

        // GET: Administration/Votes
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var webApplicationContext = _context.Votes.Include(v => v.Feedback).Include(v => v.User);
            return View(await webApplicationContext.ToListAsync());
        }

        // GET: Administration/Votes/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .Include(v => v.Feedback)
                .Include(v => v.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // GET: Administration/Votes/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["FeedbackId"] = new SelectList(_context.Feedbacks, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login");
            return View();
        }

        // POST: Administration/Votes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(Vote vote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FeedbackId"] = new SelectList(_context.Feedbacks, "Id", "Name", vote.FeedbackId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", vote.UserId);
            return View(vote);
        }

        // GET: Administration/Votes/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes.SingleOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }
            ViewData["FeedbackId"] = new SelectList(_context.Feedbacks, "Id", "Name", vote.FeedbackId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", vote.UserId);
            return View(vote);
        }

        // POST: Administration/Votes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, Vote vote)
        {
            if (id != vote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteExists(vote.Id))
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
            ViewData["FeedbackId"] = new SelectList(_context.Feedbacks, "Id", "Name", vote.FeedbackId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", vote.UserId);
            return View(vote);
        }

        // GET: Administration/Votes/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .Include(v => v.Feedback)
                .Include(v => v.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // POST: Administration/Votes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vote = await _context.Votes.SingleOrDefaultAsync(m => m.Id == id);
            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteExists(int id)
        {
            return _context.Votes.Any(e => e.Id == id);
        }
    }
}
