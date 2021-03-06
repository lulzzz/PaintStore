﻿using System;
using System.IO;
using System.Reflection;
using Akka.Actor;
using AutoMapper;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodingBlast;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using PaintStore.Application.Actors;
using PaintStore.Application.Actors.Services;
using PaintStore.Application.Interfaces;
using PaintStore.Application.Services;
using PaintStore.BackEnd.Middlewares;
using PaintStore.Domain.UploadModels;
using PaintStore.Persistence;

namespace PaintStore.BackEnd
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //var settingsSection = Configuration.GetSection("AppIdentitySettings");
            //settingsSection.Get<AppIdentitySettings>();

            services.Configure<AppIdentitySettings>(Configuration.GetSection("AppIdentitySettings"));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    opt => opt.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAllOrigins", build =>
            //    {

            //        build.AllowAnyOrigin();
            //        build.AllowAnyHeader();
            //        build.AllowAnyMethod();
            //        build.AllowCredentials();
            //    });
            //});

            services.AddSpaStaticFiles(c => { c.RootPath = "wwwroot";});

            services.AddAutoMapper();
            
            services.AddTransient<IPostsService, PostService>();
            services.AddTransient<ILikesService, LikesService>();
            services.AddTransient<ISignInService, SignInService>();
            services.AddTransient<ITagsService, TagsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IFollowersService, FollowersService>();
            services.AddTransient<IPostCommentsService, PostCommentsService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "My API",
                    Version = "v1"
                });
                //Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);
            });
                

            services.AddSingleton<IActivityManagerStartup, ActivityManager>();
            services.AddMvc().AddControllersAsServices();

            services.AddSingleton<ITagHelperComponent>(new GoogleAnalyticsTagHelperComponent("UA-136426296-1"));

            DBContextCreate.env = _env;
            if (_env.IsDevelopment())
            {

                var connectionString = Configuration.GetConnectionString("PaintStoreDatabase");
                DBContextCreate.connectionString = connectionString;
                if (connectionString == null)
                {
                    services.AddDbContext<PaintStoreContext>(options =>
                        options.UseSqlite("Data Source=PaintStore.db"));
                }
                services.AddDbContext<PaintStoreContext>(
                    options => options.UseSqlServer(connectionString));
            }
            else
            {
                var connectionString = Configuration.GetConnectionString("PaintStoreDatabase");

                DBContextCreate.connectionString = connectionString;

                services.AddDbContext<PaintStoreContext>(
                    options => options.UseSqlServer(connectionString)
                );
            }
            //Now register our services with Autofac container

            var builder = new ContainerBuilder();

            var actorSystem = ActorSystem.Create("PSActorSystem");
            builder.Register(_ => actorSystem);
            var actorActivity = actorSystem.ActorOf(Props.Create(() => new ActivityActor()));
            var actorSupervisor = actorSystem.ActorOf(Props.Create(() => new SupervisorActor(actorActivity)));
            builder.Register(c => actorSupervisor);
            builder.Populate(services);
            var container = builder.Build();


            //Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IActivityManagerStartup activityManager, IServiceScopeFactory _scopeFactory)
        {
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSwagger();
            app.UseDeveloperExceptionPage();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "my API V1"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
             

            activityManager.RunManager();

            app.UseCors("CorsPolicy");

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "api/{controller}/{action}/{id?}");

            routes.MapSpaFallbackRoute(
                name: "spa-fallback",
                defaults: new { controller = "Home", action = "Index" });
        });

            app.UseSpa(spa =>
            {
                
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

    }
}
