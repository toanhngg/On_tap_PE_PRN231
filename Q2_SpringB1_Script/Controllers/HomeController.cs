using Microsoft.AspNetCore.Mvc;
using Q2_SpringB1_Script.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Q2_SpringB1_Script.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult EmployeeWithSkills()
        {
            return View();
        } 

    }
}