using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WeatherApp.Core.Models;

namespace WeatherApp.Core
{
    public class RequestManager
    {
        private HttpClient _client;
        private readonly JsonSerializerSettings _jsonCamelCaseSettings;

        private static readonly Lazy<RequestManager> LazyRequestManager = new Lazy<RequestManager>();
        public static RequestManager Instance => LazyRequestManager.Value;

        public RequestManager()
        {
            //Determine which HttpClient to Initialize
            ConfigureHttpClient();
            
            //Configure Json.Net
            _jsonCamelCaseSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        private void ConfigureHttpClient()
        {
            //Cleanup any potentially old reference to HttpClient
            _client?.Dispose();

            //Create new HttpClient
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(30000)
            };
        }

        public async Task<ApiResult<T>> GetApiAsync<T>(string url)
        {
            var apiResult = new ApiResult<T>();

            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };

                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

                using (HttpResponseMessage response = await _client.SendAsync(request).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        apiResult.IsSuccess = true;

                        using (Stream s = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            if (s.Length > 0)
                            {
                                using (var sr = new StreamReader(s))
                                {
                                    using (JsonReader reader = new JsonTextReader(sr))
                                    {
                                        JsonSerializer serializer = JsonSerializer.Create(_jsonCamelCaseSettings);
                                        apiResult.SuccessResult = serializer.Deserialize<T>(reader);
                                    }
                                }
                            }
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        apiResult.IsSuccess = true;
                    }
                    else
                    {
                        apiResult.IsSuccess = false;

                        try
                        {
                            string errorMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                            if (errorMessage.Length > 0)
                            {
                                try
                                {
                                    apiResult.ErrorResult =
                                        JsonConvert.DeserializeObject<ErrorResponse>(errorMessage, _jsonCamelCaseSettings);
                                }
                                catch (Exception)
                                {
                                    //No Json response message - use raw string
                                    apiResult.ErrorResult = new ErrorResponse
                                    {
                                        Code = ((int) response.StatusCode).ToString(),
                                        Message = errorMessage
                                    };
                                }
                            }
                            else
                            {
                                //No response Message
                                apiResult.ErrorResult = new ErrorResponse
                                    {Code = ((int) response.StatusCode).ToString()};
                            }
                        }
                        catch (Exception)
                        {
                            //No response Message
                            apiResult.ErrorResult = new ErrorResponse {Code = ((int) response.StatusCode).ToString()};
                        }
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                apiResult.IsSuccess = false;
                apiResult.ErrorResult = new ErrorResponse {Code = "REQUEST_CANCELED", Message = ex.Message};
            }
            catch (HttpRequestException ex)
            {
                apiResult.IsSuccess = false;
                apiResult.ErrorResult = new ErrorResponse {Code = "HTTP_ERROR", Message = ex.Message};
            }
            catch (Exception ex)
            {
                apiResult.IsSuccess = false;
                apiResult.ErrorResult = new ErrorResponse {Code = "EXCEPTION", Message = ex.Message};
            }

            return apiResult;
        }
    }
}