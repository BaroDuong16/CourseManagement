using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories
{
    public class CourseRepositories : ICourseRepositories
    {
        private readonly CMContext _context;
        public CourseRepositories(CMContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            return await _context.Courses
            .Include(c => c.Teacher)
            .Select(static c => new CourseDto
            {
                CourseName = c.CourseName,
                Description = c.Description,
                Price = c.Price,
                MaxStudentQuantity =c.MaxStudentQuantity,
                StartDate = (DateTime)c.StartDate,
                EndDate = (DateTime)c.EndDate,
            })
            .ToListAsync();

        }
        public async Task AddCourseAsync(Course course)
        {
            
            course.CourseId = Guid.NewGuid().ToString();
            course.CreateDate = DateTime.UtcNow;
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCourseAsync(Course course)
        {
            course.UpdateDate = DateTime.UtcNow;
            // DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCourseAsync(string CourseId)
        {
            var course = await _context.Courses.FindAsync(CourseId);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with {CourseId} is not found");
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
        public async Task<Course?> GetCourseByIdAsync(string courseId)
        {
            return await _context.Courses
                .Include(c => c.CourseStudents)
                .Include(c => c.RoomCourses)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }
    }
}