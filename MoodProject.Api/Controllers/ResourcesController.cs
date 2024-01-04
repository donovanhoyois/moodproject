using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodProject.Core.Models;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class ResourcesController
{
    private readonly MoodProjectContext DbContext;
    
    public ResourcesController(MoodProjectContext dbContext)
    {
        DbContext = dbContext;
    }

    [HttpGet, ActionName("GetAll")]
    public List<Resource> GetAll()
    {
        return DbContext.Ressources.ToList();
    }

    [HttpGet, ActionName("GetById")]
    public Resource? GetById(int id)
    {
        return DbContext.Ressources
            .Include(r => r.Files)
            .FirstOrDefault(r => r.Id.Equals(id));
    }

    [HttpPut, ActionName("Create")]
    public Resource Create(Resource resource)
    {
        DbContext.Add(resource);
        DbContext.SaveChanges();
        return DbContext.Ressources.First(r => r.Id.Equals(resource.Id));
    }
    
}