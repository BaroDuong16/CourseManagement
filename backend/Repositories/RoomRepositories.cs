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
        public async Task<IEnumerable<RoomDetailDto>> GetAllRoomsAsync()
        {
            return await _context.Rooms
            .Include(r => r.RoomCourses).ThenInclude(rc => rc.Course)
            .Select(static c => new RoomDetailDto
            {
                RoomId = c.RoomId,
                RoomName = c.RoomName,
                Description = c.Description,
                CreateDate = (DateTime)c.CreateDate,
                Course = c.RoomCourses.Select(rc => new CourseDto
                {
                    CourseName = rc.Course.CourseName,
                    Description = rc.Course.Description,
                    Price = rc.Course.Price,
                    MaxStudentQuantity = rc.Course.MaxStudentQuantity,
                    StartDate = (DateTime)rc.Course.StartDate,
                    EndDate = (DateTime)rc.Course.EndDate
                }).ToList()
            }).ToListAsync();
        }

        public async Task<RoomDetailDto?> GetRoomByIdAsync(string roomId)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomCourses).ThenInclude(rc => rc.Course)
                .FirstOrDefaultAsync(r => r.RoomId == roomId);
            if (room == null)
                return null;
            return new RoomDetailDto
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                Description = room.Description,
                CreateDate = room.CreateDate,
                Course = room.RoomCourses.Select(rc => new CourseDto
                {
                    CourseName = rc.Course.CourseName,
                    Description = rc.Course.Description,
                    Price = rc.Course.Price,
                    MaxStudentQuantity = rc.Course.MaxStudentQuantity,
                    StartDate = (DateTime)rc.Course.StartDate,
                    EndDate = (DateTime)rc.Course.EndDate
                }).ToList()
            };
        }
        public async Task<Room?> GetRoomEntityByIdAsync(string roomId)
        {
            return await _context.Rooms
            .FirstOrDefaultAsync(c => c.RoomId == roomId);
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