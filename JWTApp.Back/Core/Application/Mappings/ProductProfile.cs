using AutoMapper;
using JWTApp.Back.Core.Application.Dto;
using JWTApp.Back.Core.Domain;

namespace JWTApp.Back.Core.Application.Mappings
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            this.CreateMap<Product, ProductListDto>().ReverseMap();
        }
    }
}
