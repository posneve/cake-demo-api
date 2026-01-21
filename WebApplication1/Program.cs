using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openap
builder.Services.AddOpenApi();
// builder.Services.AddSingleton<MyClass>();
// builder.Services.AddScoped<MyClass2>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<MyDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyDb");
    options.UseSqlServer(connectionString, b => { b.EnableRetryOnFailure(); });
});

var app = builder.Build();

app.MapOpenApi();

app.UseMiddleware<AuthML>();
app.UseMiddleware<ExceptionMiddleware>();


app.MapControllers();

app.Run();


public interface IContainsName
{
    string Name { get; set; }
}

class MyClass : IContainsName
{
    public string Name { get; set; } = "No name";

    public void WriteMyName()
    {
        Console.WriteLine($"Hello, {Name}");
    }
}

class MyClass2(MyClass myClass)
{
    public void SetName(string name)
    {
        myClass.Name = name;
    }

    public void WriteMyName()
    {
        Console.WriteLine($"Hello, {myClass.Name}");
    }
}