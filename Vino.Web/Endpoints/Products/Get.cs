using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Products;

[Authorize]
public class Get : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<ProductGetResponse>
{
    private readonly IProductService _productService;

    public Get(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet("api/v1/products/{id}")]
    [SwaggerOperation(
        Summary = "Get product by ID",
        Description = "Returns a single product by its ID",
        OperationId = "Products.Get",
        Tags = new[] { "Products" })]
    public override async Task<ActionResult<ProductGetResponse>> HandleAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var product = await _productService.GetByIdAsync(id);
        
        return Ok(product);
    }
}