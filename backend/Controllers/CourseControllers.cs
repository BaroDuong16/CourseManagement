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
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepositories _courseRepo;
        private readonly IUserService _userService;
        private readonly CMContext _context;

        public CourseController(ICourseRepositories courseRepo, IUserService userService, CMContext context)
        {
            _courseRepo = courseRepo;
            _userService = userService;
            _context = context;
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseDto courseDto)
        {
            var teacherId = _userService.GetUserId();
            if (string.IsNullOrEmpty(teacherId))
            return Unauthorized("User not authenticated.");

            //  Kiểm tra xem ID có tồn tại trong bảng AspNetUsers không
            var teacherExists = await _context.AspNetUsers.AnyAsync(u => u.Id == teacherId);
            if (!teacherExists)
                return BadRequest($"teacherId from token: {teacherId}");
            var course = new Course
            {
                CourseId = Guid.NewGuid().ToString(),
                CourseName = courseDto.CourseName,
                Description = courseDto.Description,
                Price = courseDto.Price,
                MaxStudentQuantity = courseDto.MaxStudentQuantity,
                StartDate = courseDto.StartDate,
                EndDate = courseDto.EndDate,
                TeacherId = teacherId,
                CreateDate = DateTime.UtcNow,
                CreatedUserId = _userService.GetUserId()
            };

            await _courseRepo.AddCourseAsync(course);
            return Ok(course);
        }
        [Authorize(Roles = "Teacher,Student")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDetailRes>>> GetAllCourses()
        {
            var courses = await _courseRepo.GetAllCoursesAsync();
            return Ok(courses);
        }
        [Authorize(Roles = "Teacher,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(string id)
        {
            var course = await _courseRepo.GetCourseByIdAsync(id);
            return course != null ? Ok(course) : NotFound();
        }
        [Authorize(Roles = "Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(string id, [FromBody] CourseDto updatedCourse)
        {
            var course = await _courseRepo.GetCourseEntityByIdAsync(id); 
            if (course == null || course.TeacherId != _userService.GetUserId()) return Forbid();

            // Cập nhật thuộc tính
            course.CourseName = updatedCourse.CourseName;
            course.Description = updatedCourse.Description;
            course.Price = updatedCourse.Price;
            course.MaxStudentQuantity = updatedCourse.MaxStudentQuantity;
            course.StartDate = updatedCourse.StartDate;
            course.EndDate = updatedCourse.EndDate;
            course.UpdateDate = DateTime.UtcNow;
            course.UpdatedUserId = _userService.GetUserId();

            await _courseRepo.UpdateCourseAsync(course);

            // Trả về DTO giống với GetCourseByIdAsync
            var result = await _courseRepo.GetCourseByIdAsync(id); // 
            return Ok(result);
        }
        [Authorize(Roles = "Teacher")]
        [HttpDelete("{CourseId}")]
        public async Task<IActionResult> DeleteCourse(string CourseId)
        {
            var course = await _courseRepo.GetCourseEntityByIdAsync(CourseId);
            if (course == null || course.TeacherId != _userService.GetUserId()) return Forbid();

            await _courseRepo.DeleteCourseAsync(course.CourseId);
            return NoContent();
        }
    }
}