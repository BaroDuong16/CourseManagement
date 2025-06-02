using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories
{
    public class CourseStudentRepositories : ICourseStudentRepositories
    {
        private readonly CMContext _context;

        public CourseStudentRepositories(CMContext context)
        {
            _context = context;
        }

        public async Task<int> CountByCourseIdAsync(string courseId)
        {
            return await _context.CourseStudents.CountAsync(cs => cs.CourseId == courseId);
        }

        public async Task<bool> ExistsAsync(string courseId, string studentId)
        {
            return await _context.CourseStudents.AnyAsync(cs => cs.CourseId == courseId && cs.StudentId == studentId);
        }

        public async Task RegisterStudentAsync(CourseStudent registration)
        {
            _context.CourseStudents.Add(registration);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseStudent>> GetStudentsByCourseAsync(string courseId)
        {
            return await _context.CourseStudents
                .Where(cs => cs.CourseId == courseId)
                .Include(cs => cs.Student)
                .ToListAsync();
        }

        public async Task UnregisterStudentFromCourseAsync(string courseId, string studentId)
        {
            var item = await _context.CourseStudents
                .FirstOrDefaultAsync(cs => cs.CourseId == courseId && cs.StudentId == studentId);

            if (item != null)
            {
                _context.CourseStudents.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
        // public async Task<IEnumerable<Course>> GetCoursesByStudentAsync(string studentId)
        // {
        //     return await _context.CourseStudents
        //     .Where(cs => cs.StudentId == studentId)
        //     .Include(cs => cs.Course)
        //         .ThenInclude(c => c.Teacher) // nếu muốn lấy luôn thông tin giáo viên
        //     .Select(cs => cs.Course)
        //     .ToListAsync();
        // }
    }
}


