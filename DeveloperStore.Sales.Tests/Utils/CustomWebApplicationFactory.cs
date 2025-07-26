using DeveloperStore.Sales.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 🧹 Remove o registro anterior do DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<SalesDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // ✅ Adiciona o DbContext InMemory para testes
            services.AddDbContext<SalesDbContext>(options =>
            {
                options.UseInMemoryDatabase("SalesTestDb");
            });

            // ✅ Garante que o banco é criado
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SalesDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
