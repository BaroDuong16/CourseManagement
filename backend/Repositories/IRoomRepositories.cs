using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories
{
    public interface IRoomRepositories
    {
        Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
        Task<Room?> GetRoomByIdAsync(string id);
        Task AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room course);
        Task DeleteRoomAsync(string RoomId);
    }
}