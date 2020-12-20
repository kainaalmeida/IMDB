using IMDB.Models;
using System.Threading.Tasks;

namespace IMDB.Services.Contracts
{
    public interface IIncomingRepository
    {
        Task<Movies> GetIncomingMoviesAsync(int page = 1);
    }
}
