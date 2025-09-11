using Core.ExceptionHandling;
using Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDataServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();