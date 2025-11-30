namespace BetterThanYou.Web.Endpoints.Accounts;

public class AccountLoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}