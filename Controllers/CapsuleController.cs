using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Extensions;
using Project.Models;
using Project.Models.Entities;

namespace Project.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CapsuleController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public CapsuleController(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult GetAllCapsules()
    {
        var capsules = _dbContext.Capsules.ToList();

        return Ok(capsules);
    }

    [HttpPost]
    public IActionResult AddCapsule(AddCapsuleDto addCapsuleDto)
    {
        var loggedUserId = User.GetUserId();

        var capsuleEntity = new Capsule() {
            Id = Guid.NewGuid(),
            CreaterId = loggedUserId,
            CapsuleName = addCapsuleDto.CapsuleName,
            ExpireDate = addCapsuleDto.ExpireDate,
            CrationDate = DateTime.UtcNow,
            CapsuleData = addCapsuleDto.CapsuleData,
        };

        _dbContext.Capsules.Add(capsuleEntity);
        _dbContext.SaveChanges();

        return Ok(capsuleEntity);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetCapsuleById(Guid id)
    {
        var capsule = _dbContext.Capsules.FirstOrDefault(x => x.Id == id);
        if (capsule == null)
        {
            return NotFound();
        }
        return Ok(capsule);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public IActionResult UpdateCapsuleById(Guid id, UpdateCapsuleDto updateCapsuleDto)
    {
        var capsule = _dbContext.Capsules.FirstOrDefault(x => x.Id == id);
        if (capsule == null)
        {
            return NotFound();
        }

        capsule.ExpireDate = updateCapsuleDto.ExpireDate;
        capsule.CapsuleName = updateCapsuleDto.CapsuleName;
        capsule.CapsuleData = updateCapsuleDto.CapsuleData;
        capsule.Image = updateCapsuleDto.Image;

        _dbContext.SaveChanges();

        return Ok(capsule);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult DeleteCapsuleById(Guid id)
    {
        var capsule = _dbContext.Capsules.FirstOrDefault(x => x.Id == id);
        if (capsule == null)
        {
            return NotFound();
        }

        _dbContext.Capsules.Remove(capsule);
        _dbContext.SaveChanges();

        return Ok();
    }
}
