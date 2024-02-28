using System.Reflection;
using System.Text.Json.Serialization;
using Application.Validators;
using FluentMigrator.Runner;
using FluentValidation.AspNetCore;
using Infrastructure.Contracts;
using Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true) //TODO: Записать себе эту божественную строчку, которую я искал целый день
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateEmployeeCommandValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Employee Web Service", 
        Version = "v1",
        Description = "Ограничения по базе данных:<br>1) Название Department уникальное;<br>2) Номер паспорта уникальный."
    });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.Configure<DatabaseConnectionOptions>(builder.Configuration.GetSection("DatabaseConnectionOptions"));
builder.Services.AddMediatR(typeof(Program), typeof(DatabaseConnectionOptions));
builder.Services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var connectionString = builder.Configuration["DatabaseConnectionOptions:ConnectionString"];

builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(r => 
        r.AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(typeof(EmployeeTable).Assembly)
            .For.Migrations());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Web Service v1"));
    
    using var scope = ((IApplicationBuilder) app).ApplicationServices.CreateScope();
    var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();
    migrator.MigrateUp();
}

app.MapControllers();

await app.RunAsync();