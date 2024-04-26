using AllowCors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


string _cors = "all";



builder.Services.AddCors( options =>
    {
        ////IN CODE
        options.AddPolicy(_cors, builder =>
          builder.SetIsOriginAllowed(origin =>
          {
              var uri = new Uri(origin);
              return uri.Host == "localhost" ||
                      uri.Host == "localhost:8080";
          })
        .AllowAnyMethod()
        .AllowAnyHeader());

        //IN CODE
        options.AddPolicy(name: _cors, builder =>
          builder.WithOrigins("http://example.com",
                              "http://example.org",
                              "http://example.net")
        .AllowAnyMethod()
        .AllowAnyHeader());


        // FROM appsettings.json
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

        options.AddPolicy(name: _cors, builder =>
        {
            if (allowedOrigins != null && allowedOrigins.Length > 0)
            {
                builder.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader();
            }
        });

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(_cors);

app.UseMiddleware<IPAllowCorsMiddleware>();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
