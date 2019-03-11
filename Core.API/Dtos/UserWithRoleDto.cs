using System.Collections.Generic;

namespace Core.API.Dtos
{
    public class UserWithRoleDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}