using System.Net.Mail;
using System.Security.Claims;
using Database;
using Database.Filters;
using Database.Models;
using WebApi.Converters;
using WebApi.Dtos;
using WebApi.Exceptions;
using WebApi.Handlers;
using WebApi.Helpers;
using WebApi.Misc;
using WebApi.Models.Search;
using WebApi.Requests;

namespace WebApi.Services;

public class AccountsService : ServiceBase, IAccountsService
{
    public AccountsService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    
    public AccountDto Get(int accountId)
    {
        var account = UnitOfWork.AccountsRepository.Get(accountId);

        if (account == null)
            throw new NotFoundException(ErrorMessages.AccountNotFound);
        
        return account.AsDto();
    }

    public IEnumerable<AccountDto> Search(AccountSearch search)
    {
        var filter = new AccountsFilter
        {
            Email = search.Email,
            Firstname = search.FirstName,
            LastName = search.LastName
        };
        return UnitOfWork.AccountsRepository.GetAll(filter)
            .OrderBy(x => x.Id).Skip(search.From).Take(search.Size).AsEnumerable().AsDto();
    }

    public bool CheckAuthorization(HttpContext context)
    {
        return UnitOfWork.AccountsRepository.CheckAuthorization(context.Request) != null;
    }
    
    public AccountDto Register(AccountRegisterRequest request)
    {
        if (!MailAddress.TryCreate(request.Email, out _)) throw new BadRequestException(ErrorMessages.InvalidData);

        var normEmail = request.Email.ToLower();

        if (UnitOfWork.AccountsRepository.GetWithEmail(normEmail) != null)
            throw new ConflictException(ErrorMessages.EmailExists);

        var account = new Account
        {
            Email = normEmail,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        
        UnitOfWork.AccountsRepository.Create(account);
        UnitOfWork.Save();
        
        return account.AsDto();
    }
    
    public AccountDto Create(AccountCreateRequest request)
    {
        if (!MailAddress.TryCreate(request.Email, out _))
            throw new BadRequestException(ErrorMessages.InvalidData);

        var normEmail = request.Email.ToLower();

        if (UnitOfWork.AccountsRepository.GetWithEmail(normEmail) != null)
            throw new ConflictException(ErrorMessages.EmailExists);

        var created = new Account
        {
            Email = normEmail,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role
        };
        UnitOfWork.AccountsRepository.Create(created);
        UnitOfWork.Save();
        
        return created.AsDto();
    }

    private static Exception ProcessNullAccount(bool isAdmin)
    {
        if (isAdmin)
            return new NotFoundException(ErrorMessages.AccountNotFound);
        return new ForbiddenException(ErrorMessages.Forbidden);
    }

    private static bool IsAccountOwner(HttpContext httpContext, int accountId)
    {
        var id = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(id, out var idLong) && idLong == accountId;
    }
    
    public AccountDto Update(int accountId, AccountCreateRequest request, HttpContext httpContext)
    {
        if (!MailAddress.TryCreate(request.Email, out _))
            throw new BadRequestException(ErrorMessages.InvalidData);
        
        var isAdmin = httpContext.User.GetRole().IsAdmin();

        var current = UnitOfWork.AccountsRepository.Get(accountId);
        if (current == null)
            throw ProcessNullAccount(isAdmin);

        if (!isAdmin)
        {
            if (request.Role != current.Role)
                throw new BadRequestException(ErrorMessages.TryingToChangeRole);
            
            if(!IsAccountOwner(httpContext, accountId))
                throw new ForbiddenException(ErrorMessages.Forbidden);
        }

        var normEmail = request.Email.ToLower();
        var account = UnitOfWork.AccountsRepository.GetWithEmail(normEmail);
        if (account != null && account.Id != accountId)
            throw new ConflictException(ErrorMessages.EmailExists);

        current.Email = normEmail;
        current.Password = request.Password;
        current.FirstName = request.FirstName;
        current.LastName = request.LastName;
        current.Role = request.Role;

        UnitOfWork.AccountsRepository.Update(current);
        
        UnitOfWork.Save();

        return current.AsDto();
    }

    public void Delete(int accountId, HttpContext httpContext)
    {
        var isAdmin = httpContext.User.GetRole().IsAdmin();
        
        var account = UnitOfWork.AccountsRepository.Get(accountId);
        if (account == null)
            throw ProcessNullAccount(isAdmin);

        if (UnitOfWork.AnimalsRepository.GetFirstWithChipper(accountId) != null)
            throw new BadRequestException(ErrorMessages.AccountLinkedToAnimal);

        if (!isAdmin && !IsAccountOwner(httpContext, accountId))
            throw new ForbiddenException(ErrorMessages.Forbidden);
        
        UnitOfWork.AccountsRepository.Delete(account);
        UnitOfWork.Save();
    }
}