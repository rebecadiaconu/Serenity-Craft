using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Serenity_Craft.Models;

[assembly: OwinStartupAttribute(typeof(Serenity_Craft.Startup))]
namespace Serenity_Craft
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateAdminAndUserRoles();
        }

        private void CreateAdminAndUserRoles()
        {
            var ctx = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(ctx));
            
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "reb.d31@gmail.com";
                user.Email = "reb.d31@gmail.com";
                var adminCreated = userManager.Create(user, "rebeca03.");
                if (adminCreated.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }

    }
}
