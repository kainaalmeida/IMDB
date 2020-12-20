using IMDB.Models;
using System.Threading.Tasks;

namespace IMDB.Services.Contracts
{
    public interface ITopRatingRepository
    {
        Task<Movies> GetTopRatingMoviesAsync(int page = 1);
    }
}
