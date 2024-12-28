using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAll();
        Task<Club> GetByIdAsync(int id);
        Task<IEnumerable<Club>> GetClubByCity(string city);

        bool Add(Club club);
        bool Updata(Club club);
        bool Delete(Club club);
        bool Save();
    }
}
