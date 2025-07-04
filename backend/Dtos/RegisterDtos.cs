namespace backend.Dtos
{
    public class RegisterReq
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // "Teacher" or "Student"
    }
}