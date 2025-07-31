using MasterApp.Application.Interface;
using MasterApp.Application.Setup.MasterApp;
using MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;
using MasterApp.Service.DbContext;

namespace MasterApp.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<GetNavCloudPosDBKMART>();
        services.AddScoped<CreateNavCloudPosDBKMART>();
        services.AddScoped<CreateProject>();
        services.AddSingleton<IDbConnectionFactory, DapperConnectionFactory>();
        services.AddScoped<GetParentNavCloudPosDBKMART>();
        services.AddScoped<GetProjectList>();
        services.AddScoped<UpdateNavCloudPosDBKMART>();

        return services;
    }
}
