using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tangy.Business.Repository.IRepository;
using Tangy.DataAccess;
using Tangy.DataAccess.Data;
using Tangy.Models.Dto;

namespace Tangy.Business.Repository
{
    public class ProductRepository : IProductRepository
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
        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        #endregion

        public async Task<ProductDto> Create(ProductDto objDto)
        {
            Product product = this.mapper.Map<ProductDto, Product>(objDto);
            EntityEntry<Product> entity = this.dbContext.Products.Add(product);
            await this.dbContext.SaveChangesAsync();

            return this.mapper.Map<Product, ProductDto>(entity.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = this.dbContext.Products.FirstOrDefault(i => i.Id == id);
            if (obj != null)
            {
                this.dbContext.Products.Remove(obj);
                return await this.dbContext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductDto> Get(int id)
        {
            Product? products = await this.dbContext.Products.Include(u => u.Category).Include(i => i.ProductPrices).FirstOrDefaultAsync(i => i.Id == id);
            if (products != null)
            {
                return this.mapper.Map<Product, ProductDto>(products);
            }
            return new();
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return this.mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(this.dbContext.Products.Include(u => u.Category).Include(i => i.ProductPrices));
        }
         
        public async Task<ProductDto> Update(ProductDto objDto)
        {
            var products = await this.dbContext.Products.FirstOrDefaultAsync(i => i.Id == objDto.Id);
            if (products != null)
            {
                products.Name = objDto.Name;
                products.CustomerFavorites = objDto.CustomerFavorites;
                products.ShopFavorites = objDto.ShopFavorites;
                products.Color = objDto.Color;
                //products.Category = objDto.Category;
                products.CategoryId = objDto.CategoryId;
                products.Description = objDto.Description;
                products.ImageUrl = objDto.ImageUrl;
                this.dbContext.Update(products);
                await this.dbContext.SaveChangesAsync();
                return this.mapper.Map<Product, ProductDto>(products);
            }
            return objDto;
        }
    }
}
