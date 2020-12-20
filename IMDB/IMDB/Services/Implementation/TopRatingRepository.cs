using Flurl;
using Flurl.Http;
using IMDB.Helpers;
using IMDB.Models;
using IMDB.Services.Contracts;
using MonkeyCache.LiteDB;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace IMDB.Services.Implementation
{
    public class TopRatingRepository : ITopRatingRepository
    {
        public async Task<Movies> GetTopRatingMoviesAsync(int page = 1)
        {
            try
            {
                string key = $"TopRating#{page}";

                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    return Barrel.Current.Get<Movies>(key: key);
                }

                if (!Barrel.Current.IsExpired(key))
                {
                    return Barrel.Current.Get<Movies>(key);
                }

                var response = await ApiHelper.BASE_URL
                    .AppendPathSegment("/top_rated")
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

                    if (!(movies.results is null))
                        Barrel.Current.Add(key: key, data: movies, expireIn: TimeSpan.FromMinutes(10));

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
