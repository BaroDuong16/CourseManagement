using backend.Models;

namespace backend.Repositories
{
    public interface IRoomCourseRepositories
    {
        Task<bool> IsRoomAvailableAsync(string roomId, DateTime startDate, DateTime endDate);
        Task<bool> AssignRoomToCourseAsync(RoomCourse roomCourse);
    }
}
