using System.ComponentModel.DataAnnotations;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Requests;
using WebApi.Services;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationsService _locationsService;

    public LocationsController(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    [AllowAnonymous]
    [HttpGet("{pointId:int}")]
    public ActionResult<LocationDto> GetLocation([Range(1, long.MaxValue)]long pointId)
    {
        if (!User.IsAuthenticated()) return Unauthorized();
        return _locationsService.Get(pointId);
    }
    
    [HttpPost("")]
    public ActionResult<LocationDto> PostLocation([FromBody] LocationCreateRequest request)
    {
        var location = _locationsService.CreateLocation(request);
        return CreatedAtAction(nameof(GetLocation), new { pointId = location.Id }, location);
    }

    [HttpPut("{pointId:long}")]
    public ActionResult<LocationDto> PutLocation([FromBody] LocationCreateRequest request, [Range(1, long.MaxValue)]long pointId)
    {
        return _locationsService.UpdateLocation(pointId, request);
    }

    [HttpDelete("{pointId:long}")]
    public ActionResult DeleteLocation([Range(1, long.MaxValue)]long pointId)
    {
        _locationsService.DeleteLocation(pointId);
        return Ok();
    }
    
    [HttpGet("")]
    public long SecretEndpoint([FromQuery] LocationCords cords)
    {
        return _locationsService.Get(cords.Latitude, cords.Longitude).Id;
    }
    
    [HttpGet("geohash")]
    public string GeoHash([FromQuery] LocationCords cords)
    {
        return _locationsService.GetHashV1(cords);
    }
    
    [HttpGet("geohashv2")]
    public string GeoHashV2([FromQuery] LocationCords cords)
    {
        return _locationsService.GetHashV2(cords);
    }
    
    [HttpGet("geohashv3")]
    public string GeoHashV3([FromQuery] LocationCords cords)
    {
        return _locationsService.GetHashV3(cords);
    }
}