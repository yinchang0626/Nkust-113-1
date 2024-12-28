using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Data.Enum;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _context;
        public RaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Race race)
        {
            _context.Add(race);     // 並非新增，只是呼叫
            return Save();          // 真正存放資料
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);     // 並非新增，只是呼叫
            return Save();          // 真正存放資料
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCity(string city)
        {
            //return await _context.Races.Where(c => c.Address.City.Contains(city)).Distinct().ToListAsync();
            return await _context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Race?> GetByIdAsync(int id)
        {
            return await _context.Races.FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<Race?> GetByIdAsyncNoTracking(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountByCategoryAsync(RacesCategory category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Race>> GetRacesByCategoryAndSliceAsync(RacesCategory category, int offset, int size)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Race>> GetSliceAsync(int offset, int size)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}
