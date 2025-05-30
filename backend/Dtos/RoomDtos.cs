namespace backend.Dtos
{
    public class RoomDto
    {
        public string RoomName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
    }
}