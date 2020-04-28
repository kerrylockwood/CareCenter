using CareData;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GraceCareCenterOrder.Startup))]
namespace GraceCareCenterOrder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        // Method to create default User Roles
        private void CreateRolesAndUsers()
        {
            ApplicationDbContext ctx = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));

            // At startup, create first Admin Role and a default Admin User
            if (!roleManager.RoleExists("Admin"))
            {
                // Create Admin role
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // Create Admin Super User who will maintain the website
                var user = new ApplicationUser();
                user.UserName = "Admin";
                user.Email = "KerryLockwood@yahoo.com";

                string userPWD = "Password-0";
                var chkUser = userManager.Create(user, userPWD);

                // Add default Admin to Admin Role
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Admin");
                }
            }

            // At startup, create first Associate Role and a default Admin User
            if (!roleManager.RoleExists("Associate"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Associate";
                roleManager.Create(role);
            }

            // At startup, create first Customer Role and a default Admin User
            if (!roleManager.RoleExists("Customer"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Customer";
                roleManager.Create(role);
            }
        }
    }
}
