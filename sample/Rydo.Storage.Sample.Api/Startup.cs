namespace Rydo.Storage.Sample.Api
{
    using System.Reflection;
    using Prometheus;
    using Storage.Extensions;

    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", Assembly.GetEntryAssembly()?.GetName().Name));
            }

            app.UseStorage();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
                endpoints.MapControllers();
            });
        }
    }
}