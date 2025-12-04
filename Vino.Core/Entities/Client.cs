namespace BetterThanYou.Core.Entities;

public class Client
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
    
    public List<ContactPhone> Telefones { get; set; } = new List<ContactPhone>();
    
    public string CriadoPor { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public string? ModificadoPor { get; set; }
    public DateTime? DataModificacao { get; set; }
    public bool Ativo { get; set; } = true;
    public string? FotoUrl { get; set; }
}

public class ContactPhone
{
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
}