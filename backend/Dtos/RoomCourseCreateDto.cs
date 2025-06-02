namespace backend.Dtos
{
    public class RoomCourseCreateDto
    {
        public string RoomId { get; set; } = null!;
        public string CourseId { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
