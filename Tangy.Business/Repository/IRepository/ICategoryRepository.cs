using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy.Models.Dto;

namespace Tangy.Business.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public CategoryDto Create(CategoryDto objDto);

        public CategoryDto Update(CategoryDto objDto);

        public int Delete(int id);

        public CategoryDto Get(int id);

        public IEnumerable<CategoryDto> GetAll();
    }
}
