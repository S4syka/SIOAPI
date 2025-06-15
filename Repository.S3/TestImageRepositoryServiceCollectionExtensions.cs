using Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Repository.S3;

public static class TestImageRepositoryServiceCollectionExtensions
{
    public static IServiceCollection AddTestImageRepository(this IServiceCollection services, IConfiguration configuration)
    {
        var useMock = configuration.GetValue<bool>("UseMockTestImageRepository", true);
        if (useMock)
        {
            services.AddScoped<ITestImageRepository, TestImageMockRepository>();
        }
        else
        {
            services.AddScoped<ITestImageRepository, TestImageRepository>();
        }
        return services;
    }
}
