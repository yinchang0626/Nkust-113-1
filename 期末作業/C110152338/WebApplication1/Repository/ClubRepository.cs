using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class ClubRepository : IClubRepository
    {
        public ClubRepository()
        {
            
        }
        public bool Add(Club club)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Club club)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Club>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Club> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Updata(Club club)
        {
            throw new NotImplementedException();
        }
    }
}
