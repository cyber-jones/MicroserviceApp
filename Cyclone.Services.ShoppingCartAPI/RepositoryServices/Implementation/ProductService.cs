
using Cyclone.Services.ShoppingCartAPI.DTOs;
using Cyclone.Services.ShoppingCartAPI.RepositoryServices.Abstraction;
using Newtonsoft.Json;

namespace Cyclone.Services.ShoppingCartAPI.RepositoryServices.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory) 
        { 
            _httpClientFactory = httpClientFactory;
        }



        public async Task<ResponseDto> GetProducts()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.GetAsync("/api/product");
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseDto>(content);
            }
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
