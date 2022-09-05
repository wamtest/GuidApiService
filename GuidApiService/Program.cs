using GuidApiService.DataProviders;
using GuidApiService.Models;
using GuidApiService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var isSqlServer = builder.Configuration.GetValue<bool>("DbIsSqlServer");
var dbConnString = builder.Configuration.GetValue<string>("ConnectionStrings:GuidApiServiceDatabase");
var shouldUseCache = builder.Configuration.GetValue<bool>("UseCache");
var cacheConnString = builder.Configuration.GetValue<string>("ConnectionStrings:Redis");
var cacheName = builder.Configuration.GetValue<string>("RedisCacheName");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<IGuidService, GuidService>();
//Add DbContext
if (isSqlServer)
{
    builder.Services.AddDbContext<GuidServiceContext>(opt => opt.UseSqlServer(dbConnString));

    if (shouldUseCache)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
         {
             options.Configuration = builder.Configuration.GetConnectionString("cacheConnString");
             options.InstanceName = cacheName;
         });
    }
}
else
{
    builder.Services.AddDbContext<GuidServiceContext>(opt => opt.UseInMemoryDatabase("GuidInfoSet"));
}

builder.Services.AddScoped<IGuidApiRepository<GuidInfo>, GuidApiRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Guid API Service",
        Description = "Guid and metadata generator",
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

if (isSqlServer)
{
    using (var scope = app.Services.CreateScope())
    {
        using (var dbContext = scope.ServiceProvider.GetService<GuidServiceContext>())
        {
            if (dbContext != null)
            {
                dbContext.Database.EnsureCreated();
            }
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
