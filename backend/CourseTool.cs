using backend.Models;
using backend.Repositories;
using ModelContextProtocol.Server;
using backend.Controllers;
using backend.Dtos;
using System.Security.Claims;
using System.ComponentModel;
using backend.Data;
using Microsoft.EntityFrameworkCore;

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
    [McpServerTool, Description("Tạo khóa học mới - truyền teacherId và các thông tin khóa học riêng biệt voi với các trường thời gian có định dạng chuẩn ISO 8601 với UTC")]
    public async Task<Course> CreateCourseWithId(
        // string teacherId,
        string courseName,
        string description,
        int MaxStudentQuantity,
        decimal price,
        DateTime startDate,
        DateTime endDate
    )
    {
        var teacherId = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(teacherId))
            throw new Exception("Unauthenticated."); 

        var dto = new CourseDto
        {
            CourseName = courseName,
            Description = description,
            Price = price,
            MaxStudentQuantity = MaxStudentQuantity,
            StartDate = startDate,
            EndDate = endDate
        };

        return await _courseRepo.CreateCourseAsync(dto, teacherId);
    }
    [McpServerTool, Description("Cập nhật một hoặc nhiều thông tin khoá học")]
    public async Task<CourseDto?> UpdateCourseFlexible(
        string courseId,
        string? courseName = null,
        string? description = null,
        int ? maxStudentQuantity = null,
        decimal? price = null,
        DateTime? startDate = null,
        DateTime? endDate = null
    )
    {
        if (!Guid.TryParse(courseId, out var id))
            return null;

        var teacherId = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(teacherId))
            throw new Exception("Unauthenticated.");

        // ✅ Lấy dữ liệu cũ để merge
        var oldCourse = await _courseRepo.GetCourseEntityByIdAsync(courseId);
        if (oldCourse == null || oldCourse.TeacherId != teacherId)
            return null;

        var updatedDto = new CourseDto
        {
            CourseName = courseName ?? oldCourse.CourseName,
            Description = description ?? oldCourse.Description,
            MaxStudentQuantity = maxStudentQuantity ?? oldCourse.MaxStudentQuantity,
            Price = price ?? oldCourse.Price,
            StartDate = (DateTime)(startDate ?? oldCourse.StartDate),
            EndDate = (DateTime)(endDate ?? oldCourse.EndDate)
        };

        return await _courseRepo.UpdateCourseByIdAsync(courseId, updatedDto, teacherId);
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
    }
