using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

[Route("/api/[controller]")]
[ApiController]
public class CakeController(MyDbContext dbContext) : ControllerBase
{
    // GET: api/<My>
    [HttpGet]
    public IEnumerable<Cake> Get()
    {
        var cakes = dbContext.Cakes
            .Include(c => c.Ingredients)
            .ToList();
        
        return  cakes;
    }

    // GET api/<MyController>/5
    [HttpGet("{id}")]
    public Cake? Get(Guid id)
    {
        return dbContext.Cakes.FirstOrDefault(c => c.Id == id);
    }

    // POST api/<MyController>
    [HttpPost]
    public void Post([FromBody] Cake  cake)
    {
        dbContext.Cakes.Add(cake);
        dbContext.SaveChanges();    
    }
    
    // POST api/<MyController>
    [HttpPost("{id}/ingredients")]
    public async Task<Cake> AddIngredient(Guid id, [FromBody] IngredientDto  ingredient)
    {
        var cake = await dbContext.Cakes
            .Include(c=>c.Ingredients)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (cake == null)
            throw new NotFoundException("Cake not found");
    
        cake.Ingredients.Add(new Ingredient{Name =  ingredient.Name});

        var entities = dbContext.ChangeTracker.Entries();
        
        await dbContext.SaveChangesAsync();

        return cake;
    }

    // PUT api/<MyController>/5
    [HttpPut("{id}")]
    public void Put(Guid id, [FromBody] CakeDto cake)
    {
        var existing = dbContext.Cakes.FirstOrDefault(c => c.Id == id);
        if (existing == null)
            throw new NotFoundException("Cake not found");
        existing.Name = cake.Name;
        existing.Description = cake.Description;
        dbContext.SaveChanges();
        
    }

    // DELETE api/<MyController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

public class NotFoundException(string cakeNotFound) : Exception(cakeNotFound);

public record IngredientDto
{
    public string Name { get; set; }
}