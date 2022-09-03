using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy.DataAccess;
using Tangy.DataAccess.Data;
using Tangy.Models.Dto;

namespace Tangy.Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}
