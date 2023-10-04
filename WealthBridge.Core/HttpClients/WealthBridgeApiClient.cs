using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WealthBridge.Core.Enums;
using WealthBridge.Core.Helpers;
using WealthBridge.Core.Models;

namespace WealthBridge.Core.HttpClients
{
    public partial class WealthBridgeApiClient
    {
        public readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }

        public Func<IDictionary<string, string>> HeaderHandler { get; set; }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        public WealthBridgeApiClient(Uri baseEndpoint)
        {
            BaseEndpoint = baseEndpoint ?? throw new ArgumentNullException("baseEndpoint");
            _httpClient = new HttpClient();
        }

        protected HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        protected FormUrlEncodedContent CreateFormContent<T>(T content)
        {
            return new FormUrlEncodedContent(content.ToKeyValue().AsEnumerable());
        }
        protected async Task<T> GetAsync<T>(Uri requestUrl, ApiReadType readType)
        {
            return await ProcessAsync<T, T>(requestUrl, default(T), "GET", readType: readType);
        }


        protected async Task<bool> HeadAsync(Uri requestUrl)
        {
            return await ProcessAsync<bool, object>(requestUrl, default(object), "HEAD");
        }

        protected async Task<bool> HeadAsync<T>(Uri requestUrl, T content)
        {
            return await ProcessAsync<bool, T>(requestUrl, default(T), "HEAD");
        }

        protected async Task<T> PostAsync<T>(Uri requestUrl, T content, ContentType contentType)
        {
            return await ProcessAsync<T, T>(requestUrl, content, "POST", contentType);
        }

        protected async Task<T> PostAsync<T>(Uri requestUrl)
        {
            return await ProcessAsync<T>(requestUrl, default(T), "POST");
        }
        protected async Task<T1> PostAsync<T1, T2>(Uri requestUrl, T2 content, ContentType type)
        {
            return await ProcessAsync<T1, T2>(requestUrl, content, "POST", type);
        }

        protected async Task<T> ProcessAsync<T>(Uri requestUrl, T content, string httpMethod)
        {
            return await ProcessAsync<T, T>(requestUrl, content, httpMethod);
        }

        protected async Task<T1> ProcessAsync<T1, T2>(Uri requestUrl, T2 content, string httpMethod, ContentType contentType = ContentType.StringContent, ApiReadType readType = ApiReadType.String)
        {
            var response = await RequestAsync<T1, T2>(requestUrl, content, httpMethod, contentType, readType);
            return response.Response;
        }

        protected async Task<ApiClientResponse<T1>> PostReqAsync<T1, T2>(Uri requestUrl, T2 content, ContentType type = ContentType.StringContent)
        {
            return await RequestAsync<T1, T2>(requestUrl, content, "POST", type);
        }
        protected async Task<ApiClientResponse<T1>> GetReqAsync<T1>(Uri requestUrl, ApiReadType readType = ApiReadType.String,bool isSerializationRequired = true)
        {
            return await RequestAsync<T1, T1>(requestUrl, default(T1), "GET",readType:readType,isSerializationRequired:isSerializationRequired);
        }

        protected async Task<ApiClientResponse<T1>> PatchReqAsync<T1, T2>(Uri requestUrl, T2 content)
        {
            return await RequestAsync<T1, T2>(requestUrl, content, "PATCH");
        }
        protected async Task<ApiClientResponse<T1>> DeleteReqAsync<T1>(Uri requestUrl, ApiReadType readType = ApiReadType.String, bool isSerializationRequired = true)
        {
            return await RequestAsync<T1, T1>(requestUrl, default(T1), "DELETE", readType: readType, isSerializationRequired: isSerializationRequired);
        }

        protected async Task<ApiClientResponse<T1>> RequestAsync<T1, T2>(Uri requestUrl, T2 content, string httpMethod,ContentType contentType = ContentType.StringContent, ApiReadType readType = ApiReadType.String, bool isSerializationRequired = true)
        {
            AddHeaders();
            HttpResponseMessage response;
            switch (httpMethod.ToUpper())
            {
                case "GET":
                    response = await _httpClient.GetAsync(requestUrl.ToString());
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return new ApiClientResponse<T1> { Response = default(T1) };
                    break;

                case "HEAD":
                    var httpReq = new HttpRequestMessage(HttpMethod.Head, requestUrl);
                    if (!EqualityComparer<T2>.Default.Equals(content, default(T2)))
                        httpReq.Content = CreateHttpContent(content);
                    response = await _httpClient.SendAsync(httpReq);
                    break;
                case "POST":
                    response = await _httpClient.PostAsync(requestUrl.ToString(), contentType == ContentType.StringContent ? CreateHttpContent(content) : CreateFormContent(content));
                    break;
                case "PUT":
                    response = await _httpClient.PutAsync(requestUrl.ToString(), CreateHttpContent(content));
                    break;
                case "DELETE":
                    response = await _httpClient.DeleteAsync(requestUrl.ToString());
                    break;
                case "PATCH":
                    response = await _httpClient.PatchAsync(requestUrl.ToString(), CreateHttpContent(content));
                    break;
                default:
                    throw new InvalidOperationException($"The http method {httpMethod} is unsupported");
            }

            ApiClientResponse<T1> apiClientResponse = new ApiClientResponse<T1>();
            apiClientResponse.StatusCode = Convert.ToInt32(response.StatusCode);

            if (typeof(T1) == typeof(bool))
            {
                apiClientResponse.Response = (T1)Convert.ChangeType(response.IsSuccessStatusCode, typeof(T1));
                return apiClientResponse;
            }
            else if (typeof(T1) == typeof(HttpStatusCode))
            {
                if (response.IsSuccessStatusCode)
                {
                    apiClientResponse.Response = (T1)Enum.ToObject(typeof(T1), HttpStatusCode.OK);
                }
                else
                {
                    var data = await response.Content.ReadAsStringAsync();

                    try
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            apiClientResponse.Response = (T1)Enum.ToObject(typeof(T1), Convert.ToInt32(data));
                        }
                        else
                        {
                            apiClientResponse.Response = (T1)Enum.ToObject(typeof(T1), Convert.ToInt32(response.StatusCode));
                        }
                    }
                    catch (Exception ex)
                    {
                        apiClientResponse.Response = (T1)Enum.ToObject(typeof(T1), Convert.ToInt32(response.StatusCode));
                    }

                    apiClientResponse.StatusCode = Convert.ToInt32(response.StatusCode);
                }
                return apiClientResponse;
            }
            else
            {
                if (readType == ApiReadType.Array)
                {
                    var array = await response.Content.ReadAsByteArrayAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            apiClientResponse.Response = (T1)Convert.ChangeType(Convert.ToBase64String(array), typeof(T1));
                            return apiClientResponse;
                        }
                        catch (Exception)
                        {
                            apiClientResponse.Response = (T1)Convert.ChangeType(Convert.ToBase64String(array), typeof(T1));
                            return apiClientResponse;
                        }
                    }
                    apiClientResponse.Error = new ApiClientResponseError
                    {
                        Error = "Conversion failure"
                    };
                    return apiClientResponse;
                }
                var data = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(data);
                        if (apiResponse is { Data: { } } && isSerializationRequired && apiResponse.ErrorCode ==0)
                        {
                            apiClientResponse.Response = JsonConvert.DeserializeObject<T1>(apiResponse.Data.ToString() ?? string.Empty);
                            return apiClientResponse;
                        }
                        apiClientResponse.Response = JsonConvert.DeserializeObject<T1>(data);
                        return apiClientResponse;
                    }
                    catch (JsonReaderException)
                    {
                        apiClientResponse.Response = (T1)Convert.ChangeType(data, typeof(T1));
                        return apiClientResponse;
                    }
                    catch
                    {
                        return apiClientResponse;
                    }
                }
                else
                {
                    apiClientResponse.Error = JsonConvert.DeserializeObject<ApiClientResponseError>(data);
                    return apiClientResponse;
                }
            }
        }
        public bool IsValidJson<T>(string request)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(request);
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p;

            List<string> result = new List<string>();
            foreach (var x in properties)
            {
                var property = (DataMemberAttribute)Attribute.GetCustomAttribute(x, typeof(DataMemberAttribute));
                if (x.PropertyType == typeof(string[]))
                {
                    var val = x.GetValue(obj, null) as string[];

                    foreach (var value in val)
                    {
                        result.Add(property.Name + "=" + HttpUtility.UrlEncode(value).ToString());
                    }
                }
                else
                {

                    result.Add(property.Name + "=" + HttpUtility.UrlEncode(x.GetValue(obj, null).ToString()));

                }

            }

            return string.Join("&", result);
        }
        protected Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        private void AddHeaders()
        {
            if (HeaderHandler == null) return;

            var headerPairs = HeaderHandler();

            if (headerPairs == null || !headerPairs.Any()) return;

            foreach (var item in headerPairs)
            {
                if (_httpClient.DefaultRequestHeaders.Any(k => k.Key == item.Key))
                    _httpClient.DefaultRequestHeaders.Remove(item.Key);

                if (item.Key == "Authorization")
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                else
                    _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);

            }
        }
        public void AddBasicToken(string usernameApi,string passwordApi)
        {
            var authenticationString = $"{usernameApi}:{passwordApi}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        }
    }
}
