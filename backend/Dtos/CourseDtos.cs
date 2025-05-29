namespace backend.Dtos
{
    public class CourseDto
    {
        public string CourseName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? MaxStudentQuantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}