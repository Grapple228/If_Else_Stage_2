using System.ComponentModel.DataAnnotations;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Helpers;
using WebApi.Requests;
using WebApi.Services;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("Animals/[Controller]")]
public class TypesController : ControllerBase
{
    private readonly ITypesService _typesService;

    public TypesController(ITypesService typesService)
    {
        _typesService = typesService;
    }

    [AllowAnonymous]
    [HttpGet("{typeId:long}")]
    public ActionResult<ATypeDto> GetType([Range(1, long.MaxValue)] long typeId)
    {
        if (!User.IsAuthenticated()) return Unauthorized();

        return _typesService.Get(typeId);
    }

    [HttpPost("")]
    public ActionResult<ATypeDto> PostAnimalType([FromBody] ATypeCreateRequest request)
    {
        var type = _typesService.Create(request);
        return CreatedAtAction(nameof(GetType), new { typeId = type.Id }, type);
    }

    [HttpPut("{typeId:long}")]
    public ActionResult<ATypeDto> PutAnimalType([FromBody] ATypeCreateRequest request, [Range(1, long.MaxValue)] long typeId)
    {
        return _typesService.Update(typeId, request);
    }

    [HttpDelete("{typeId:long}")]
    public ActionResult DeleteType([Range(1, long.MaxValue)] long typeId)
    {
        _typesService.Delete(typeId);
        return Ok();
    }
}