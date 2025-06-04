namespace backend.Dtos
{
    public class CourseDetailRes
    {
        public string CourseId { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? MaxStudentQuantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<RoomDto> Rooms { get; set; } = new();
        public int RegisteredStudentCount { get; set; }

        public string? TeacherName { get; set; }
        public string? PhoneNumber { get; set; }
        // public string? CreatedUserName { get; set; }
        // public string? UpdatedUserName { get; set; }
    }
}
