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
    [Authorize(Roles = "Student, Teacher")]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ICourseRepositories _courseRepo;
        private readonly ICourseStudentRepositories _courseStudentRepo;

        public StudentController(ICourseRepositories courseRepo, ICourseStudentRepositories courseStudentRepo)
        {
            _courseRepo = courseRepo;
            _courseStudentRepo = courseStudentRepo;
        }

        [HttpPost("register/{courseId}")]
        public async Task<IActionResult> RegisterToCourse(string courseId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier); // lấy student từ JWT
            if (string.IsNullOrEmpty(studentId)) return Unauthorized("Không xác định được sinh viên");

            var course = await _courseRepo.GetCourseByIdAsync(courseId);
            if (course == null) return NotFound("Khóa học không tồn tại");

            var currentCount = await _courseStudentRepo.CountByCourseIdAsync(courseId);
            if (course.MaxStudentQuantity.HasValue && currentCount >= course.MaxStudentQuantity.Value)
                return BadRequest("Khóa học đã đạt giới hạn số sinh viên");

            var existed = await _courseStudentRepo.ExistsAsync(courseId, studentId);
            if (existed)
                return BadRequest("Bạn đã đăng ký khóa học này rồi");

            var courseStudent = new CourseStudent
            {
                CourseStudentId = Guid.NewGuid().ToString(),
                CourseId = courseId,
                StudentId = studentId,
                CreateDate = DateTime.UtcNow,
                CreatedUserId = studentId
            };

            await _courseStudentRepo.RegisterStudentAsync(courseStudent);
            return Ok("Đăng ký thành công");
        }
        // [HttpGet("my-courses")]
        // public async Task<IActionResult> GetMyCourses()
        // {
        //     var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     if (string.IsNullOrEmpty(studentId)) return Unauthorized("Không xác định được sinh viên");

        //     var courses = await _courseStudentRepo.GetCoursesByStudentAsync(studentId);
        //     return Ok(courses);
        // }
    }
}
