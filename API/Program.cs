using API.Configuration;
using API.MiddleWare;
using Application.Interfaces;
using Infrastructure.Configuration;
using Infrastructure.Services;
using MediatR;
using Shared.Constant;
using Microsoft.AspNetCore.Mvc;
using API.Middleware;
using Microsoft.AspNetCore.Authentication;
using API.Authentication;
using CorrelationId.DependencyInjection;
using CorrelationId;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCorsPolicy();
builder.Services.AddApiVersioningConfig();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMediatR(typeof(Application.AssemblyReference).Assembly);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHttpContextAccessor();
builder.Services.AddDefaultCorrelationId();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddAuthentication("XenForoSession")
    .AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>("XenForoSession", options => { });

builder.Services.AddAuthorization();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");


app.UseCorrelationId();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseMiddleware<SessionMiddleware>();
app.UseAuthorization();

app.UseMiddleware<SecureResponseMiddleware>(); 
app.MapControllers();
app.Run();