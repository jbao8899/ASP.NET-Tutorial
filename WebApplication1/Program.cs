using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options => 
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ShirtStoreManagement"));
            });
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Add the controllers I made in the Controllers folder
            builder.Services.AddControllers(); 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();

            //var summaries = new[]
            //{
            //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            //};

            //app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            //{
            //    var forecast = Enumerable.Range(1, 5).Select(index =>
            //        new WeatherForecast
            //        {
            //            Date = DateTime.Now.AddDays(index),
            //            TemperatureC = Random.Shared.Next(-20, 55),
            //            Summary = summaries[Random.Shared.Next(summaries.Length)]
            //        })
            //        .ToArray();
            //    return forecast;
            //});

            //// Routing
            //app.MapGet("/shirts", () =>
            //{
            //    return "Reading all the shirts.";
            //});

            //app.MapGet("/shirts/{id}", (int id) =>
            //{
            //    return $"Reading shirt with id = {id}.";
            //});

            //app.MapPost("/shirts", () =>
            //{
            //    return "Creating a shirt";
            //});

            //app.MapPut("/shirts/{id}", (int id) =>
            //{
            //    return $"Updating shirt with id = {id}";
            //});

            //app.MapDelete("/shirts/{id}", (int id) =>
            //{
            //    return $"Deleting shirt with id = {id}";
            //});

            app.MapControllers();

            app.Run();
        }
    }
}