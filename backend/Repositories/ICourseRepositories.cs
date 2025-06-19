using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories
{
    public interface ICourseRepositories
    {
        Task<IEnumerable<CourseDetailRes>> GetAllCoursesAsync();
        Task<CourseDetailRes?> GetCourseByIdAsync(string id);
        Task<CourseDetailRes?> GetCourseByIdOrNameAsync(string idOrName);
        Task<Course?> GetCourseEntityByIdAsync(string courseId);
        Task<Course> CreateCourseAsync(CourseDto dto, string teacherId);
        // Task UpdateCourseAsync(Course course);
        Task<CourseDto?> UpdateCourseByIdAsync(string courseId, CourseDto updatedDto, string userId);
        Task DeleteCourseAsync(string CourseId);
        // Task<IEnumerable<CourseDetailDto>> SearchCoursesAsync(string? keyword, DateTime? from, DateTime? to);
    }
}