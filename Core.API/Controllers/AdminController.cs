using System.Linq;
using System.Threading.Tasks;
using Core.API.Data;
using Core.API.Dtos;
using Core.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Core.API.Helpers.Constants;

namespace Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repo;
        private readonly UserManager<User> _userManager;
        public AdminController(IAdminRepository repo, UserManager<User> userManager)
        {
            this._userManager = userManager;
            this._repo = repo;
        }


        [Authorize(Policy = Policy.RequireAdmin)]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await _repo.GetUsersWithRoles();
            return Ok(userList);
        }

        [Authorize(Policy = Policy.RequireAdmin)]
        [HttpPost("EditRoles/{id}")]
        public async Task<IActionResult> EditRoles(int id, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roleEditDto.RoleNames ?? new string[] {};

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to add roles to user");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to remove roles from user");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = Policy.ModeratePhoto)]
        [HttpGet("photosForMOderation")]
        public async Task<IActionResult> GetPhotosForModeration()
        {
            return Ok("Only admins or moderators can see this");
        }
    }
}