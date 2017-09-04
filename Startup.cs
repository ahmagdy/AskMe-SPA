using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Aspcorespa.Context;
using Aspcorespa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Aspcorespa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace WebApplicationBasic
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminAreaOnly", policy => policy.RequireRole("admin"));
                options.AddPolicy("MembersOnly", policy => policy.RequireRole("user", "admin"));
            });
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DBCNS")));
            services.AddIdentity<UserEntity, IdentityRole>().AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.Configure<IdentityOptions>(config =>
            {
                config.Password = new PasswordOptions
                {
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };
            });

            var siginKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("I Like to eat, like food."));
            var tokenParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = siginKey,
                ValidateLifetime = false,
                ValidateIssuer = false,
                ValidateAudience = false
            };


            services.AddAuthentication()
                        .AddCookie("Cookie",opt =>
                        {
                            opt.Cookie.Name = "access_token";
                            opt.Events = new CookieAuthenticationEvents
                            {
                                OnRedirectToLogin = ctx =>
                                {
                                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                                    {
                                        ctx.Response.StatusCode = 401;
                                        return Task.FromResult<object>(null);
                                    }
                                    ctx.Response.Redirect(ctx.RedirectUri);
                                    return Task.FromResult<object>(null);
                                }
                            };
                            opt.TicketDataFormat = new CustomJwtDataFormat(
                                                       SecurityAlgorithms.HmacSha256,
                                                       tokenParams);
                        })
                        .AddJwtBearer("Bearer", opt => {
                            opt.TokenValidationParameters = tokenParams;
                        });



            services.AddCors(options => options.AddPolicy("Cors", builder =>
             {
                 builder
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();

             }));

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var options = new RewriteOptions()
                            .AddRedirectToHttps();
            app.UseRewriter(options);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

             CreateRoles(serviceProvider).Wait();

   
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            app.UseAuthentication();

            app.UseStaticFiles();



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }


        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
            string[] roleNames = { "admin", "user" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var poweruser = new UserEntity
            {

                UserName = "admin",
                Email ="a@a.com",
                Name= "AdminUser"
            };
            string userPWD = "aaa111";
            var _user = await UserManager.FindByEmailAsync("a@a.com");

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, "admin");

                }
            }
        }
    }
}
