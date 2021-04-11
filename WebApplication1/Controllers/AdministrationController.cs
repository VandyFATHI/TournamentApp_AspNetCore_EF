using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdministrationController : Controller
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();
        private  ApplicationRoleManager _roleManager;

        public AdministrationController(ApplicationRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public AdministrationController()
        {

        }


        [HttpGet]
        public ActionResult CreateRole()
        {
            return View(new CreateRoleViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!RoleManager.RoleExists(model.RoleName))
                {
                    var role = new ApplicationRole() { Name = model.RoleName };
                    IdentityResult result = await RoleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.msgError = "This role already exist";
                }

            }
            return View();
        }
        
        
    }
}