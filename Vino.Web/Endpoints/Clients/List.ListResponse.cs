using BetterThanYou.Core.DTOs.Account;

namespace BetterThanYou.Web.Endpoints.Clients;

public class ClientListResponse
{
    public List<ClientDto> Clients { get; set; } = new List<ClientDto>();
}