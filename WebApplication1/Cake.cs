using System.ComponentModel.DataAnnotations;

namespace WebApplication1;

public class Cake
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(500)] public string Name { get; set; } = null!;
    [MaxLength(500)]
    public string? Description { get; set; }

    public List<Ingredient> Ingredients { get; set; } = new();
}

public class Ingredient
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    //public Guid CakeId { get; set; }
    public Cake Cake { get; set; } = null!;
    [MaxLength(500)]
    public string Name { get; set; } = null!;
}

public class CakeDto
{
    [MaxLength(500)]
    public string Name { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
}