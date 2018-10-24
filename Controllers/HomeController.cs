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
    public class HomeController : Controller
    {
        // Set up Context Service
        private WeddingContext dbContext;
        public HomeController(WeddingContext context) {
            dbContext = context;
        }

        // ROUTES
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost("")]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // unique email validation
                if (dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>(); // initialize a pwhasher obj
                user.Password = Hasher.HashPassword(user, user.Password); // save user obj to the db
                dbContext.Add(user);
                dbContext.SaveChanges();
                // SetObjectAsJson(this ISession session, string key, object value)
                SessionExtensions.SetObjectAsJson(HttpContext.Session, "currentUser", user);
                User currentUser = SessionExtensions.GetObjectFromJson<User>(HttpContext.Session, "currentUser");
                HttpContext.Session.SetInt32("UserId", currentUser.UserId);
                return RedirectToAction("Dashboard", "Dashboard");
            }
            return View("Index");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LogEmail);
                Console.WriteLine("Current login user: " + userInDb);
                // if user does not exist in db
                if (userInDb == null)
                {
                    ModelState.AddModelError("LogEmail", "Email does not exist");
                    return View("Index");
                }
                else 
                {
                    // initialize hasher obj
                    var hasher = new PasswordHasher<LoginUser>();
                    // varify input pw against hash in db
                    var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LogPassword);
                    // if wrong password
                    if (result == 0)
                    {
                        ModelState.AddModelError("LogPassword", "Invalid Password");
                        return View("Index");
                    }
                    else 
                    {
                        // else, store current user id in session
                        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
            } 
            return View("Index");
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
