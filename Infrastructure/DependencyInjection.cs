using Domain.Repository.UnitOfWork;
using Elastic.Clients.Elasticsearch;
using Infrastructure.Caching;
using Infrastructure.Caching.Setting;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Elasticsearch;
using Infrastructure.Persistence.Elasticsearch.Setting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //transfer data from appsetting.json to the correspondeding setting class
            services.Configure<RedisSetting>(configuration.GetSection("Redis"));
            services.Configure<ElasticSetting>(configuration.GetSection("ELasticSearch"));


            //Add DBcontext
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("local"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                options.UseLazyLoadingProxies();
            });

            //Add redis
            services.AddStackExchangeRedisCache(options =>
            {
                var redisConnection = configuration["Redis:HostName"];
                //var redisPassword = configuration["Redis:Password"];
                //options.Configuration = $"{redisConnection},password={redisPassword}";
                options.Configuration = redisConnection;
            });
            services.AddDistributedMemoryCache();

            //Add JWTconfig
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration.GetSection("JWTSetting:Issuer").Get<string>(),
                    ValidAudience = configuration.GetSection("JWTSetting:Audience").Get<string>(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTSetting:Securitykey").Get<string>())),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    //ClockSkew = TimeSpan.Zero
                };
            });

            //Add ElasticSearch
            var elasticsearchSettings = configuration.GetSection("ELasticSearch").Get<ElasticSetting>();
            var settings = new ElasticsearchClientSettings(new Uri(elasticsearchSettings.Url));
            var client = new ElasticsearchClient(settings);

            //add life time for the services
            services.AddSingleton(client);
            services.AddSingleton<IRedisCaching,  RedisCaching>();
            services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }
}
