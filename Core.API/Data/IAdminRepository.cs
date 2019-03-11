using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API.Dtos;

namespace Core.API.Data
{
    public interface IAdminRepository
    {
         Task<List<UserWithRoleDto>> GetUsersWithRoles();
    }
}