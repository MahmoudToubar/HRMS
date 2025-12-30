using API.Extensions;
using API.Mapping;
using API.Middleware;
using API.Services;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Validators;
using Core.Validators.AccountValidator;
using Core.Validators.DepartmentValidator;
using Core.Validators.EmployeeValidator;
using Core.Validators.JobValidator;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddDbContext<TamweelyHrDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<JwtTokenService>();

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IExcelExportService, ExcelExportService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDepartmentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateJobValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateJobValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateDepartmentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateEmployeeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();


builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseMiddleware<ExceptionMiddleware>();


app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
.WithOrigins("http://localhost:4200"));


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Core.Identity.AppUser>>();
    var roleManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
    await DbInitializer.SeedAsync(userManager, roleManager);
}

app.Run();
