using BetterThanYou.Core.DTOs.Account;
using BetterThanYou.Core.Entities;
using BetterThanYou.Core.Interfaces.Client;
using BetterThanYou.SharedKernel.Validation;

namespace BetterThanYou.Core.Services.Client;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientDto> CreateAsync(string nomeFantasia, string? razaoSocial, string? email,
        string? cpfCnpj, string? endereco, string? cidade, string? estado, string? cep,
        List<ContactPhone> telefones, string? fotoUrl, string criadoPor)
    {
        if (string.IsNullOrWhiteSpace(nomeFantasia))
            throw new ArgumentException("Nome Fantasia é obrigatório", nameof(nomeFantasia));

        cpfCnpj = CpfCnpjValidator.RemoverCaracteresEspeciais(cpfCnpj);

        if (!CpfCnpjValidator.ValidarCpfCnpj(cpfCnpj))
            throw new ArgumentException("CPF/CNPJ inválido", nameof(cpfCnpj));

        if (await _clientRepository.ExistsByNomeFantasiaAsync(nomeFantasia))
            throw new InvalidOperationException("Já existe um cliente com este Nome Fantasia");

        if (!string.IsNullOrWhiteSpace(cpfCnpj))
        {
            if (await _clientRepository.ExistsByCpfCnpjAsync(cpfCnpj))
                throw new InvalidOperationException("Já existe um cliente com este CPF/CNPJ");
        }

        var client = new Entities.Client
        {
            Id = Guid.NewGuid(),
            NomeFantasia = nomeFantasia,
            RazaoSocial = razaoSocial,
            Email = email,
            CpfCnpj = cpfCnpj,
            Endereco = endereco,
            Cidade = cidade,
            Estado = estado,
            Cep = cep,
            FotoUrl = fotoUrl,
            Telefones = telefones ?? new List<ContactPhone>(),
            CriadoPor = criadoPor,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        var created = await _clientRepository.CreateAsync(client);

        return MapToDto(created);
    }

    public async Task<List<ClientDto>> GetAllAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        return clients.Select(MapToDto).ToList();
    }

    public async Task<ClientDto> GetByIdAsync(Guid id)
    {
        var client = await _clientRepository.GetByIdAsync(id);
        
        if (client == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        return MapToDto(client);
    }

    public async Task<ClientDto> UpdateAsync(Guid id, string nomeFantasia, string? razaoSocial, string? email,
        string? cpfCnpj, string? endereco, string? cidade, string? estado, string? cep,
        List<ContactPhone> telefones, string? fotoUrl, string modificadoPor)
    {
        if (string.IsNullOrWhiteSpace(nomeFantasia))
            throw new ArgumentException("Nome Fantasia é obrigatório", nameof(nomeFantasia));

        // Limpar CPF/CNPJ (remover caracteres especiais)
        cpfCnpj = CpfCnpjValidator.RemoverCaracteresEspeciais(cpfCnpj);

        // Validar CPF/CNPJ
        if (!CpfCnpjValidator.ValidarCpfCnpj(cpfCnpj))
            throw new ArgumentException("CPF/CNPJ inválido", nameof(cpfCnpj));

        var client = await _clientRepository.GetByIdAsync(id);
    
        if (client == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        // Validar se já existe outro cliente com o mesmo nome fantasia
        if (await _clientRepository.ExistsByNomeFantasiaAsync(nomeFantasia, id))
            throw new InvalidOperationException("Já existe outro cliente com este Nome Fantasia");

        // Validar se já existe outro cliente com o mesmo CPF/CNPJ
        if (!string.IsNullOrWhiteSpace(cpfCnpj))
        {
            if (await _clientRepository.ExistsByCpfCnpjAsync(cpfCnpj, id))
                throw new InvalidOperationException("Já existe outro cliente com este CPF/CNPJ");
        }

        client.NomeFantasia = nomeFantasia;
        client.RazaoSocial = razaoSocial;
        client.Email = email;
        client.CpfCnpj = cpfCnpj;
        client.Endereco = endereco;
        client.Cidade = cidade;
        client.Estado = estado;
        client.Cep = cep;
    
        if (!string.IsNullOrEmpty(fotoUrl))
            client.FotoUrl = fotoUrl;
    
        client.Telefones = telefones ?? new List<ContactPhone>();
        client.ModificadoPor = modificadoPor;
        client.DataModificacao = DateTime.UtcNow;

        var updated = await _clientRepository.UpdateAsync(client);

        return MapToDto(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var client = await _clientRepository.GetByIdAsync(id);
        
        if (client == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        await _clientRepository.DeleteAsync(client);
    }

    private ClientDto MapToDto(Entities.Client client)
    {
        return new ClientDto
        {
            Id = client.Id,
            NomeFantasia = client.NomeFantasia,
            RazaoSocial = client.RazaoSocial,
            Email = client.Email,
            CpfCnpj = client.CpfCnpj,
            Endereco = client.Endereco,
            Cidade = client.Cidade,
            Estado = client.Estado,
            Cep = client.Cep,
            Telefones = client.Telefones,
            CriadoPor = client.CriadoPor,
            DataCriacao = client.DataCriacao,
            ModificadoPor = client.ModificadoPor,
            DataModificacao = client.DataModificacao,
            Ativo = client.Ativo,
            FotoUrl = client.FotoUrl
        };
    }
}