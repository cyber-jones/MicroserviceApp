using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Cyclone.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Cyclone.RepositoryService.Implementation
{
	public class BaseService : IBaseService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ITokenProvider _tokenProvider;

		public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }




        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true)
		{
			try
			{
				HttpClient httpClient = _httpClientFactory.CreateClient("CyloneAPI");
				HttpRequestMessage httpRequestMessage = new();

				httpRequestMessage.Headers.Add("Accept", "application/json");

				if (withBearer)
				{
                    string token = _tokenProvider.GetToken();
                    httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");
                }

				httpRequestMessage.RequestUri = new Uri(requestDto.Url);

				if (requestDto.Data != null)
				{
					httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), encoding: Encoding.UTF8, mediaType:"application/json");
				}

				httpRequestMessage.Method = requestDto.ApiType switch
				{
					SD.ApiType.POST => HttpMethod.Post,
					SD.ApiType.PUT => HttpMethod.Put,
					SD.ApiType.DELETE => HttpMethod.Delete,
					_ => HttpMethod.Get,
				};

				HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
				
				switch(httpResponseMessage.StatusCode)
				{
					case HttpStatusCode.NotFound:
						return new() { Success = false, Message = "Not Found" };
					case HttpStatusCode.Unauthorized:
						return new() { Success = false, Message = "Unauthorized" };
					case HttpStatusCode.Forbidden:
						return new() { Success = false, Message = "Access Denied" };
					case HttpStatusCode.InternalServerError:
						return new() { Success = false, Message = "Internal ServerError" };
					default:
						var data = await httpResponseMessage.Content.ReadAsStringAsync();
						ResponseDto? response = JsonConvert.DeserializeObject<ResponseDto>(data);
						return response;
				}
			}
			catch (Exception ex)
			{
				ResponseDto responseDto = new()
				{
					Message = ex.Message,
					Success = false,
				};

				return responseDto;
			}
		}
	}
}
