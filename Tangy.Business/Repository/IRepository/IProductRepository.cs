using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy.Models.Dto;

namespace Tangy.Business.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<ProductDto> Create(ProductDto product);

        Task<ProductDto> Update(ProductDto product);

        Task<int> Delete(int id);

        Task<ProductDto> Get(int id);

        Task<IEnumerable<ProductDto>> GetAll();
    }
}
