using API.Configuration;
using API.MiddleWare;
using Application.Interfaces;
using Infrastructure.Configuration;
using Infrastructure.Services;
using MediatR;
using Shared.Constant;
using Microsoft.AspNetCore.Mvc;
using API.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCorsPolicy();
builder.Services.AddApiVersioningConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMediatR(typeof(Application.AssemblyReference).Assembly);


builder.Services.AddAuthorization();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod());

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthentication();                         
app.UseAuthorization();


//app.UseMiddleware<SecureResponseMiddleware>();

app.MapControllers();
app.Run();
