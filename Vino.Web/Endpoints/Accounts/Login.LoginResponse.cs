namespace BetterThanYou.Web.Endpoints.Accounts;

public class AccountLoginResponse
{
    public Guid Id { get; set; } 
    public string Email { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
}