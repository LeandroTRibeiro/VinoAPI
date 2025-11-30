using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.File;
using BetterThanYou.Core.Interfaces.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Products;

[Authorize]
public class Update : EndpointBaseAsync
    .WithRequest<UpdateRequest>
    .WithActionResult<UpdateResponse>
{
    private readonly IProductService _productService;
    private readonly IFileStorageService _fileStorageService;

    public Update(IProductService productService, IFileStorageService fileStorageService)
    {
        _productService = productService;
        _fileStorageService = fileStorageService;
    }
    
    [HttpPut("api/v1/products")]  // ← SEM {id}
    [SwaggerOperation(
        Summary = "Update a product",
        Description = "Updates an existing product",
        OperationId = "Products.Update",
        Tags = new[] { "Products" })]
    public override async Task<ActionResult<UpdateResponse>> HandleAsync(
        [FromForm] UpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                     ?? throw new UnauthorizedAccessException("Usuário não identificado");

        string? fotoUrl = null;

        if (request.Foto != null)
        {
            using var stream = request.Foto.OpenReadStream();
            fotoUrl = await _fileStorageService.SaveImageAsync(stream, request.Foto.FileName);
        }
        
        var productDto = await _productService.UpdateAsync(
            request.Id,
            request.Nome, 
            request.Descricao, 
            request.QuantidadeEstoque,
            request.PrecoCusto, 
            request.PrecoVenda, 
            fotoUrl,
            userId);

        var response = new UpdateResponse
        {
            Id = productDto.Id,
            Nome = productDto.Nome,
            Descricao = productDto.Descricao,
            QuantidadeEstoque = productDto.QuantidadeEstoque,
            PrecoCusto = productDto.PrecoCusto,
            PrecoVenda = productDto.PrecoVenda,
            FotoUrl = productDto.FotoUrl,
            CriadoPor = productDto.CriadoPor,
            DataCriacao = productDto.DataCriacao
        };

        return Ok(response);
    }
}