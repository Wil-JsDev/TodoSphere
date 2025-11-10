using TodoSphere.Auth.API.Extensions;
using TodoSphere.Auth.Application;
using TodoSphere.Auth.Infrastructure.Shared;
using TodoSphere.Auth.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructureShared(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddSwaggerExtension();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UserSwaggerExtension();

app.MapControllers();

app.Run();