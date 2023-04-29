using WebApi.Dtos;
using WebApi.Models.Search;
using WebApi.Requests;

namespace WebApi.Services;

public interface IAccountsService
{
    AccountDto Get(int accountId);
    IEnumerable<AccountDto> Search(AccountSearch search);
    bool CheckAuthorization(HttpContext context);
    AccountDto Register(AccountRegisterRequest request);
    AccountDto Create(AccountCreateRequest request);
    AccountDto Update(int accountId, AccountCreateRequest request, HttpContext httpContext);
    void Delete(int accountId, HttpContext httpContext);
}