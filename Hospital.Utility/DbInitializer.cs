using Hospital.DataAccess;
using Hospital.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Utility
{
    public class DbInitializer : IDbInitializer
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if(_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();   
                }

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            if (!_roleManager.RoleExistsAsync(Roles.WebSite_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Roles.WebSite_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.WebSite_Patient)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.WebSite_Doctor)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Rayhan",
                    Email = "masudrayhan@yahoo.com"
                }, "Rayhan123").GetAwaiter().GetResult();


                var appUser = _context.ApplicationUsers.FirstOrDefault(x =>x.Email=="masudrayhan@yahoo.com");

                if (appUser != null)
                {
                    _userManager.AddToRoleAsync(appUser,Roles.WebSite_Admin);
                }



            }
        }
    }
}
