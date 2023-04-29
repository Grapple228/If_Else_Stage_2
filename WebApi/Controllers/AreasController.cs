using System.ComponentModel.DataAnnotations;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Requests;
using WebApi.Services;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]")]
public class AreasController : ControllerBase
{
    private readonly IAreasService _areasService;

    public AreasController(IAreasService areasService)
    {
        _areasService = areasService;
    }

    [HttpGet("{areaId:long}")]
    public ActionResult<AreaDto> GetArea([Range(1, long.MaxValue)] long areaId)
    {
        return _areasService.Get(areaId);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("")]
    public ActionResult<AreaDto> PostArea(AreaCreateRequest request)
    {
        var area = _areasService.Create(request);
        return CreatedAtAction(nameof(GetArea), new { areaId = area.Id }, area);
    }

    [HttpPut("{areaId:long}")]
    [Authorize(Roles = "Admin")]
    public ActionResult<AreaDto> PutArea(AreaCreateRequest request, [Range(1, long.MaxValue)] long areaId)
    {
        return _areasService.Update(areaId, request);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{areaId:long}")]
    public ActionResult<AreaDto> PostArea([Range(1, long.MaxValue)] long areaId)
    {
        _areasService.Delete(areaId);
        return Ok();
    }
    
    
}