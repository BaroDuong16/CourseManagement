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

            try
            {
                var createdCourse = await _courseRepo.CreateCourseAsync(courseDto, teacherId);
                return Ok(createdCourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            var userId = _userService.GetUserId();
            var result = await _courseRepo.UpdateCourseByIdAsync(id, updatedCourse, userId);

            if (result == null)
                return Forbid();

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
        [HttpGet("Get/{idOrName}")]
        public async Task<IActionResult> GetCoursebyIdOrName(string idOrName)
        {
            var course = await _courseRepo.GetCourseByIdOrNameAsync(idOrName);
            return course != null ? Ok(course) : NotFound();
        }
    }
}