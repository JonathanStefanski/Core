using System.Collections.Generic;
using System.Linq;
using Core.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using static Core.API.Helpers.Constants;

namespace Core.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public void SeedDatabase()
        {
            SeedRoles();
            SeedUsers();
        }

        private void SeedRoles()
        {
            var roles = UserRoles.GetUserRoles();

            foreach (var role in roles)
            {
                _roleManager.CreateAsync(role).Wait();
            }
        }

        private void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, UserRoles.Member).Wait();
                }

                var adminUser = new User
                {
                    UserName = "Admin"
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("admin").Result;
                    _userManager.AddToRolesAsync(admin, new[] { UserRoles.Admin, UserRoles.Moderator });
                }
            }
        }
    }
}