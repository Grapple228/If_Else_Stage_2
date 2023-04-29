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
[Route("Animals/{animalId:long}/locations")]
public class VisitedLocationController : ControllerBase
{
    private readonly IVisitedLocationService _visitedLocationService;

    public VisitedLocationController(IVisitedLocationService visitedLocationService)
    {
        _visitedLocationService = visitedLocationService;
    }

    [HttpGet("")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<VisitedLocationDto>> SearchVisitedPoints(
        [Range(1, long.MaxValue)] long animalId,
        [FromQuery] VisitedLocationSearch search)
    {
        if (!User.IsAuthenticated()) return Unauthorized();
        return Ok(_visitedLocationService.Search(animalId, search));
    }

    [HttpPost("{pointId:long}")]
    public ActionResult<VisitedLocationDto> AddLocationToAnimal([Range(1, long.MaxValue)] long pointId, [Range(1, long.MaxValue)]long animalId)
    {
        var point = _visitedLocationService.AddLocationToAnimal(animalId, pointId);
        return Created(Request.GetUrl(), point);
    }

    [HttpPut("")]
    public ActionResult<VisitedLocationDto> ChangeAnimalLocation([Range(1, long.MaxValue)] long animalId,
        [FromBody] VisitedLocationUpdateRequest request)
    {
        return _visitedLocationService.ChangeAnimalLocation(animalId, request);
    }

    [HttpDelete("{visitedPointId:long}")]
    public ActionResult DeleteLocationFromAnimal([Range(1, long.MaxValue)]long visitedPointId, [Range(1, long.MaxValue)]long animalId)
    {
        _visitedLocationService.DeleteLocationFromAnimal(animalId, visitedPointId);
        return Ok();
    }
}