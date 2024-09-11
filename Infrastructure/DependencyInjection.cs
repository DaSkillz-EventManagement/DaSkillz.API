using Application.Abstractions.Authentication;
using Application.Abstractions.AvatarApi;
using Application.Abstractions.Caching;
using Application.Abstractions.ElasticSearch;
using Application.Abstractions.Email;
using Application.Abstractions.Oauth2;
using Application.ExternalServices.BackgroundTák;
using Application.ExternalServices.Images;
using Application.ExternalServices.Mail;
using Application.ExternalServices.Quartz;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Infrastructure.ExternalServices.Authentication;
using Infrastructure.ExternalServices.Authentication.Setting;
using Infrastructure.ExternalServices.AvatarApi;
using Infrastructure.ExternalServices.BackgroundTask;
using Infrastructure.ExternalServices.Caching;
using Infrastructure.ExternalServices.Caching.Setting;
using Infrastructure.ExternalServices.ElasticSearch;
using Infrastructure.ExternalServices.ElasticSearch.Setting;
using Infrastructure.ExternalServices.Email;
using Infrastructure.ExternalServices.Email.Setting;
using Infrastructure.ExternalServices.Images;
using Infrastructure.ExternalServices.Oauth2;
using Infrastructure.ExternalServices.Quartz;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
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
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EmailSetting>(configuration.GetSection("SmtpSettings"));


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
                    ValidIssuer = configuration.GetSection("JWTSettings:Issuer").Get<string>(),
                    ValidAudience = configuration.GetSection("JWTSettings:Audience").Get<string>(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTSettings:Securitykey").Get<string>())),
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
            var settings = new ElasticsearchClientSettings(new Uri(elasticsearchSettings.Url!))
                .Authentication(new BasicAuthentication(elasticsearchSettings.Username!, elasticsearchSettings.Password!));
            var client = new ElasticsearchClient(settings);

            //add life time for the services
            services.AddSingleton(client);
            services.AddSingleton<IRedisCaching, RedisCaching>();
            services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));
            services.AddScoped<IUnitOfWork>(provider => (IUnitOfWork)provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ILogoRepository, LogoRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ISponsorEventRepository, SponsorEventRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IAvatarApiClient, AvatarApiClient>();
            services.AddScoped<IGoogleTokenValidation, GoogleTokenValidation>();
            //services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IEmailService, EmailServices>();
            services.AddScoped<IQuartzService, QuartzService>();
            services.AddScoped<ISendMailTask, SendMailTask>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddQuartz();

            return services;
        }
    }
}
