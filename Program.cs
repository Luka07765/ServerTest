using Microsoft.EntityFrameworkCore;
using NotesApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL
builder.Services.AddDbContext<NotesContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging() // For debugging
           .LogTo(Console.WriteLine, LogLevel.Information)); // Log to console

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.WithOrigins("http://localhost:3000") // Your frontend URL
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