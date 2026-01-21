// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<MyClass>();
serviceCollection.AddTransient<MyClass2>();
var serviceProvider = serviceCollection.BuildServiceProvider();


//var service = serviceProvider.GetRequiredService<MyClass>();
var service2 = serviceProvider.GetRequiredService<MyClass2>();

service2.SetName("Paul Anton");

var service = serviceProvider.GetRequiredService<MyClass>();
service.WriteMyName();

if (service is IContainsName containsName)
{
    Console.WriteLine($"Hello, {containsName.Name}");
}


public interface  IContainsName{
    string Name { get; set; }
    }

class MyClass: IContainsName
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