using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Client;
using BetterThanYou.Core.Interfaces.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Clients;

[Authorize]
public class Update : EndpointBaseAsync
    .WithRequest<ClientUpdateRequest>
    .WithActionResult<ClientUpdateResponse>
{
    private readonly IClientService _clientService;
    private readonly IFileStorageService _fileStorageService;

    public Update(IClientService clientService, IFileStorageService fileStorageService)
    {
        _clientService = clientService;
        _fileStorageService = fileStorageService;
    }
    
    [HttpPut("api/v1/clients")]
    [SwaggerOperation(
        Summary = "Update a client",
        Description = "Updates an existing client",
        OperationId = "Clients.Update",
        Tags = new[] { "Clients" })]
    public override async Task<ActionResult<ClientUpdateResponse>> HandleAsync(
        [FromForm] ClientUpdateRequest request,
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

        var clientDto = await _clientService.UpdateAsync(
            request.Id,
            request.NomeFantasia,
            request.RazaoSocial,
            request.Email,
            request.CpfCnpj,
            request.Endereco,
            request.Cidade,
            request.Estado,
            request.Cep,
            request.Telefones,
            fotoUrl,
            userId);

        var response = new ClientUpdateResponse
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