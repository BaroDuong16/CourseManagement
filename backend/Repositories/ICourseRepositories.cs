using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories
{
    public interface ICourseRepositories
    {
        Task<IEnumerable<CourseDetailDto>> GetAllCoursesAsync();
        Task<CourseDetailDto?> GetCourseByIdAsync(string id);
        Task<Course?> GetCourseEntityByIdAsync(string courseId);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(string CourseId);
        // Task<IEnumerable<CourseDetailDto>> SearchCoursesAsync(string? keyword, DateTime? from, DateTime? to);
    }
}