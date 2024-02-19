using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.HubConfig;
using Server.Services;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                "CorsPolicy", builder => builder
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddSignalR();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services.AddSingleton<IRabbitMQHandler, RabbitMQHandler>();
            builder.Services.AddSingleton<IMessageListener, MessageListenerService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            /*builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });*/

            builder.Services.AddDbContext<DistributionContext>(options =>
                        options.UseLazyLoadingProxies()
                        .UseSqlServer(builder.Configuration.GetConnectionString("Distribution")));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<InventoryHub>("/data");

            app.Run();
        }
    }
}
