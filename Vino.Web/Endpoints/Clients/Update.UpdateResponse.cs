using BetterThanYou.Core.Entities;

namespace BetterThanYou.Web.Endpoints.Clients;

public class ClientUpdateResponse
{
    public Guid Id { get; set; }
    public string NomeFantasia { get; set; } = string.Empty;
    public string? RazaoSocial { get; set; }
    public string? Email { get; set; }
    public string? CpfCnpj { get; set; }
    public string? Endereco { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Cep { get; set; }
    public string? FotoUrl { get; set; }
    public List<ContactPhone> Telefones { get; set; } = new List<ContactPhone>();
    public string CriadoPor { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}