using HepsiYemek.Products.Data.Abstract;
using HepsiYemek.Products.Data.Concrete;
using HepsiYemek.Products.Services.Abstract;
using HepsiYemek.Products.Services.Concrete;
using HepsiYemek.Products.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace HepsiYemek.Products
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


            #region EventBus

            //services.AddSingleton<IRabbitMQPersistentConnection>(sp => {
            //    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

            //    var factory = new ConnectionFactory()
            //    {
            //        HostName = Configuration["EventBus:HostName"]
            //    };

            //    if (!string.IsNullOrWhiteSpace(Configuration["EventBus:UserName"]))
            //    {
            //        factory.UserName = Configuration["EventBus:UserName"];
            //    }

            //    if (!string.IsNullOrWhiteSpace(Configuration["EventBus:Password"]))
            //    {
            //        factory.UserName = Configuration["EventBus:Password"];
            //    }

            //    var retryCount = 5;
            //    if (!string.IsNullOrWhiteSpace(Configuration["EventBus:RetryCount"]))
            //    {
            //        retryCount = int.Parse(Configuration["EventBus:RetryCount"]);
            //    }

            //    return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);
            //});

            //services.AddSingleton<EventBusOrderCreateConsumer>();

            #endregion

            #region Configuration Dependencies
            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));
            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            #endregion

            #region Project Dependencies
            services.AddTransient<ISourcingContext, SourcingContext>();
            services.AddTransient<IProductService, ProductService>();
            #endregion

            #region Swagger Dependencies
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HepsiYemek.Products",
                    Version = "v1"
                });
            });
            #endregion

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HepsiYemek.Products v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
