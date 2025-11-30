using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Products;

[Authorize]
public class Delete : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult
{
    private readonly IProductService _productService;

    public Delete(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpDelete("api/v1/products/{id}")]
    [SwaggerOperation(
        Summary = "Delete (deactivate) a product",
        Description = "Deactivates a product (soft delete)",
        OperationId = "Products.Delete",
        Tags = new[] { "Products" })]
    public override async Task<ActionResult> HandleAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }
}