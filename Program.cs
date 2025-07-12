using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("connection string not found");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.UseStaticFiles(); //Enables wwwroot

app.UseDirectoryBrowser(); //allows listing contents

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "products");
    if (!Directory.Exists(basePath))
        return;

    foreach (var productDir in Directory.GetDirectories(basePath))
    {
        string productName = Path.GetFileName(productDir);

        if (db.Products.Any(p => p.Name == productName)) continue;

        var product = new Product { Name = productName };

        string slidesPath = Path.Combine(productDir, "slides");
        if (!Directory.Exists(slidesPath)) continue;

        // Scan images and videos folders
        var allSlides = new List<string>();
        var imageDir = Path.Combine(slidesPath, "images");
        var videoDir = Path.Combine(slidesPath, "videos");

        if (Directory.Exists(imageDir))
            allSlides.AddRange(Directory.GetFiles(imageDir));

        if (Directory.Exists(videoDir))
            allSlides.AddRange(Directory.GetFiles(videoDir));

        // Sort numerically by filename (assuming "1.jpg", "2.mp4", etc.)
        var orderedSlides = allSlides
            .Where(f => int.TryParse(Path.GetFileNameWithoutExtension(f), out _))
            .OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f)))
            .ToList();

        int order = 1;
        foreach (var file in orderedSlides)
        {
            string relativePath = file.Replace(Directory.GetCurrentDirectory(), "")
                                      .Replace("\\", "/");

            if (relativePath.Contains("/wwwroot"))
                relativePath = relativePath.Substring(relativePath.IndexOf("/wwwroot") + "/wwwroot".Length);

            product.Slides.Add(new Slide
            {
                Url = relativePath,
                OrderNumber = order++
            });
        }

        db.Products.Add(product);
        db.SaveChanges();
    }
}



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
