using Tangy.Models.Dto;

namespace TangyWebClient.Service.IService
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDto>> GetAll(string? userId);

        public Task<OrderDto> Get(int orderHeaderId);
    }
}
