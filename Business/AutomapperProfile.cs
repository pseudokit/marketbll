using AutoMapper;
using Business.Models;
using Business.Services;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
                .ForMember(pm => pm.ReceiptDetailIds,
                           p => p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ReverseMap();

            CreateMap<Customer, CustomerModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Person.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(x => x.Person.Surname))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(x => x.Person.BirthDate))
                .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(x => x.DiscountValue))
                .ForMember(dest => dest.ReceiptsIds, opt => opt.MapFrom(x => x.Receipts.Select(r => r.Id)))
                .ReverseMap();

            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(x => x.CategoryName))
                .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(x => x.Products.Select(p => p.Id)))
                .ReverseMap();
        }
    }
}