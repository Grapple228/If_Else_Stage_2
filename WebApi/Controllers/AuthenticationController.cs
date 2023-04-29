using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Requests;
using WebApi.Services;

namespace WebApi.Controllers;

[Authorize]
[Route("")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAccountsService _accountsService;

    public AuthenticationController(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    [AllowAnonymous]
    [HttpPost("Registration")]
    public ActionResult<AccountDto> Registration([FromHeader] string? authorization,
        AccountRegisterRequest request)
    {
        if(authorization != null && !_accountsService.CheckAuthorization(HttpContext))
            return Forbid();

        var account = _accountsService.Register(request);
        
        var routeValues = new
        {
            action = nameof(AccountsController.GetAccount),
            controller = "Accounts",
            accountId = account.Id
        };

        return CreatedAtRoute(routeValues, account);
    }
}