using Tangy.Models.Dto;

namespace TangyWebClient.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAll();

        Task<ProductDto> Get(int id);
    }
}
