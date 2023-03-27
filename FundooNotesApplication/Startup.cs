using CloudinaryDotNet;
using ManagerLayer.Interface;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using System;
using System.Net;
using System.Text;

namespace FundooNotesApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))

                    };

                });


            services.AddControllers();
            
            
            services.AddDbContext<FunContext>(o => o.UseSqlServer(Configuration["ConnectionStrings:FundooDB"]));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddSingleton(o =>
            {
                var config = o.GetService<IConfiguration>();
                return new Cloudinary(new Account(
                    config["Cloudinary:CloudName"],
                    config["Cloudinary:ApiKey"],
                    config["Cloudinary:ApiSecret"]
                    ));

            });
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {

                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });

            services.AddControllers();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });

            services.AddTransient<INotesRepository, NotesRepository>();
            services.AddTransient<INoteManager, NoteManager>();
            services.AddTransient<ICollabRepository, CollabRepository>();
            services.AddTransient<ICollabManager, CollabManager>();

            services.AddTransient<ILabelRepository, LabelRepository>();
            services.AddTransient<ILabelManager, LabelManager>();
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Fundoo Application ",
                    Description = "ASP.NET Core 3.1 Web API"
                });
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your token in the text input below",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }

                        }
                });
            });

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // This middleware serves generated Swagger document as a JSON endpoint
            app.UseSwagger();

            // This middleware serves the Swagger documentation UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Registration API ");
            });
        }
    }
}
