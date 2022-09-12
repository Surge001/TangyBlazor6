using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tangy.Common;
using Tangy.DataAccess.Data;
using TangyWeb.Server.Service.IService;

namespace TangyWeb.Server.Service
{
    public class DbInitializer : IDbInitializer
    {
        #region Private Fields

        private readonly UserManager<IdentityUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly ApplicationDbContext dbContext;

        #endregion

        #region

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }

        #endregion

        public void Initialize()
        {
            try
            {
                if (this.dbContext.Database.GetPendingMigrations().Any())
                {
                    this.dbContext.Database.Migrate();

                    if (!this.roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        // await this.roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)); --> Same as line below!
                        this.roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                        this.roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                    }
                    else
                    {
                        return;
                    }

                    // Create Admin user now:
                    IdentityUser user = new()
                    {
                        UserName = "Admin@Tangy.com",
                        Email = "Admin@TAngy.com",
                        EmailConfirmed = true
                    };
                    this.userManager.CreateAsync(user, "TangyDotnet*6").GetAwaiter().GetResult();
                    this.userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                }
            }
            catch (Exception err)
            {

            }
        }
    }
}
