using API.Configuration;
using API.Middleware;
using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Elasticsearch.Setting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ElasticSetting>(
    builder.Configuration.GetSection("ElasticsearchSettings"));

builder.ConfigureSerilog();

builder.Services.AddScoped<GlobalException>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureHealthChecks(builder.Configuration);

builder.Services.AddSwagger();

builder.Services.AddCors();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(10);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
//default services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//build phrase
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
}

app.UseCors(app => app
    .AllowAnyOrigin()
        .AllowAnyMethod()
            .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.ConfigureSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

//app.UseSession();

app.MapHealthChecks("/h", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.ConfigureExceptionHandler();

app.Run();
