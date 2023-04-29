using WebApi.Dtos;
using WebApi.Requests;

namespace WebApi.Services;

public interface IAreasService
{
    AreaDto Get(long areaId);
    AreaDto Create(AreaCreateRequest request);
    AreaDto Update(long areaId, AreaCreateRequest request);
    void Delete(long areaId);
    AnalyticDto GetAnalytic(long areaId, DateTime? startDate, DateTime? endDate);
}