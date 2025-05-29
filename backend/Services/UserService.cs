using System.Security.Claims;
using System.Threading.Tasks;
using backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Validations;

public interface IUserService
{
    string GetUserId();
    string GetUserRole();
    bool IsInRole(string roleName);
}

public class UserService : IUserService
{
     private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
    }

    public string GetUserRole()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) ?? "";
    }

    public bool IsInRole(string roleName)
    {
        return _httpContextAccessor.HttpContext?.User?.IsInRole(roleName) ?? false;
    }
}
