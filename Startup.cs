using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intex_app.DataContext;
using Intex_app.Infrastructure;
using Intex_app.Models;
using Intex_app.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Intex_app
{
    public class Startup
    {
        private const string SecretKey = "this.should.be.a.better.secret.key.from.the.environment";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AccountDataContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(Configuration["ConnectionSTrings:IntexDBConnection"]);
            });
            services.AddDbContext<AuthDataContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(Configuration["ConnectionSTrings:IntexDBConnection"]);
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<AuthDataContext>().AddDefaultTokenProviders();

            services.AddSession();
            services.AddScoped<Token>(sp => SessionToken.GetCart(sp));

            services.AddControllersWithViews();
            services.AddMvc();

            var tokenValidationParamaters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKeys = new List<SecurityKey> { _signingKey },

                ValidateLifetime = true,
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParamaters;
                    options.RequireHttpsMetadata = false;
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 401;
                            c.Response.ContentType = "text/plain";

                            return c.Response.WriteAsync(c.Exception.ToString());
                        }
                    };
                });
            var jwtAppSettingsOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                options.ValidFor = TimeSpan.FromDays(14);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseSession();



            app.Use(async (context, next) =>
            {
                SessionToken JWToken = context.Session.GetJason<SessionToken>("Token")
                                ?? new SessionToken();
                if (!string.IsNullOrEmpty(JWToken.TokenString))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken.TokenString);
                }
                await next();
            });
            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseMvc();

            //AccountDataContext ac_context = app.ApplicationServices.
            //    CreateScope().ServiceProvider.GetRequiredService<AccountDataContext>();

            //if (ac_context.Database.GetPendingMigrations().Any())
            //{
            //    ac_context.Database.Migrate();
            //}

            //AuthDataContext au_context = app.ApplicationServices.
            //    CreateScope().ServiceProvider.GetRequiredService<AuthDataContext>();

            //if (au_context.Database.GetPendingMigrations().Any())
            //{
            //    au_context.Database.Migrate();
            //}
        }
    }
}
