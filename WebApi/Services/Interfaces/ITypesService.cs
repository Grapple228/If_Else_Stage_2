using WebApi.Dtos;
using WebApi.Requests;

namespace WebApi.Services;

public interface ITypesService
{
    ATypeDto Get(long typeId);
    ATypeDto Create(ATypeCreateRequest request);
    ATypeDto Update(long typeId, ATypeCreateRequest request);
    void Delete(long typeId);
}