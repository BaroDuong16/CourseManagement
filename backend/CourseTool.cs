using backend.Models;
using backend.Repositories;
using ModelContextProtocol.Server;
using backend.Controllers;
using backend.Dtos;
using System.Security.Claims;
using System.ComponentModel;
using backend.Data;

[McpServerToolType]
public class CourseTool
{
    private readonly ICourseRepositories _courseRepo;
    private readonly IHttpContextAccessor _http;
    private readonly CMContext _context;

    public CourseTool(ICourseRepositories courseRepo, IHttpContextAccessor http, CMContext context)
    {
        _courseRepo = courseRepo;
        _http = http;
        _context = context;
    }

    [McpServerTool, Description("Lấy thông tin khóa học theo ID hoặc tên")]
    public async Task<string> GetCourseInfo(string idOrName)
    {
        // if (!Guid.TryParse(idOrName, out var id))
        //     return "❌ Mã khóa học không hợp lệ.";

        var course = await _courseRepo.GetCourseByIdOrNameAsync(idOrName);

        if (course == null)
            return "❌ Không tìm thấy khóa học.";

        return $" Khóa học: {course.CourseName}\n Mô tả: {course.Description} \n Giá: {course.Price} \n Ngày bắt đầu: {course.StartDate} \n Ngày kết thúc: {course.EndDate} \n Số lượng sinh viên đã đăng ký: {course.RegisteredStudentCount}\n Phòng: {course.Rooms.Select(r => r.RoomName).FirstOrDefault() ?? "Chưa đăng ký phòng học"} \n Giảng viên: {course.TeacherName} \n Số điện thoại: {course.PhoneNumber}";
    }


    [McpServerTool]
    public async Task<string> GetAllCourse()
    {
        var courses = await _courseRepo.GetAllCoursesAsync();

        if (courses == null || !courses.Any())
            return "❌ Không tìm thấy khóa học.";

        var courseInfo = courses.Select(course => $"ID: {course.CourseId}\n  Khóa học: {course.CourseName}\n Mô tả: {course.Description} \n Giá: {course.Price} \n Ngày bắt đầu: {course.StartDate} \n Ngày kết thúc: {course.EndDate} \n Số lượng sinh viên đã đăng ký: {course.RegisteredStudentCount} \n Phòng: {course.Rooms.Select(r => r.RoomName).FirstOrDefault() ?? "Chưa đăng ký phòng học"} \n Giảng viên: {course.TeacherName} \n Số điện thoại: {course.PhoneNumber}");
        return string.Join("\n\n", courseInfo);
        
    }


    [McpServerTool, Description("Tạo khóa học mới với các trường thời gian có định dạng chuẩn ISO 8601 với UTC")]
    public async Task<Course> CreateCourse(CourseDto dto)
    {
        var teacherId = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(teacherId))
            throw new Exception("Unauthenticated.");

        return await _courseRepo.CreateCourseAsync(dto, teacherId);
    }


    [McpServerTool, Description("Cập nhật khóa học theo yêu cầu và giữ nguyên các giá trị còn lại nếu không có trong dto")]
    public async Task<CourseDto?> UpdateCourse(string courseId, CourseDto updatedDto)
    {
        if (!Guid.TryParse(courseId, out var id))
            return null;

        var TeacherId = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(TeacherId))
            throw new Exception("Unauthenticated.");                

        return await _courseRepo.UpdateCourseByIdAsync(id.ToString(), updatedDto, TeacherId);
    }


    [McpServerTool, Description("Xoa khóa học")]
    public async Task<bool> DeleteCourse(string courseId)
    {
        if (!Guid.TryParse(courseId, out var id))
            return false;

        var course = await _courseRepo.GetCourseEntityByIdAsync(courseId);
        if (course == null || course.TeacherId != _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            return false;

        await _courseRepo.DeleteCourseAsync(courseId);
        return true;
    }
    // [McpServerTool, Description("xoa khoá học")]
    // public async Task<bool> DeleteCourse(string courseId)
    // {
    //     if (!Guid.TryParse(courseId, out var id))
    //         return false;

    //     var course = await _courseRepo.GetCourseEntityByIdAsync(courseId);
    //     if (course == null)
    //         return false;

    //     await _courseRepo.DeleteCourseAsync(courseId);
    //     return true;
    // }
}
