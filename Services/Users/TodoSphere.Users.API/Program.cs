using Microsoft.OpenApi.Models;
using TodoSphere.Users.API.Extensions;
using TodoSphere.Users.Application;
using TodoSphere.Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//DI Layer
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

//Extensions
builder.Services.AddSwaggerExtension();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    app.UserSwaggerExtension();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();