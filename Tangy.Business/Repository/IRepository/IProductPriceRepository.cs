using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy.Models.Dto;

namespace Tangy.Business.Repository.IRepository
{
    public interface IProductPriceRepository
    {
        Task<ProductPriceDto> Create(ProductPriceDto product);

        Task<ProductPriceDto> Update(ProductPriceDto product);

        Task<int> Delete(int id);

        Task<ProductPriceDto> Get(int id);

        Task<IEnumerable<ProductPriceDto>> GetAll(int? productId = null);
    }
}
