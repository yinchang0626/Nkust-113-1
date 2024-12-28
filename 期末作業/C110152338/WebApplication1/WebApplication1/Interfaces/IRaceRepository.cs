using WebApplication1.Data.Enum;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IRaceRepository
    {
        Task<int> GetCountAsync();

        Task<int> GetCountByCategoryAsync(RacesCategory category);

        Task<Race?> GetByIdAsync(int id);

        Task<Race?> GetByIdAsyncNoTracking(int id);

        Task<IEnumerable<Race>> GetAll();

        Task<IEnumerable<Race>> GetAllRacesByCity(string city);
        Task<IEnumerable<Race>> GetSliceAsync(int offset, int size);

        Task<IEnumerable<Race>> GetRacesByCategoryAndSliceAsync(RacesCategory category, int offset, int size);

        bool Add(Race race);

        bool Update(Race race);

        bool Delete(Race race);

        bool Save();
    }
}
