using Newtonsoft.Json;
using Tangy.Models.Dto;
using TangyWebClient.Service.IService;

namespace TangyWebClient.Service
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;
        private string BaseServerUrl;

        public OrderService(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
            this.BaseServerUrl = configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<OrderDto> Get(int orderHeaderId)
        {
            var response = await this.client.GetAsync($"/api/order/orderHeaderId?orderHeaderId={orderHeaderId}");

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                OrderDto order = JsonConvert.DeserializeObject<OrderDto>(content);
                return order;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);
                throw new ApplicationException(errorModel.ErrorMessage);
            }
        }

        public async Task<IEnumerable<OrderDto>> GetAll(string? userId)
        {
            var response = await this.client.GetAsync("/api/order");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                IEnumerable<OrderDto> order = JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(content);
                return order;
            }
            return new List<OrderDto>();
        }
    }
}
