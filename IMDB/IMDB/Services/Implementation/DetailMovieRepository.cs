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
    public class DetailMovieRepository : IDetailMovieRepository
    {
        public async Task<MovieDetail> GetMovieDetailAsync(int id)
        {
            try
            {
                string key = $"MovieDetail#{id}";

                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    return Barrel.Current.Get<MovieDetail>(key: key);
                }

                if (!Barrel.Current.IsExpired(key))
                {
                    return Barrel.Current.Get<MovieDetail>(key);
                }

                var response = await ApiHelper.BASE_URL
                    .AppendPathSegment($"/{id}")
                    .SetQueryParams(new
                    {
                        api_key = ApiHelper.API_KEY,
                        language = ApiHelper.LANGUAGE
                    })
                    .GetAsync();

                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    var content = await response.ResponseMessage.Content.ReadAsStringAsync();
                    var movieDetail = JsonConvert.DeserializeObject<MovieDetail>(content);

                    if (!(movieDetail is null))
                        Barrel.Current.Add(key: key, data: movieDetail, expireIn: TimeSpan.FromMinutes(10));

                    return movieDetail;
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

            return new MovieDetail();
        }
    }
}
