using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Roles.Count() == 0)
                {
                    var Role1 = new ApplicationRole();
                    var Role2 = new ApplicationRole();

                    Role1.Name = "Admin";
                    Role2.Name = "User";

                    db.Roles.Add(Role1);
                    db.Roles.Add(Role2);
                    db.SaveChanges();

                }
            }
        
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
