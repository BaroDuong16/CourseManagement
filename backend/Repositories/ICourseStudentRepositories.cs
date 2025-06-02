using backend.Models;
namespace backend.Repositories
{
    public interface ICourseStudentRepositories
    {
        Task<int> CountByCourseIdAsync(string courseId);
        Task<bool> ExistsAsync(string courseId, string studentId);
        Task RegisterStudentAsync(CourseStudent registration);
        Task<IEnumerable<CourseStudent>> GetStudentsByCourseAsync(string courseId);
        Task UnregisterStudentFromCourseAsync(string courseId, string studentId);
        // Task<IEnumerable<Course>> GetCoursesByStudentAsync(string studentId);
    }

}
