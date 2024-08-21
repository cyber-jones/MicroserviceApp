
using Cyclone.Services.ShoppingCartAPI.DTOs;

namespace Cyclone.Services.ShoppingCartAPI.RepositoryServices.Abstraction
{
    public interface IProductService
    {
        Task<ResponseDto> GetProducts();
    }
}
