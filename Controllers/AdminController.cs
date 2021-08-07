using System;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swift.Models;

namespace Swift.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataContext _context;
        public AdminController(DataContext context)
        {
            _context = context;
        }

        [Route("/admin")]
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            
            if(HttpContext.Session.GetString("username") == null) 
            {
                return RedirectToAction("Login", "Account");
            } 
            else if (users == null) 
            {
                return NotFound();
            }
            else 
            { 
                return View(users);
            }
        }
        
        [HttpGet]
        [Route("/edit/{id}")]
        public ViewResult Edit(int id)
        {
            return View();
        }

        [HttpPut]
        public IActionResult Edit(int id, UserAccount user)
        {
            string strUsername = HttpContext.Session.GetString("username");
            // Update Statement
            var update = _context.Users.Where(o => o.Username == strUsername).FirstOrDefault();
            update.Username = user.Username;
            update.Email = user.Email;
        
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            UserAccount user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ExportToExcel()
        {
            var users = _context.Users.ToList();

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "users.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                workbook.Worksheets.Add("Users");
                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Username";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "Dob";
                worksheet.Cell(1, 5).Value = "Occupation";
                worksheet.Cell(1, 6).Value = "Income";
                worksheet.Cell(1, 7).Value = "SocialMediaLink";
                worksheet.Cell(1, 8).Value = "Telephone";
                worksheet.Cell(1, 9).Value = "CreatedAt";
                
                for (int index = 1; index <= users.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value =
                    users[index - 1].UserId;
                    worksheet.Cell(index + 1, 2).Value =
                    users[index - 1].Username;
                    worksheet.Cell(index + 1, 3).Value =
                    users[index - 1].Email;
                    worksheet.Cell(index + 1, 4).Value =
                    users[index - 1].Dob;
                    worksheet.Cell(index + 1, 5).Value =
                    users[index - 1].Occupation;
                    worksheet.Cell(index + 1, 6).Value =
                    users[index - 1].Income;
                    worksheet.Cell(index + 1, 7).Value =
                    users[index - 1].SocialMediaLink;
                    worksheet.Cell(index + 1, 8).Value =
                    users[index - 1].Telephone;
                    worksheet.Cell(index + 1, 9).Value =
                    users[index - 1].CreatedAt;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
    }
}