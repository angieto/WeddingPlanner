using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
// add these lines for validation
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
// add these lines for session
using Microsoft.AspNetCore.Http;

namespace WeddingPlanner.Controllers
{
    public class DashboardController : Controller
    {
        // Set up Context Service
        private WeddingContext dbContext;
        public DashboardController(WeddingContext context) {
            dbContext = context;
        }

        // ROUTES
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("UserId") == null) 
            {
                return RedirectToAction("Index", "Home");
            } 
            else 
            {
                int? UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.UserId = UserId;
                ViewBag.LogUser = dbContext.Users.FirstOrDefault(user => user.UserId == UserId);
                // query all wedding invitation the user's received
                List<Wedding> AllWeds = dbContext.Weddings.Include(w => w.RSVPs).ThenInclude(rsvp => rsvp.User).OrderByDescending(w => w.Date).ToList();
                List<RSVP> UserRSVPs = dbContext.RSVPs.Include(r => r.Wedding).Include(r => r.User).Where(r => r.UserId == UserId).ToList(); 
                // Create a list of wedding ids the current user has rsvp
                List<int> UsedWedIds = UserRSVPs.Select(r => r.WeddingId).ToList();
                ViewBag.AllWeds = AllWeds;
                ViewBag.UsedRSVPs = UsedWedIds;
                return View("Dashboard");
            }
        }

        [HttpPost("rsvp")]
        public IActionResult RSVP(int weddingId, int userId)
        {
            // form a relationship btw the user & the wedding
            Wedding selectedWed = dbContext.Weddings.Include(w => w.RSVPs).ThenInclude(rsvp => rsvp.User).FirstOrDefault(w => w.WeddingId == weddingId);
            RSVP connection = new RSVP()
            {
                UserId = userId,
                WeddingId = weddingId
            };
            dbContext.RSVPs.Add(connection);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost("un-rsvp")]
        public IActionResult UnRSVP(int weddingId, int userId)
        {
            // remove the relationship btw the user & the wedding
            Wedding selectedWed = dbContext.Weddings.Include(w => w.RSVPs).ThenInclude(rsvp => rsvp.User).FirstOrDefault(w => w.WeddingId == weddingId);
            RSVP connection = dbContext.RSVPs.FirstOrDefault(r => r.UserId == userId);
            dbContext.Remove(connection);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost("delete")]
        public IActionResult Delete(int weddingId)
        {
            Wedding selectedWed = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
            dbContext.Weddings.Remove(selectedWed);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            if (HttpContext.Session.GetInt32("UserId") == null) 
            {
                return RedirectToAction("Index", "Home");
            } 
            else 
            {
                int? UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.UserId = UserId;
                User LogUser = dbContext.Users.FirstOrDefault(user => user.UserId == UserId);
                ViewBag.LogUser = LogUser;
                return View("New");
            }
        }

        [HttpPost("new")]
        public IActionResult CreatePlan(Wedding wedding)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(wedding);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else {
                int? UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.UserId = UserId;
                User LogUser = dbContext.Users.FirstOrDefault(user => user.UserId == UserId);
                ViewBag.LogUser = LogUser;
                return View("New");
            }
        }
        
        [HttpGet("detail/{id}")]
        public IActionResult Detail(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null) 
            {
                return RedirectToAction("Index", "Home");
            } 
            else 
            {
                int? UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.UserId = UserId;
                User LogUser = dbContext.Users.FirstOrDefault(user => user.UserId == UserId);
                ViewBag.LogUser = LogUser;
                Wedding selectedWed = dbContext.Weddings.Include(w => w.RSVPs).ThenInclude(rsvp => rsvp.User).FirstOrDefault(w => w.WeddingId == id);
                IEnumerable<RSVP> Guests = selectedWed.RSVPs;
                ViewBag.Wed = selectedWed;
                ViewBag.Guests = Guests;
                return View("Detail");
            }
        }
    }
}