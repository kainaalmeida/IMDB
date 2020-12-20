using Flurl;
using Flurl.Http;
using IMDB.Helpers;
using IMDB.Models;
using IMDB.Services.Contracts;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace IMDB.Services.Implementation
{
    public class TrendingRepository : ITrendingRepository
    {
        public async Task<Movies> GetTrendingMoviesAsync(int page)
        {
            try
            {
                var response = await ApiHelper.BASE_URL
                    .AppendPathSegment("/popular")
                    .SetQueryParams(new
                    {
                        api_key = ApiHelper.API_KEY,
                        language = ApiHelper.LANGUAGE,
                        page = page
                    })
                    .GetAsync();

                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    var content = await response.ResponseMessage.Content.ReadAsStringAsync();
                    var movies = JsonConvert.DeserializeObject<Movies>(content);
                    return movies;
                }

            }
            catch (FlurlHttpTimeoutException ex)
            {
                //Crashes.TrackError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
                throw ex;
            }

            return new Movies();
        }
    }
}
