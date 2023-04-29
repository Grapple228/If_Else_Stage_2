using System.ComponentModel.DataAnnotations;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Helpers;
using WebApi.Models.Search;
using WebApi.Requests;
using WebApi.Services;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService _accountsService;

    public AccountsController(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    [AllowAnonymous]
    [HttpGet("{accountId:int}")]
    public ActionResult<AccountDto> GetAccount([Range(1, int.MaxValue)]int accountId)
    {
        if (!User.IsAuthenticated()) return Unauthorized();
        return _accountsService.Get(accountId);
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public ActionResult<IEnumerable<AccountDto>> SearchAccounts([FromQuery] AccountSearch accountSearch)
    {
        if (!User.IsAuthenticated()) return Unauthorized();
        return Ok(_accountsService.Search(accountSearch));
    }

    [HttpPost("")]
    [Authorize(Roles = "Admin")]
    public ActionResult<AccountDto> PostAccount(AccountCreateRequest request)
    {
        var account = _accountsService.Create(request);
        return CreatedAtAction(nameof(GetAccount), new { accountId = account.Id }, account);
    }

    [HttpPut("{accountId:int}")]
    public ActionResult<AccountDto> PutAccount(AccountCreateRequest request, [Range(1, int.MaxValue)]int accountId)
    {
        return Ok(_accountsService.Update(accountId, request, HttpContext));
    }

    [HttpDelete("{accountId:int}")]
    public ActionResult DeleteAccount([Range(1, int.MaxValue)]int accountId)
    {
        _accountsService.Delete(accountId, HttpContext);
        return Ok();
    }
}