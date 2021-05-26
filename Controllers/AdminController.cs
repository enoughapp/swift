using Microsoft.AspNetCore.Mvc;

namespace Swift.Controllers
{
    public class AdminController : Controller 
    {

        [Route("/admin")]
        public IActionResult Admin() 
        {
            //get all users details
            return View();
        }        

        public void ExportToExcel()
        {
           
        }

        public void ExportToCSV()
        {

        }
    }
}