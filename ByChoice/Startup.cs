using ByChoice.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ByChoice.Startup))]
namespace ByChoice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUser();
        }
        //In this method we Create default User and Admin user for login
        private void CreateRolesAndUser()
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Create Super Admin for the Web Adminstrator
                var user = new ApplicationUser();
                user.UserName = "melakumen@gmail.com";
                user.Email = "melakumen@gmail.com";
                user.TaxNumber = "0123456789";
                user.FirstName = "Melaku";
                user.LastName = "Michael";
                user.Country = "Ethiopia";
                user.RegionId = 10;
                user.TaxNumber = "0123456789";

                string userPassword = "BabiBoo@12";
                var chkUser = UserManager.Create(user, userPassword);
                // Add Default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }
            //Creating Manager Role
            //if (!roleManager.RoleExists("Officer"))
            //{
            //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            //    role.Name = "Officer";
            //    roleManager.Create(role);

            //}

            //if (!roleManager.RoleExists("MicroFinance"))
            //{
            //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            //    role.Name = "MicroFinance";
            //    roleManager.Create(role);

            //}

            if (!roleManager.RoleExists("Agent"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Agent";
                roleManager.Create(role);
            }
        }
    }
}
