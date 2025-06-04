namespace backend.Dtos
{
    public class RoomDetailDto
    {
        public string RoomId { get; set; } = null!;
        public string RoomName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? CreateDate { get; set; } = null!;
        public List<CourseDto> Course { get; set; } = new();

    }
}