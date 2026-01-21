using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public class MyDbContext:DbContext
{
    public DbSet<Cake> Cakes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    
    public MyDbContext() { }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        // Needed for running migrations
        var connectionString ="Server=tcp:test-pos-db.database.windows.net,1433;Initial Catalog=pos_test_db;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(
            connectionString,
            builder => { builder.EnableRetryOnFailure(); });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Ingredient>()
            .HasOne(c => c.Cake)
            .WithMany(c => c.Ingredients);

        modelBuilder.Entity<Ingredient>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .ValueGeneratedNever();
    }
}