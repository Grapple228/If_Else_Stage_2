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
public class AnimalsController : ControllerBase
{
    private readonly IAnimalsService _animalsService;

    public AnimalsController(IAnimalsService animalsService)
    {
        _animalsService = animalsService;
    }

    [AllowAnonymous]
    [HttpGet("{animalId:int}")]
    public ActionResult<AnimalDto> GetAnimal([Range(1, int.MaxValue)]int animalId)
    {
        if (!User.IsAuthenticated()) return Unauthorized();
        return _animalsService.Get(animalId);
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public ActionResult<IEnumerable<AnimalDto>> SearchAccounts([FromQuery] AnimalSearch search)
    {
        if (!User.IsAuthenticated()) return Unauthorized();
        return Ok(_animalsService.Search(search));
    }

    [HttpPost("")]
    public ActionResult<AnimalDto> PostAnimal(AnimalCreateRequest request)
    {
        var animal = _animalsService.Create(request);
        return CreatedAtAction(nameof(GetAnimal), new { animalId = animal.Id }, animal);
    }

    [HttpPut("{animalId:long}")]
    public ActionResult<AnimalDto> PutAnimal(AnimalUpdateRequest request, [Range(1, long.MaxValue)]long animalId)
    {
        return _animalsService.Update(animalId, request);
    }

    [HttpDelete("{animalId:long}")]
    public ActionResult DeleteAnimal([Range(1, long.MaxValue)] long animalId)
    {
        _animalsService.Delete(animalId);
        return Ok();
    }

    [HttpPost("{animalId:long}/types/{typeId:long}")]
    public ActionResult<AnimalDto> AddTypeToAnimal([Range(1, long.MaxValue)] long animalId, [Range(1, long.MaxValue)] long typeId)
    {
        var animal = _animalsService.AddTypeToAnimal(animalId, typeId);
        return CreatedAtAction(nameof(GetAnimal), new { animalId = animal.Id }, animal);
    }

    [HttpPut("{animalId:long}/types")]
    public ActionResult<AnimalDto> ReplaceTypeToAnimal([Range(1, long.MaxValue)]long animalId,
        [FromBody] ATypeReplaceRequest replaceRequest)
    {
        return _animalsService.ReplaceTypeToAnimal(animalId, replaceRequest);
    }

    [HttpDelete("{animalId:long}/types/{typeId:long}")]
    public ActionResult<AnimalDto> DeleteTypeFromAnimal([Range(1, long.MaxValue)]long animalId, [Range(1, long.MaxValue)]long typeId)
    {
        return _animalsService.DeleteTypeFromAnimal(animalId, typeId);
    }
}