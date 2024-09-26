using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Infrastructure.Repositories;

namespace UseCaseManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILogSourceRepository, LogSourceRepository>();
        services.AddScoped<ILogSourceFileRepository, LogSourceFileRepository>();
        services.AddScoped<IUseCaseRepository, UseCaseRepository>();
        services.AddScoped<IUseCaseFileRepository, UseCaseFileRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();

        //var asd = configuration["Storage:UseCaseDirectory"];
        services.AddDbContext<UseCaseDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("UseCase"));
        });

        services.AddScoped<IUnitOfWork>(s => s.GetRequiredService<UseCaseDbContext>());

        return services;
    }
}
