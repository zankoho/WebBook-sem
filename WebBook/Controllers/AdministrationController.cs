using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBook.Data;
using WebBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBook.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace WebBook.Controllers

{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
    }
}
