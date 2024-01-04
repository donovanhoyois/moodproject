using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodProject.Core.Models;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class RessourcesController
{
    private readonly MoodProjectContext DbContext;
    
    public RessourcesController(MoodProjectContext dbContext)
    {
        DbContext = dbContext;
    }

    [HttpGet, ActionName("GetAll")]
    public List<Ressource> GetAll()
    {
        return DbContext.Ressources.ToList();
    }

    [HttpGet, ActionName("GetById")]
    public Ressource? GetById(int id)
    {
        return DbContext.Ressources
            .Include(r => r.Files)
            .FirstOrDefault(r => r.Id.Equals(id));
    }

    [HttpPut, ActionName("Create")]
    public Ressource Create(Ressource ressource)
    {
        DbContext.Add(ressource);
        DbContext.SaveChanges();
        return DbContext.Ressources.First(r => r.Id.Equals(ressource.Id));
    }
    
}