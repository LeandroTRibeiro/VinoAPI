using BetterThanYou.Core.Entities;

namespace BetterThanYou.Web.Endpoints.Clients;

public class ClientCreateRequest
{
    public required string NomeFantasia { get; set; }
    public string? RazaoSocial { get; set; }
    public string? Email { get; set; }
    public string? CpfCnpj { get; set; }
    public string? Endereco { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Cep { get; set; }
    public List<ContactPhone> Telefones { get; set; } = new List<ContactPhone>();
    public IFormFile? Foto { get; set; }
}
