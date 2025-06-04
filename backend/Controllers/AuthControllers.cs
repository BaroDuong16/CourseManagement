using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Dtos;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterReq model)
    {
        var result = await _authService.RegisterAsync(model);
        if (!result)
            return BadRequest("Username hoặc Role không hợp lệ");
        return Ok("Đăng ký thành công");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginReq model)
    {
        var token = await _authService.LoginAsync(model);
        if (token == null)
            return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng");
        return Ok(new { Token = token });
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet("teacher")]
    public IActionResult TeacherOnly()
    {
        return Ok("Chỉ giáo viên mới xem được dữ liệu này");
    }

    [Authorize(Roles = "Student")]
    [HttpGet("student")]
    public IActionResult StudentOnly()
    {
        return Ok("Chỉ sinh viên mới xem được dữ liệu này");
    }
}
