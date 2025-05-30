using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories
{
    public class RoomRepositories : IRoomRepositories
    {
        private readonly CMContext _context;
        public RoomRepositories(CMContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            return await _context.Rooms
            .Include(r => r.RoomCourses)
            .Select(static c => new RoomDto
            {
                RoomName = c.RoomName,
                Description = c.Description,
                CreateDate = (DateTime)c.CreateDate,
            })
            .ToListAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(string roomId)
        {
            return await _context.Rooms
                .Include(r => r.RoomCourses)
                .FirstOrDefaultAsync(r => r.RoomId == roomId);
        }

        public async Task AddRoomAsync(Room room)
        {
            room.RoomId = Guid.NewGuid().ToString();
            room.CreateDate = DateTime.UtcNow;
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoomAsync(Room room)
        {
            room.UpdateDate = DateTime.UtcNow;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoomAsync(string RoomId)
        {
            var room = await _context.Rooms.FindAsync(RoomId);
            if (room == null)
            {
               throw new KeyNotFoundException($"Room with {RoomId} is not found");
            }
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }
}