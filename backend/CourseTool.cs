using backend.Models;
using backend.Repositories;
using ModelContextProtocol.Server;
using backend.Controllers;
using backend.Dtos;
using System.Security.Claims;
using System.ComponentModel;

[McpServerToolType]
public class CourseTool
{
    private readonly ICourseRepositories _courseRepo;
    private readonly IHttpContextAccessor _http;

    public CourseTool(ICourseRepositories courseRepo, IHttpContextAccessor http)
    {
        _courseRepo = courseRepo;
        _http = http;
    }

    [McpServerTool]
    public async Task<string> GetCourseInfo(string courseId)
    {
        if (!Guid.TryParse(courseId, out var id))
            return "❌ Mã khóa học không hợp lệ.";

        var course = await _courseRepo.GetCourseByIdAsync(courseId);

        if (course == null)
            return "❌ Không tìm thấy khóa học.";

        return $" Khóa học: {course.CourseName}\n Mô tả: {course.Description} \n Giá: {course.Price} \n Ngày bắt đầu: {course.StartDate} \n Ngày kết thúc: {course.EndDate} \n Số lượng sinh viên đã đăng ký: {course.RegisteredStudentCount}"; ;
    }
    [McpServerTool]
    public async Task<string> GetAllCourse()
    {
        var courses = await _courseRepo.GetAllCoursesAsync();

        if (courses == null || !courses.Any())
            return "❌ Không tìm thấy khóa học.";

        var courseInfo = courses.Select(course => $" Khóa học: {course.CourseName}\n Mô tả: {course.Description} \n Giá: {course.Price} \n Ngày bắt đầu: {course.StartDate} \n Ngày kết thúc: {course.EndDate} \n Số lượng sinh viên đã đăng ký: {course.RegisteredStudentCount}");
        return string.Join("\n\n", courseInfo);
    }
    
    [McpServerTool]
    public async Task<Course> CreateCourse(CourseDto dto)
    {
        var teacherId = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(teacherId))
            throw new Exception("Unauthenticated.");

        return await _courseRepo.CreateCourseAsync(dto, teacherId);
    }
}
