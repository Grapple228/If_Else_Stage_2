using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Database;
using Database.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace WebApi.Handlers;

internal static class Extensions
{
    public static Account? CheckAuthorization(this IAccountsRepository accountsRepository,
        HttpRequest request)
    {
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            var email = credentials[0];
            var password = credentials[1];
            return accountsRepository.Authenticate(email, password);
        }
        catch
        {
            return null;
        }
    }
}

internal class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUnitOfWork _unitOfWork;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUnitOfWork unitOfWork)
        : base(options, logger, encoder, clock)
    {
        _unitOfWork = unitOfWork;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        var isAnon = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;
        var isHaveHeader = Request.Headers.ContainsKey("Authorization");

        // Если точка анонимная и в ней нет заголовка авторизации
        if (isAnon && !isHaveHeader)
            return Task.FromResult(AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(Scheme.Name)), Scheme.Name)));

        if (!isHaveHeader)
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

        var account = _unitOfWork.AccountsRepository.CheckAuthorization(Request);
        if (account == null)
            return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Role, ""+account.Role)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}