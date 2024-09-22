using Application.Abstractions.Authentication;
using Application.Abstractions.AvatarApi;
using Application.Abstractions.Caching;
using Application.Abstractions.ElasticSearch;
using Application.Abstractions.Email;
using Application.Abstractions.Oauth2;
using Application.Abstractions.Payment.ZaloPay;
using Application.ExternalServices.BackgroundTák;
using Application.ExternalServices.Images;
using Application.ExternalServices.Mail;
using Application.ExternalServices.Quartz;
using Application.Helper;
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
using Infrastructure.ExternalServices.Payment.ZaloPay;
using Infrastructure.ExternalServices.Payment.ZaloPay.Setting;
using Infrastructure.ExternalServices.Quartz;
using Infrastructure.ExternalServices.Quartz.PaymentScheduler;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using StackExchange.Redis;
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
            services.Configure<ZaloPaySetting>(configuration.GetSection("ZaloPay"));



            //Add DBcontext
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("local"),
                    //configuration.GetConnectionString("production"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                options.UseLazyLoadingProxies();
            });

            //Add redis
            var redisConnection = configuration["Redis:HostName"];
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(redisConnection);
            });

            services.AddStackExchangeRedisCache(options =>
            {
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
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IRefundTransactionRepository, RefundTransactionRepository>();
            services.AddScoped<IPriceRepository, PriceRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IAdvertisedEventRepository, AdvertisedEventRepository>();


            services.AddScoped<ISponsorEventRepository, SponsorEventRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IAvatarApiClient, AvatarApiClient>();
            services.AddScoped<IGoogleTokenValidation, GoogleTokenValidation>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IEmailService, EmailServices>();
            services.AddScoped<IZaloPayService, ZaloPayService>();
            services.AddScoped<IQuartzService, QuartzService>();
            services.AddScoped<ISendMailTask, SendMailTask>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddQuartz(q => {
                // Cấu hình CheckTransactionStatusJob
                var checkJobKey = new JobKey("CheckTransactionStatusJob");
                q.AddJob<CheckTransactionStatusJob>(opts => opts.WithIdentity(checkJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(checkJobKey)
                    .WithIdentity("CheckTransactionStatusTrigger")
                    .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(DateTime.UtcNow.AddMinutes(1)))));
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            return services;
        }
    }
}
