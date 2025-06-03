using backend.Models;
using backend.Dtos;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomCourseController : ControllerBase
    {
        private readonly IRoomCourseRepositories _roomCourseRepositories;
        private readonly CMContext _context;

        public RoomCourseController(IRoomCourseRepositories roomCourseRepositories, CMContext context)
        {
            _roomCourseRepositories = roomCourseRepositories;
            _context = context;
        }

        // POST: api/RoomCourse/assign
        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoom([FromBody] RoomCourseDto request)
        {
            // 1. Kiểm tra khóa học có tồn tại không và có thông tin thời gian không
            var course = await _context.Courses.FindAsync(request.CourseId);
            if (course == null || course.StartDate == null || course.EndDate == null)
            {
                return BadRequest("Không tìm thấy khóa học hoặc khóa học thiếu thông tin thời gian.");
            }

            // 2. Kiểm tra trùng lịch phòng học
            bool isAvailable = await _roomCourseRepositories.IsRoomAvailableAsync(
                request.RoomId,
                course.StartDate.Value,
                course.EndDate.Value
            );

            if (!isAvailable)
            {
                return BadRequest("Phòng đã được sử dụng trong khoảng thời gian này.");
            }

            // 3. Gán phòng cho khóa học
            var roomCourse = new RoomCourse
            {
                RoomCourseId = Guid.NewGuid().ToString(),
                RoomId = request.RoomId,
                CourseId = request.CourseId,
                CreateDate = DateTime.UtcNow
            };

            await _roomCourseRepositories.AssignRoomToCourseAsync(roomCourse);
            return Ok("Đăng ký phòng học thành công.");
        }
    }
}
