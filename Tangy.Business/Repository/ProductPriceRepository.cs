using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy.Business.Repository.IRepository;
using Tangy.DataAccess;
using Tangy.DataAccess.Data;
using Tangy.Models.Dto;

namespace Tangy.Business.Repository
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        #region Private Fields

        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="context">Db Context</param>
        public ProductPriceRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        #endregion
        public async Task<ProductPriceDto> Create(ProductPriceDto price)
        {
            ProductPrice productPrice = this.mapper.Map<ProductPriceDto, ProductPrice>(price);
            EntityEntry<ProductPrice> entity = this.dbContext.ProductPrices.Add(productPrice);
            await this.dbContext.SaveChangesAsync();

            return this.mapper.Map<ProductPrice, ProductPriceDto>(entity.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = this.dbContext.ProductPrices.FirstOrDefault(i => i.Id == id);
            if (obj != null)
            {
                this.dbContext.ProductPrices.Remove(obj);
                return await this.dbContext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductPriceDto> Get(int id)
        {
            ProductPrice? products = await this.dbContext.ProductPrices.Include(u => u.Product).FirstOrDefaultAsync(i => i.Id == id);
            if (products != null)
            {
                return this.mapper.Map<ProductPrice, ProductPriceDto>(products);
            }
            return new();
        }

        public async Task<IEnumerable<ProductPriceDto>> GetAll(int? productId = null)
        {
            if (productId != null && productId > 0)
            {
                return  this.mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDto>>(await this.dbContext.ProductPrices.Where(u => u.ProductId == productId).ToListAsync());
            }
            else
                return this.mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDto>>(this.dbContext.ProductPrices.Include(u => u.Product));
        }

        public async Task<ProductPriceDto> Update(ProductPriceDto objDto)
        {
            var products = await this.dbContext.ProductPrices.FirstOrDefaultAsync(i => i.Id == objDto.Id);
            if (products != null)
            {
                products.ProductId = objDto.ProductId;
                products.Size = objDto.Size;
                products.Price = objDto.Price;
                this.dbContext.Update(products);
                await this.dbContext.SaveChangesAsync();
                return this.mapper.Map<ProductPrice, ProductPriceDto>(products);
            }
            return objDto;
        }
    }
}
