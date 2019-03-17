using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Core.API.Helpers;
using Core.Game;
using Core.Game.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMaze _maze;

        public GameController(IMaze maze)
        {
            _maze = maze;
        }

        [HttpGet("GenerateWorld")]
        public async Task<IActionResult> GenerateWorld([FromQuery]MazeParams mazeParams)
        {
            try
            {
                var world = await Task.Run(() =>
                {
                    // Generate the world
                    var generatedWorld = _maze.Generate(mazeParams.Columns, mazeParams.Rows);
                    // Create the maze inside world
                    _maze.CreateMaze(generatedWorld);
                    // Return world
                    return generatedWorld;
                });

                return Ok(world);
            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException());
            }
        }
    }
}