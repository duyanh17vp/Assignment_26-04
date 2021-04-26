using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebAPI.Extensions;
using WebAPI.Models;
using WebAPI.Repository;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                builder =>
                                {
                                    builder.WithOrigins("http://localhost:3000",
                                                        "http://localhost:5001")
                                                        .AllowAnyHeader()
                                                        .AllowAnyMethod();;
                                });
            });
            services.AddDbContext<LibraryDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("sqlConnection")));
            //
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie("Cookies", options =>
                    {
                        options.Cookie.Name = "auth_cookie";
                        options.Cookie.SameSite = SameSiteMode.None;
                        options.Events = new CookieAuthenticationEvents
                        {
                            OnRedirectToLogin = redirectContext =>
                            {
                                redirectContext.HttpContext.Response.StatusCode = 401;
                                return Task.CompletedTask;
                            }
                        };
                    });
            // repository Wrapper
            services.ConfigureRepositoryWrapper();
            // handel ReferenceLoop JsonException 
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IRequestDetailsRepository, RequestDetailsRepository>();
            services.AddTransient<IRequestOrderRepository, RequestOrderRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseCookiePolicy(
                new CookiePolicyOptions {
                    Secure = CookieSecurePolicy.Always
                }
            );

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
