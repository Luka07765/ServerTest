using Microsoft.EntityFrameworkCore;
using NotesApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("DATABASE_URL environment variable is not set.");
}
Console.WriteLine($"Database Connection String: {connectionString}");
foreach (var item in Environment.GetEnvironmentVariables().Keys)
{
    Console.WriteLine($"{item}: {Environment.GetEnvironmentVariable(item.ToString())}");
}


// Configure PostgreSQL
builder.Services.AddDbContext<NotesContext>(options =>
    options.UseNpgsql(connectionString)
           .EnableSensitiveDataLogging() // For debugging
           .LogTo(Console.WriteLine, LogLevel.Information)); // Log to console

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.WithOrigins("https://server-front-wheat.vercel.app", "http://localhost:3000") // Your frontend URL
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();


// Error handling
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}

// Root endpoint
app.MapGet("/", () => "Notes API is running!");

app.UseHttpsRedirection(); app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();