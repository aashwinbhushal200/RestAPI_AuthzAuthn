using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Configuration;

namespace RestAPI_AuthzAuthn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddMicrosoftIdentityWebApiAuthentication(IConfiguration config); 
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Optional: Use Authorization if you have role-based or policy-based authorization
            // app.UseAuthorization();
            app.UseAuthentication();
            app.Use(async (context, next) =>
            {
                if(!context.User.Identity?.IsAuthenticated?? false) 
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Unauthenticated");
                    }
                    else await next();
            });

            app.MapControllers();

            app.Run();
        }
    }
}
