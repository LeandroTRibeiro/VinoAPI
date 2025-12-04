using BetterThanYou.Core.Entities;

namespace BetterThanYou.Web.Endpoints.Clients;

public class ClientUpdateRequest
{
    public Guid Id { get; set; }
    public required string NomeFantasia { get; set; }
    public string? RazaoSocial { get; set; }
    public string? Email { get; set; }
    public string? CpfCnpj { get; set; }
    public string? Endereco { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Cep { get; set; }
    public string? Telefones { get; set; } 
    public IFormFile? Foto { get; set; }
}