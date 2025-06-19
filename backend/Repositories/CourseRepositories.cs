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
                    TeacherName = c.Teacher.FullName,
                    PhoneNumber = c.Teacher.PhoneNumber
                })
                .ToListAsync();
        }
        // public async Task AddCourseAsync(Course course)
        // {
        //     course.CourseId = Guid.NewGuid().ToString();
        //     course.CreateDate = DateTime.UtcNow;
        //     _context.Courses.Add(course);
        //     await _context.SaveChangesAsync();
        // }
        public async Task<Course> CreateCourseAsync(CourseDto dto, string teacherId)
        {
            // Kiểm tra teacherId có tồn tại
            var teacherExists = await _context.AspNetUsers.AnyAsync(u => u.Id == teacherId);
            if (!teacherExists)
                throw new Exception($"TeacherId from token: {teacherId} không tồn tại");

            var course = new Course
            {
                CourseId = Guid.NewGuid().ToString(),
                CourseName = dto.CourseName,
                Description = dto.Description,
                Price = dto.Price,
                MaxStudentQuantity = dto.MaxStudentQuantity,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TeacherId = teacherId,
                CreateDate = DateTime.UtcNow,
                CreatedUserId = teacherId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course;
        }
        // public async Task UpdateCourseAsync(Course course)
        // {
        //     course.UpdateDate = DateTime.UtcNow;
        //     // DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        //     _context.Courses.Update(course);
        //     await _context.SaveChangesAsync();
        // }
        public async Task<CourseDto?> UpdateCourseByIdAsync(string courseId, CourseDto updatedDto, string userId)
        {
            var course = await GetCourseEntityByIdAsync(courseId);
            if (course == null || course.TeacherId != userId)
                return null;

            course.CourseName = updatedDto.CourseName;
            course.Description = updatedDto.Description;
            course.Price = updatedDto.Price;
            course.MaxStudentQuantity = updatedDto.MaxStudentQuantity;
            course.StartDate = updatedDto.StartDate;
            course.EndDate = updatedDto.EndDate;
            course.UpdateDate = DateTime.UtcNow;
            course.UpdatedUserId = userId;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            // Trả về CourseDto sau khi cập nhật
            return new CourseDto
            {
                CourseName = course.CourseName,
                Description = course.Description,
                Price = course.Price,
                MaxStudentQuantity = course.MaxStudentQuantity,
                StartDate = (DateTime)course.StartDate,
                EndDate = (DateTime)course.EndDate
            };
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
                .Include(c => c.Teacher)
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
                RegisteredStudentCount = course.CourseStudents.Count,
                TeacherName = course.Teacher.FullName,
                PhoneNumber = course.Teacher.PhoneNumber
            };
        }
        public async Task<CourseDetailRes?> GetCourseByIdOrNameAsync(string idOrName)
        {
            Course? course;

            // Nếu là GUID → tìm theo CourseId
            if (Guid.TryParse(idOrName, out _))
            {
                course = await _context.Courses
                    .Include(c => c.RoomCourses).ThenInclude(rc => rc.Room)
                    .Include(c => c.CourseStudents)
                    .Include(c => c.Teacher)
                    .FirstOrDefaultAsync(c => c.CourseId == idOrName);
            }
            else
            {
                // Nếu là tên → tìm theo CourseName
                course = await _context.Courses
                    .Include(c => c.RoomCourses).ThenInclude(rc => rc.Room)
                    .Include(c => c.CourseStudents)
                    .Include(c => c.Teacher)
                    .FirstOrDefaultAsync(c => c.CourseName == idOrName);
            }

            if (course == null)
                return null;

            // Mapping sang DTO nếu cần (nếu bạn dùng AutoMapper, hoặc tự map)
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
                RegisteredStudentCount = course.CourseStudents.Count,
                TeacherName = course.Teacher.FullName,
                PhoneNumber = course.Teacher.PhoneNumber
            };
        }


    }
}