using IMDB.Models;
using System.Threading.Tasks;

namespace IMDB.Services.Contracts
{
    public interface IDetailMovieRepository
    {
        Task<MovieDetail> GetMovieDetailAsync(int id);
    }
}
