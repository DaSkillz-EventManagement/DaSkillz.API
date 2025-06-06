﻿using Application.Abstractions.Authentication;
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
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Elasticsearch.Net;
using Infrastructure.ExternalServices.Authentication;
using Infrastructure.ExternalServices.Authentication.Setting;
using Infrastructure.ExternalServices.AvatarApi;
using Infrastructure.ExternalServices.AvatarApi.Setting;
using Infrastructure.ExternalServices.BackgroundTask;
using Infrastructure.ExternalServices.Caching;
using Infrastructure.ExternalServices.Caching.Setting;
using Infrastructure.ExternalServices.ElasticSearch;
using Infrastructure.ExternalServices.ElasticSearch.Setting;
using Infrastructure.ExternalServices.Email;
using Infrastructure.ExternalServices.Email.Setting;
using Infrastructure.ExternalServices.Images;
using Infrastructure.ExternalServices.Oauth2;
using Infrastructure.ExternalServices.Oauth2.Setting;
using Infrastructure.ExternalServices.Payment.ZaloPay;
using Infrastructure.ExternalServices.Payment.ZaloPay.Setting;
using Infrastructure.ExternalServices.Quartz;
using Infrastructure.ExternalServices.Quartz.PaymentScheduler;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nest;
using Quartz;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

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
            services.Configure<AvatarApiSetting>(configuration.GetSection("AvatarApi"));
            services.Configure<GoogleSetting>(configuration.GetSection("GoogleToken"));



            //Add DBcontext
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    //configuration.GetConnectionString("local"),
                    configuration.GetConnectionString("production"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                options.UseLazyLoadingProxies();
            });

            //Add redis
            var redisConnection = configuration["Redis:HostName"];
            var redisDatabase = ConnectionMultiplexer.Connect($"{redisConnection},abortConnect=false");
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return redisDatabase;
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
            //var settings = new ElasticsearchClientSettings(new Uri(elasticsearchSettings.Url!))
            //    .Authentication(new BasicAuthentication(elasticsearchSettings.Username!, elasticsearchSettings.Password!));
            //var client = new ElasticsearchClient(settings);

            var pool = new SingleNodeConnectionPool(new Uri(elasticsearchSettings.Url!));
            var settings = new ConnectionSettings(pool)
                .EnableApiVersioningHeader();

            var client = new ElasticClient(settings);


            //Add SignalR
            services
                .AddSignalR(option =>
                {
                    option.EnableDetailedErrors = true;
                    option.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
                    option.MaximumReceiveMessageSize = 5 * 1024 * 1024; // 5MB
                })
                .AddJsonProtocol(option =>
                {
                    option.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            //add life time for the services

            //add distributed lock with redis in DI container
            services.AddSingleton<IDistributedLockProvider>(_ =>
            new RedisDistributedSynchronizationProvider(redisDatabase!.GetDatabase()));

            //services.AddSingleton(client);
            services.AddSingleton<IElasticClient>(client);
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
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IAdvertisedEventRepository, AdvertisedEventRepository>();
            services.AddScoped<IEventStatisticsRepository, EventStatisticsRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();

            //Quiz service
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IUserAnswerRepository, UserAnswerRepository>();

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
            services.AddQuartz(q =>
            {
                var checkJobKey = new JobKey("CheckTransactionStatusJob");
                q.AddJob<CheckTransactionStatusJob>(opts => opts.WithIdentity(checkJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(checkJobKey)
                    .WithIdentity("CheckTransactionStatusTrigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(1)
                        .RepeatForever()
                        .Build()));

                var deactivateJobKey = new JobKey("DeactivateExpiredSubscriptionsJob");
                q.AddJob<DeactivateExpiredSubscriptionsJob>(opts => opts.WithIdentity(deactivateJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(deactivateJobKey)
                    .WithIdentity("DeactivateExpiredSubscriptionsTrigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(1)  // Run every 1 minute
                        .RepeatForever()));

                //var provideCertificate = new JobKey("ProvideCertificate");
                //q.AddJob<ProvideCertificateAfterEventEnded>(opts => opts.WithIdentity(provideCertificate));
                //q.AddTrigger(opts => opts
                //    .ForJob(provideCertificate)
                //    .WithIdentity("ProvideCertificateTrigger")
                //    .StartNow()
                //    .WithSimpleSchedule(x => x
                //        .WithIntervalInMinutes(1)  // Run every 1 minute
                //        .RepeatForever()));

                var jobKey = new JobKey("AllEventStatusToEndedJob");
                var jobKey2 = new JobKey("AllEventStatusToOngoingJob");
                q.AddJob<AllEventStatusToOngoingJob>(opts => opts.WithIdentity(jobKey2));
                q.AddJob<AllEventStatusToEndedJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts.ForJob(jobKey2)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(3600).WithRepeatCount(1).Build())
                    .WithDescription("Auto update status for all events")
                );
                q.AddTrigger(opts => opts.ForJob(jobKey)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(3600).WithRepeatCount(1).Build())
                    .WithDescription("Auto update status for all events")
                );
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            return services;
        }
    }
}
