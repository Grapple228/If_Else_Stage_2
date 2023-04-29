using Database;
using Database.Models;
using WebApi.Converters;
using WebApi.Dtos;
using WebApi.Exceptions;
using WebApi.Misc;
using WebApi.Requests;

namespace WebApi.Services;

public class TypesService : ServiceBase, ITypesService
{

    public TypesService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public ATypeDto Get(long typeId)
    {
        var type = UnitOfWork.TypesRepository.Get(typeId);
        
        if (type == null) throw new NotFoundException(ErrorMessages.TypeNotFound);
        
        return type.AsDto();
    }

    public ATypeDto Create(ATypeCreateRequest request)
    {
        var normType = request.Type.ToLower();

        if (UnitOfWork.TypesRepository.GetWithValue(normType) != null)
            throw new ConflictException(ErrorMessages.TypeAlreadyExists);
        
        var type = new AType
        {
            Type = normType
        };
        
        UnitOfWork.TypesRepository.Create(type);
        UnitOfWork.Save();
        return type.AsDto();
    }

    public ATypeDto Update(long typeId, ATypeCreateRequest request)
    {
        var current = UnitOfWork.TypesRepository.Get(typeId);
        if (current == null) throw new NotFoundException(ErrorMessages.TypeNotFound);

        var normType = request.Type.ToLower();

        if (UnitOfWork.TypesRepository.GetWithValue(normType) != null)
            throw new ConflictException(ErrorMessages.TypeAlreadyExists);

        current.Type = normType;
        
        UnitOfWork.TypesRepository.Update(current);
        UnitOfWork.Save();

        return current.AsDto();
    }

    public void Delete(long typeId)
    {
        var type = UnitOfWork.TypesRepository.Get(typeId);
        if (type == null) throw new NotFoundException(ErrorMessages.TypeNotFound);

        if (UnitOfWork.AnimalsRepository.GetFirstWithType(typeId) != null)
            throw new BadRequestException(ErrorMessages.ExistAnimalWithType);

        UnitOfWork.TypesRepository.Delete(type);
        UnitOfWork.Save();
    }
}