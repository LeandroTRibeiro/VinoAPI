namespace BetterThanYou.Web.Endpoints.Accounts;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}