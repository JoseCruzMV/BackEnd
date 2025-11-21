using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specifications.Products;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.PaginationProducts;

public class PaginationProductsQueryHandler : IRequestHandler<PaginationProductsQuery, PaginationVm<ProductVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaginationProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationVm<ProductVm>> Handle(PaginationProductsQuery request, CancellationToken cancellationToken)
    {
        var productSpecificationParams = new ProductSpecificationParams
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            Sort = request.Sort,
            Search = request.Search,
            CategoryId = request.CategoryId,
            PrecioMax = request.PrecioMax,
            PrecioMin = request.PrecioMin,
            Rating = request.Rating,
            Status = request.Status
        };

        var specification = new ProductSpecification(productSpecificationParams);
        var products = await _unitOfWork.Repository<Product>().GetAllWithSpec(specification);
        
        var specificationCount = new ProductForCountingSpecification(productSpecificationParams);
        var totalProducts = await _unitOfWork.Repository<Product>().CountAsync(specificationCount);

        var rounded = Math.Ceiling((decimal)totalProducts / (decimal)request.PageSize);
        var totalPages = (int)rounded;

        var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductVm>>(products);

        var productByPage = products.Count;

        var paginationVm = new PaginationVm<ProductVm>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            Count = totalProducts,
            PageCount = totalPages,
            ResultByPage = productByPage,
            Data = data
        };

        return paginationVm;
    }
}