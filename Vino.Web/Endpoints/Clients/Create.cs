using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Entities;
using BetterThanYou.Core.Interfaces.Client;
using BetterThanYou.Core.Interfaces.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Clients;

[Authorize]
public class Create : EndpointBaseAsync
    .WithRequest<ClientCreateRequest>
    .WithActionResult<ClientCreateResponse>
{
    private readonly IClientService _clientService;
    private readonly IFileStorageService _fileStorageService;

    public Create(IClientService clientService, IFileStorageService fileStorageService)
    {
        _clientService = clientService;
        _fileStorageService = fileStorageService;
    }
    
    [HttpPost("api/v1/clients")]
    [SwaggerOperation(
        Summary = "Create a new client",
        Description = "Creates a new client",
        OperationId = "Clients.Create",
        Tags = new[] { "Clients" })]
    public override async Task<ActionResult<ClientCreateResponse>> HandleAsync(
        [FromForm] ClientCreateRequest request,
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

        // Deserializar telefones
        List<ContactPhone> telefones = new List<ContactPhone>();
        if (!string.IsNullOrEmpty(request.Telefones))
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                };
                telefones = System.Text.Json.JsonSerializer.Deserialize<List<ContactPhone>>(request.Telefones, options) 
                            ?? new List<ContactPhone>();
            }
            catch
            {
                // Se falhar, continua com lista vazia
            }
        }

        var clientDto = await _clientService.CreateAsync(
            request.NomeFantasia,
            request.RazaoSocial,
            request.Email,
            request.CpfCnpj,
            request.Endereco,
            request.Cidade,
            request.Estado,
            request.Cep,
            telefones,
            fotoUrl,
            userId);

        var response = new ClientCreateResponse
        {
            Id = clientDto.Id,
            NomeFantasia = clientDto.NomeFantasia,
            RazaoSocial = clientDto.RazaoSocial,
            Email = clientDto.Email,
            CpfCnpj = clientDto.CpfCnpj,
            Endereco = clientDto.Endereco,
            Cidade = clientDto.Cidade,
            Estado = clientDto.Estado,
            Cep = clientDto.Cep,
            FotoUrl = clientDto.FotoUrl,
            Telefones = clientDto.Telefones,
            CriadoPor = clientDto.CriadoPor,
            DataCriacao = clientDto.DataCriacao
        };

        return Ok(response);
    }
}