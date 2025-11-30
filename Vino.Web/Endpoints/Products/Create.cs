using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.File;
using BetterThanYou.Core.Interfaces.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Products;

[Authorize]
public class Create : EndpointBaseAsync
    .WithRequest<ProductCreateRequest>
    .WithActionResult<ProductCreateResponse>
{
    private readonly IProductService _productService;
    private readonly IFileStorageService _fileStorageService;

    public Create(
        IProductService productService,
        IFileStorageService fileStorageService)
    {
        _productService = productService;
        _fileStorageService = fileStorageService;
    }
    
    [HttpPost("api/v1/products")]
    [SwaggerOperation(
        Summary = "Create a new product",
        Description = "Creates a new product in the system",
        OperationId = "Products.Create",
        Tags = new[] { "Products" })]
    public override async Task<ActionResult<ProductCreateResponse>> HandleAsync(
        [FromForm]ProductCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                     ?? throw new UnauthorizedAccessException("Usuário não identificado");
        
        string? fotoUrl = null;

        if (request.Foto != null)
        {
            using var stream = request.Foto.OpenReadStream();
            fotoUrl = await _fileStorageService.SaveImageAsync(stream, request.Foto.FileName);
        };
        
        var productDto = await _productService.CreateAsync(
            request.Nome, 
            request.Descricao, 
            request.QuantidadeEstoque,
            request.PrecoCusto, 
            request.PrecoVenda, 
            fotoUrl,
            userId);

        var response = new ProductCreateResponse
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

        return Created($"api/v1/products/{response.Id}", response);
    }
}