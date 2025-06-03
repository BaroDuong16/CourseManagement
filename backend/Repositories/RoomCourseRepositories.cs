using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories
{
    public class RoomCourseRepositories : IRoomCourseRepositories
    {
        private readonly CMContext _context;

        public RoomCourseRepositories(CMContext context)
        {
            _context = context;
        }

        // Kiểm tra xem phòng có trống trong khoảng thời gian không
        public async Task<bool> IsRoomAvailableAsync(string roomId, DateTime startDate, DateTime endDate)
        {
            return !await _context.RoomCourses
                .Include(rc => rc.Course) // cần Include để lấy thông tin thời gian từ Course
                .AnyAsync(rc =>
                    rc.RoomId == roomId &&
                    rc.Course.StartDate.HasValue && rc.Course.EndDate.HasValue &&
                    (
                        (startDate >= rc.Course.StartDate && startDate < rc.Course.EndDate) ||
                        (endDate > rc.Course.StartDate && endDate <= rc.Course.EndDate) ||
                        (startDate <= rc.Course.StartDate && endDate >= rc.Course.EndDate)
                    )
                );
        }

        // Gán phòng cho khóa học nếu trống
        public async Task<bool> AssignRoomToCourseAsync(RoomCourse roomCourse)
        {
            _context.RoomCourses.Add(roomCourse);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
