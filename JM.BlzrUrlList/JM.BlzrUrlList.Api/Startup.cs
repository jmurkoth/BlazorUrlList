using JM.BlzrUrList.Core.DB;
using JM.BlzrUrList.Core.Repository;
using JM.BlzrUrlList.Core.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace JM.BlzrUrlList.Api
{

    public class Startup
    {
        readonly string allowedOrigins = "allowedOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false; // Not needed
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(Configuration.GetSection("UrlListAzureAd")); // grab from the app settings
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters.NameClaimType = "name";
           
            });
            services.AddSingleton<IUrlRepository, InMemoryUrlRepository>();
            services.AddScoped<IOpenGraphRepository, OpenGraphRespository>();
            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));
            services.AddSingleton<IDatabaseSettings>(x => x.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(name: allowedOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin() // Use this if you want to allow all clients to access it
                                      //builder.WithOrigins("https://localhost:9091") // Only Blazor app allowed
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JM.BlzrUrlList.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JM.BlzrUrlList.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseCors(allowedOrigins);
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
