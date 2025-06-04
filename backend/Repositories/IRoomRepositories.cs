using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories
{
    public interface IRoomRepositories
    {
        Task<IEnumerable<RoomDetailDto>> GetAllRoomsAsync();
        Task<RoomDetailDto?> GetRoomByIdAsync(string id);
        Task<Room?> GetRoomEntityByIdAsync(string roomId);
        Task AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room course);
        Task DeleteRoomAsync(string RoomId);

    }
}