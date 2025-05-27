using backend.Models;
namespace backend.Repositories
{
    public interface ICourseRepositories
    {
        Task AddCourseAsync(Course course);
    }
}