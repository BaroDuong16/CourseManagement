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
        public async Task<IEnumerable<CourseDetailRes>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.RoomCourses).ThenInclude(rc => rc.Room)
                .Include(c => c.CourseStudents)
                .Include(c => c.Teacher)
                .Select(c => new CourseDetailRes
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    Description = c.Description,
                    Price = c.Price,
                    MaxStudentQuantity = c.MaxStudentQuantity,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Rooms = c.RoomCourses.Select(rc => new RoomDto
                    {
                        RoomName = rc.Room.RoomName,
                        Description = rc.Room.Description,
                        CreateDate = (DateTime)rc.Room.CreateDate
                    }).ToList(),
                    RegisteredStudentCount = c.CourseStudents.Count,
                    TeacherName = c.Teacher.FullName
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
        public async Task<Course?> GetCourseEntityByIdAsync(string courseId)
        {
            return await _context.Courses
            .FirstOrDefaultAsync(c => c.CourseId == courseId);
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
        public async Task<CourseDetailRes?> GetCourseByIdAsync(string courseId)
        {
            var course = await _context.Courses
                .Include(c => c.RoomCourses).ThenInclude(rc => rc.Room)
                .Include(c => c.CourseStudents)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
            if (course == null)
                return null;

            return new CourseDetailRes
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = course.Description,
                Price = course.Price,
                MaxStudentQuantity = course.MaxStudentQuantity,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                Rooms = course.RoomCourses.Select(rc => new RoomDto
                {
                    RoomName = rc.Room.RoomName,
                    Description = rc.Room.Description,
                    CreateDate = (DateTime)rc.Room.CreateDate
                }).ToList(),
                RegisteredStudentCount = course.CourseStudents.Count
            };
        }
    }
}