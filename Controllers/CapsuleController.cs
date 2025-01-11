using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Extensions;
using Project.Models;
using Project.Models.Entities;
using System.Linq;

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
    [Route("GetAll/")]
    public IActionResult GetAll()
    {
        var capsules = _dbContext.Capsules.ToList();

        return Ok(capsules);
    }

    [HttpPost]
    [Route("Create/")]
    public IActionResult AddCapsule(AddCapsuleDto addCapsuleDto)
    {
        var loggedUserId = User.GetUserId();

        var capsuleData = new CapsuleData()
        {
            Id = Guid.NewGuid(),
            CapsuleText = addCapsuleDto.CapsuleText,
            Image = addCapsuleDto.Image
        };

        var capsuleEntity = new Capsule()
        {
            Id = Guid.NewGuid(),
            CreaterId = loggedUserId,
            OwnerId = loggedUserId,
            CrationDate = DateTime.UtcNow,
            LockDate = addCapsuleDto.LockDate,
            ExpireDate = addCapsuleDto.LockDate.AddDays(10),
            CapsuleName = addCapsuleDto.CapsuleName,
            CapsuleData = capsuleData
        };

        _dbContext.Capsules.Add(capsuleEntity);
        _dbContext.SaveChanges();

        return Ok(capsuleEntity);
    }

    [HttpGet]
    [Route("GetCapsuleById/{id:guid}")]
    public IActionResult GetCapsuleById(Guid id)
    {
        var loggedUserId = User.GetUserId();

        var capsule = _dbContext.Capsules.Where(x => x.Id == id && x.OwnerId == loggedUserId).FirstOrDefault();
        if (capsule == null)
        {
            return NotFound();
        }
        if (capsule.ExpireDate < DateTime.Now)
        {
            capsule = _dbContext.Capsules.Where(x => x.Id == id && x.OwnerId == loggedUserId).Include(x => x.CapsuleData).First();
        }

        return Ok(capsule);
    }

    [HttpGet]
    [Route("GetAllCapsulesByLoggedUserId/")]
    public IActionResult GetAllCapsulesByLoggedUserId()
    {
        var loggedUserId = User.GetUserId();

        var capsules = _dbContext.Capsules.Where(x => x.OwnerId == loggedUserId)?.Select(x => new { x.Id, x.CapsuleName }).ToList();
        if (capsules == null)
        {
            return NotFound();
        }
        return Ok(capsules);
    }

    [HttpGet]
    [Route("GetCapsulCouteByLoggedUserId/")]
    public IActionResult GetAllCapsulCouteByLoggedUserId()
    {
        var loggedUserId = User.GetUserId();

        var capsules = _dbContext.Capsules.Where(x => x.OwnerId == loggedUserId)?.Select(x => new { x.Id, x.CapsuleName }).ToList();
        if (capsules == null)
        {
            return NotFound();
        }
        return Ok(capsules);
    }

    [HttpGet]
    [Route("GetAllCapsulesByUserId/{userId:guid}")]
    public IActionResult GetAllCapsulesByUserId(Guid userId)
    {
        var capsules = _dbContext.Capsules.Where(x => x.OwnerId == userId)?.Select(x => new { x.Id, x.CapsuleName }).ToList();
        if (capsules == null)
        {
            return NotFound();
        }
        return Ok(capsules);
    }

    [HttpGet]
    [Route("GetCapsuleCountByUserId/{userId:guid}")]
    public IActionResult GetCapsuleCountByUserId(Guid userId)
    {
        var capsulesCount = _dbContext.Capsules.Where(x => x.OwnerId == userId)?.Select(x => new { x.Id, x.CapsuleName }).ToList().Count();
        return Ok(capsulesCount);
    }

    [HttpPut]
    [Route("UpdateCapsuleById/{id:guid}")]
    public IActionResult UpdateCapsuleById(Guid id, UpdateCapsuleDto updateCapsuleDto)
    {
        var loggedUserId = User.GetUserId();
        var capsule = _dbContext.Capsules.Where(x => x.Id == id && x.OwnerId == loggedUserId)?.Include(x => x.CapsuleData).FirstOrDefault();
        if (capsule == null)
        {
            return NotFound();
        }
        if (capsule.LockDate > DateTime.Now)
        {
            return Ok("Kapsül kilitli");
        }

        capsule.CapsuleData.CapsuleText = updateCapsuleDto.CapsuleText;
        capsule.CapsuleData.Image = updateCapsuleDto.Image;

        _dbContext.SaveChanges();

        return Ok(capsule);
    }

    [HttpPut]
    [Route("TransferOwner/{id:guid}")]
    public IActionResult TransferCapsuleToNewOwner(Guid id, TransferCapsuleDto transferCapsuleDto)
    {
        var loggedUserId = User.GetUserId();
        var capsule = _dbContext.Capsules.Where(x => x.Id == id && x.OwnerId == loggedUserId).FirstOrDefault();
        if (capsule == null)
        {
            return NotFound();
        }
        if (capsule.ExpireDate > DateTime.Now)
        {
            return Ok("Kapsül kilitli");
        }

        capsule.OwnerId = new Guid(transferCapsuleDto.NewOwner.Id);

        _dbContext.SaveChanges();

        return Ok(capsule);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult DeleteCapsuleById(Guid id)
    {
        var loggedUserId = User.GetUserId();
        var capsule = _dbContext.Capsules.FirstOrDefault(x => x.Id == id && x.OwnerId == loggedUserId);
        if (capsule == null)
        {
            return NotFound();
        }

        _dbContext.Capsules.Remove(capsule);
        _dbContext.SaveChanges();

        return Ok();
    }

    [HttpGet]
    [Route("Cleanup/")]
    public IActionResult CleanOverExpireds()
    {
        var currentDate = DateTime.Now;
        currentDate.AddDays(-10);
        var capsules = _dbContext.Capsules.Where(x => x.ExpireDate > currentDate).ToList();

        _dbContext.Capsules.RemoveRange(capsules);
        _dbContext.SaveChanges();

        return Ok();
    }

    [HttpGet]
    [Route("{id:guid}/Status")]
    public IActionResult GetCapsuleStatus(Guid id)
    {
        var loggedUserId = User.GetUserId();
        var capsule = _dbContext.Capsules.Where(x => x.Id == id && x.OwnerId == loggedUserId)?.Include(x => x.CapsuleData).FirstOrDefault();
        if (capsule == null)
        {
            return NotFound();
        }
        if (capsule.LockDate > DateTime.Now)
        {
            return Ok("Kapsül kilitli");
        }

        return Ok("Kapsül açık");
    }

    [HttpPut]
    [Route("Search/{key:string}")]
    public IActionResult UpdateCapsuleById(string key)
    {
        var loggedUserId = User.GetUserId();
        var capsule = _dbContext.Capsules
            .Where(x => 
            (x.CapsuleData.CapsuleText.Contains(key) || x.CapsuleName.Contains(key))
            && x.OwnerId == loggedUserId)?.Select(x => new { x.Id, x.CapsuleName }).ToList();
        if (capsule == null)
        {
            return NotFound();
        }

        _dbContext.SaveChanges();

        return Ok(capsule);
    }
}
