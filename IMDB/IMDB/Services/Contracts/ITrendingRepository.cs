using IMDB.Models;
using System.Threading.Tasks;

namespace IMDB.Services.Contracts
{
    public interface ITrendingRepository
    {
        Task<Movies> GetTrendingMoviesAsync(int page = 1);
    }
}
