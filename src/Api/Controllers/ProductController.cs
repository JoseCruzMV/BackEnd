using System.Net;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.PaginationProducts;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[Controller]")]
public class ProductController : ControllerBase
{
    private IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("list", Name = "GetProductList")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductVm>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<ProductVm>>> GetProductList()
    {
        var query = new GetProductListQuery();

        var productsList = await _mediator.Send(query);
        return Ok(productsList);
    }

    [AllowAnonymous]
    [HttpGet("pagination", Name = "PaginationProduct")]
    [ProducesResponseType(typeof(PaginationVm<ProductVm>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<ProductVm>>> PaginationProduct(
        [FromQuery] PaginationProductsQuery paginationProductsQuery
        )
    {
        paginationProductsQuery.Status = ProductStatus.Activo;
        var paginationProduct = await _mediator.Send(paginationProductsQuery);
        return Ok(paginationProduct);
    }
}