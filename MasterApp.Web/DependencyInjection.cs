using MasterApp.Application.Com.Login;
using MasterApp.Application.Interface;
using MasterApp.Application.Setup.MasterApp;
using MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;
using MasterApp.Service.DbContext;
using MasterApp.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
        services.AddScoped<UpdateDatabaseNavCloudPosDBKMART>();
        services.AddScoped<UpdateNavCloudPosDBKMART>();
        services.AddScoped<RoleCreateCloudPosDBKMART>();
        services.AddScoped<GetRoleCloudPosDBKMART>();
        services.AddScoped<GetMenuIdToTheRoleCloudPosDBKMART>();
        services.AddScoped<UpdateMenuIdToTheRoleCloudPosDBKMART>();
        services.AddScoped<GetUserCloudPosDBKMART>();
        services.AddScoped<GetRoleDDCloudPosDBKMART>();
        services.AddScoped<AssignUserMenuCloudPosDBKMART>();
        services.AddScoped<LoginCommand>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHash, PasswordHash>();
        services.AddScoped<UpdateProject>();
        services.AddScoped<DeleteProject>();
        return services;
    }
}
