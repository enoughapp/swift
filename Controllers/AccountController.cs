using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swift.Models;

namespace Swift.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/register")]
        public ViewResult Register()
        { 
            if(HttpContext.Session.GetString("username") == null) 
            {
                return View();
            } else 
            {
                return View("Profile");
            }
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                var user = new UserAccount 
                { 
                    Username = account.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(account.Password),
                    Email = account.Email,
                    Dob = account.Dob, 
                    Occupation =account.Occupation,
                    Income = account.Income,
                    SocialMediaLink = account.SocialMediaLink,
                    Telephone = account.Telephone,
                    CreatedAt = DateTime.Now
                };
                _context.Add(user);
                _context.SaveChanges(); 
                return RedirectToAction("Login");
            }
            return View();
        }

        [Route("/login")]
        public IActionResult Login(UserAccount userAccount)
        {
            if(HttpContext.Session.GetString("username") == null) 
            {
                return View();
            } 
            else 
            {
                return View("Profile");
            }
        }

        [HttpPost]
        public IActionResult LoginAction(UserAccount account)
        {
            var verifyUser = LoginProcess(account.Username, account.Password);
            if (verifyUser == null)
            {
                return RedirectToAction("ErrorAccount");
            }
            else
            {
                HttpContext.Session.SetString("username", account.Username);
                return RedirectToAction("Profile");
            }
        }

        private UserAccount LoginProcess(string username, string password)
        {
            var account = _context.Users.SingleOrDefault(a => a.Username.Equals(username));
            if(account != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    return account;
                }
            }
            return null;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Login");
        }

        [Route("/profile")]
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("ErrorAccount");
            }
            return View();
        }

        [Route("/error-account")]
        public IActionResult ErrorAccount()
        {
            return View();
        }

    }
}