
public interface IUserService
{
    string GetUserId();
    string GetUserRole();
    bool IsInRole(string roleName);
}