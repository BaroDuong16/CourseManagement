using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories
{
    public interface ICourseRepositories
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(string id);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(string CourseId);
    }
}