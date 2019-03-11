using System.Threading.Tasks;
using Core.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Core.API.Dtos;
using System.Collections.Generic;

namespace Core.API.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;
        public AdminRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<List<UserWithRoleDto>> GetUsersWithRoles()
        {
            var list = await (from user in _context.Users
                              orderby user.UserName
                              select new UserWithRoleDto
                              {
                                  Id = user.Id,
                                  UserName = user.UserName,
                                  Roles = (from userRole in user.UserRoles
                                           join role in _context.Roles
                                           on userRole.RoleId equals role.Id
                                           select role.Name).ToList()
                              }).ToListAsync();

            return list;
        }
    }
}