using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Dtos;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace backend.Controllers
{
    [Authorize(Roles = "Teacher")]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepositories _roomRepo;
        private readonly IUserService _userService;

        public RoomController(IRoomRepositories roomRepo, IUserService userService)
        {
            _roomRepo = roomRepo;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomDto roomDto)
        {

            var room = new Room
            {
                RoomId = Guid.NewGuid().ToString(),
                RoomName = roomDto.RoomName,
                Description = roomDto.Description,
                CreateDate = DateTime.UtcNow,
                CreatedUserId = _userService.GetUserId(),
            };
            await _roomRepo.AddRoomAsync(room);
            return Ok(room);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDetailDto>>> GetAllRooms()
        {
            var rooms = await _roomRepo.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(string id)
        {
            var room = await _roomRepo.GetRoomByIdAsync(id);
            return room != null ? Ok(room) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(string id, [FromBody] RoomDto updatedRoom)
        {
            var room = await _roomRepo.GetRoomEntityByIdAsync(id);
            if (room == null) return NotFound();

            room.RoomName = updatedRoom.RoomName;
            room.Description = updatedRoom.Description;
            room.UpdateDate = DateTime.UtcNow;
            room.UpdatedUserId = _userService.GetUserId();

            await _roomRepo.UpdateRoomAsync(room);

            var result = await _roomRepo.GetRoomByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("{RoomId}")]
        public async Task<IActionResult> DeleteRoom(string RoomId)
        {
            var room = await _roomRepo.GetRoomByIdAsync(RoomId);
            if (room == null) return NotFound();

            await _roomRepo.DeleteRoomAsync(room.RoomId);
            return NoContent();
        }
    }
}
