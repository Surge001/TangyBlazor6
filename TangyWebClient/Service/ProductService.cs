using Newtonsoft.Json;
using Tangy.Models.Dto;
using TangyWebClient.Service.IService;

namespace TangyWebClient.Service
{
    public class ProductService : IProductService
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;
        private string BaseServerUrl; 

        public ProductService(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
            this.BaseServerUrl = configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<ProductDto> Get(int id)
        {
            var response = await this.client.GetAsync($"/api/product/productId?productId={id}");

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                ProductDto product = JsonConvert.DeserializeObject<ProductDto>(content);
                product.ImageUrl = this.BaseServerUrl + product.ImageUrl;
                return product;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);
                throw new ApplicationException(errorModel.ErrorMessage);
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var response = await this.client.GetAsync("/api/product");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                IEnumerable<ProductDto> products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(content);
                foreach(ProductDto product in products)
                {
                    product.ImageUrl = this.BaseServerUrl + product.ImageUrl;
                }
                return products;
            }
            return new List<ProductDto>();
        }
    }
}
