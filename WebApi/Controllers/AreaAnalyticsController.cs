using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("Areas/{areaId:long}")]
public class AreaAnalyticsController : ControllerBase
{
    private readonly IAreasService _areasService;

    public AreaAnalyticsController(IAreasService areasService)
    {
        _areasService = areasService;
    }

    [HttpGet("analytics")]
    public ActionResult<AnalyticDto> GetAnalytics(
        [FromQuery(Name = "startDate")] DateTime? startDate,
        [FromQuery(Name = "endDate")] DateTime? endDate, 
        [Range(1, long.MaxValue)] long areaId)
    {
        return _areasService.GetAnalytic(areaId, startDate, endDate);
    }
}