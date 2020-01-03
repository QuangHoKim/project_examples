using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vnn88.Repository;
using Vnn88.Common.Infrastructure;
using Vnn88.DataAccess.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Vnn88.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Http.Features;
using Vnn88.Service;
using Vnn88.Web.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Vnn88.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="locOptions"></param>
        public Startup(IConfiguration configuration, IOptions<RequestLocalizationOptions> locOptions)
        {
            Configuration = configuration;
            LocOptions = locOptions;
        }

        /// <summary>
        /// LocOptions
        /// </summary>
        public IOptions<RequestLocalizationOptions> LocOptions;
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region Bind Strongly Type Config

            //var jwtSection = Configuration.GetSection(Constants.Settings.JwtSection);
            //var jwtOptions = new JwtOptionsModel();
            //jwtSection.Bind(jwtOptions);
            //services.Configure<JwtOptionsModel>(jwtSection);

            #endregion

            #region framework services

            // configration for localiztion
            services.AddLocalization(options => options.ResourcesPath = Constants.Settings.ResourcesDir);
            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo(Configuration[Constants.Settings.DefaultCulture]),
                        new CultureInfo(Configuration[Constants.Settings.OptionCulture]),
                    };

                    opts.DefaultRequestCulture = new RequestCulture(Configuration[Constants.Settings.DefaultCulture]);
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                });

            services.Configure<FormOptions>(options => options.BufferBody = true);

            // Service for init mapping
            services.AddAutoMapper();

            // Config authentication
            services.AddAuthentication(options =>
                {
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(option =>
                {
                    //option.ExpireTimeSpan = TimeSpan.FromDays(int.Parse(Configuration[Constants.Settings.LoginExpiredTime]));
                    option.AccessDeniedPath = Constants.Route.AccessDenied;
                });

            // Add bearer authentication
            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    //cfg.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    //    ValidIssuer = jwtOptions.Issuer,
                    //    ValidAudience = jwtOptions.Issuer,
                    //    // Validate the token expiry  
                    //    ValidateLifetime = true,

                    //    ClockSkew = TimeSpan.Zero
                    //};
                });
            // Config the db connection string
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(Constants.Settings.DefaultConnection),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(Constants.Settings.VshopDataAccess);
                        // Configuring Connection Resiliency: 
                        // https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                    });
                // Changing default behavior when client evaluation occurs to throw. 
                // Default in EF Core would be to log a warning when client evaluation is performed.
                options.ConfigureWarnings(
                    warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                // Check Client vs. Server evaluation: 
                // https://docs.microsoft.com/en-us/ef/core/querying/client-eval
            });

            // Cors Config
            services.AddCors(options =>
            {
                options.AddPolicy(Constants.Swagger.CorsPolicy,
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.AddLogging();

            // Adds a default in-memory implementation of IDistributedCache
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = Constants.AppSession;
                options.IdleTimeout = TimeSpan.FromMinutes(Convert.ToDouble(Configuration[Constants.Settings.ExpiredSessionTime] ?? "60"));
            });
            // Custom service
            services.AddRouting(options => options.LowercaseUrls = true);

            ValidatorOptions.LanguageManager = new LanguageManager(LocOptions);
            #endregion


            #region Application services
            // Common services
            //services.AddScoped<IJwtHandler, JwtHandler>();
            //services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ValidateRequestFilter>();
            services.AddScoped<AuthorizeLoginFilter>();
            services.AddScoped<IUsersService, UsersService>();
            // Services bussiness
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.Configure<FormOptions>(x =>
            {
                x.BufferBody = true;
                x.ValueCountLimit = 4096;
                x.MultipartBodyLengthLimit = 52428800;
            });
            // Services bussiness
            services.AddMvc(options =>
            {
                options.Filters.Add(new TempDateFilter());
                options.Filters.Add(new PaginationFilter());
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.ReferenceLoopHandling =
                    ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            })
            .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddSessionStateTempDataProvider()
                .AddFluentValidation(fvc =>
                    fvc.RegisterValidatorsFromAssemblyContaining<Startup>()).AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = Constants.Settings.ResourcesDir; })
                .AddDataAnnotationsLocalization();
            //services.BuildServiceProvider();
            #endregion

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            Helpers.Configuration = configuration;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(Constants.Route.ErrorPage);
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == StatusCodes.Status404NotFound
                    && !context.Response.HasStarted)
                {
                    context.Request.Path = Constants.Route.NotFound;
                    await next();
                }
            });
            loggerFactory.AddFile(Configuration[Constants.Logger.LogFile], LogLevel.Error);
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
