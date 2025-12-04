using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Products;

[Authorize]
public class List : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ProductListResponse>
{
    private readonly IProductService _productService;

    public List(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet("api/v1/products")]
    [SwaggerOperation(
        Summary = "List all products",
        Description = "Returns a list of all products",
        OperationId = "Products.List",
        Tags = new[] { "Products" })]
    public override async Task<ActionResult<ProductListResponse>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        var products = await _productService.GetAllAsync();
        
        return Ok(new ProductListResponse { Products = products });
    }
}