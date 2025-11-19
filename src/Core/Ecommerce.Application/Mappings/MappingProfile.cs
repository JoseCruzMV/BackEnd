using AutoMapper;
using Ecommerce.Application.Features.Images.Queries.Vms;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Reviews.Queries.Vms;
using Ecommerce.Domain;

namespace Ecommerce.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductVm>()
            .ForMember(p => p.CategoryNombre, opt => opt.MapFrom(a => a.Category!.Nombre))
            .ForMember(p => p.NumeroReviews, opt => opt.MapFrom(a => a.Reviews == null ? 0 : a.Reviews.Count));
        
        CreateMap<Image, ImageVm>();            
        CreateMap<Review, ReviewVm>();
    }
}