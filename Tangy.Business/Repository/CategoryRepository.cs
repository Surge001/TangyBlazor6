using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tangy.Business.Repository.IRepository;
using Tangy.DataAccess.Data;
using Tangy.Models.Dto;

namespace Tangy.Business.Repository
{
    public class CategoryRepository : ICategoryRepository
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
        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        #endregion

        public CategoryDto Create(CategoryDto objDto)
        {
            Category category = this.mapper.Map<CategoryDto, Category>(objDto);
            category.CreatedDate = DateTime.Now;
            EntityEntry<Category> entity = this.dbContext.Categories.Add(category);
            this.dbContext.SaveChanges();

            return this.mapper.Map<Category, CategoryDto>(entity.Entity);
        }

        public int Delete(int id)
        {
            var obj = this.dbContext.Categories.FirstOrDefault(i => i.Id == id);
            if (obj != null)
            {
                this.dbContext.Categories.Remove(obj);
                return this.dbContext.SaveChanges();
            }
            return 0;
        }

        public CategoryDto Get(int id)
        {
            Category? category = this.dbContext.Categories.FirstOrDefault(i => i.Id == id);
            if (category != null)
            {
                return this.mapper.Map<Category, CategoryDto>(category);
            }
            return new();
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            return this.mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(this.dbContext.Categories);
        }
         
        public CategoryDto Update(CategoryDto objDto)
        {
            var category = this.dbContext.Categories.FirstOrDefault(i => i.Id == objDto.Id);
            if (category != null)
            {
                category.Name = objDto.Name;
                this.dbContext.Update(category);
                this.dbContext.SaveChanges();
                return this.mapper.Map<Category, CategoryDto>(category);
            }
            return objDto;
        }
    }
}
