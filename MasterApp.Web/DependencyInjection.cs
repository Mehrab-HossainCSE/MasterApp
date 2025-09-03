using MasterApp.Application.Com.Login;
using MasterApp.Application.Interface;
using MasterApp.Application.Setup.MasterApp;
using MasterApp.Application.Setup.MasterApp.NavMasterApp;
using MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;
using MasterApp.Application.Setup.SlaveApp.BillingSoftware.RoleManagement;
using MasterApp.Application.Setup.SlaveApp.BillingSoftware.UserManagement;
using MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;
using MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.NavSettingCloudPosReportHerlanCheck;
using MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.RoleManagementCloudPosReportHerlanCheck;
using MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.UserManagementCloudPosReportHerlanCheck;
using MasterApp.Application.Setup.SlaveApp.SorolSoftwate.NavSetting;
using MasterApp.Application.Setup.SlaveApp.SorolSoftwate.RoleManagement;
using MasterApp.Application.Setup.SlaveApp.SorolSoftwate.UserManagement;
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
        services.AddScoped<GetCompanyInfoCloudPosDBKMART>();
        services.AddScoped<GetNavCloudPosReportHerlanCheck>();
        services.AddScoped<CreateNavCloudPosReportHerlanCheck>();
        services.AddScoped<GetParentNavCloudPosReportHerlanCheck>();
        services.AddScoped<UpdaterNavCloudPosReportHerlanCheck>();
        services.AddScoped<UpdateDatabaseNavCloudPosReportHerlanCheck>();
        services.AddScoped<GetUserCloudPosReportHerlanCheck>();
        services.AddScoped<GetRoleDDCloudPosReportHerlanCheck>();
        services.AddScoped<RoleCreateCloudPosReportHerlanCheck>();
        services.AddScoped<GetRoleCloudPosReportHerlanCheck>();
        services.AddScoped<GetMenuIdToTheRoleCloudPosReportHerlanCheck>();
        services.AddScoped<UpdateMenuIdToTheRoleCloudPosReportHerlanCheck>();
        services.AddScoped<AssigUserMenuCloudPosReportHerlanCheck>();
        services.AddScoped<GetNavProjectByUser>();
        services.AddScoped<GetAllUser>();
        services.AddScoped<UserCreate>();
        services.AddScoped<UserProjectPermission>();
        services.AddScoped<UpdateUserInfo>();
        services.AddScoped<GetNav>();
        services.AddScoped<GetParentNav>();
        services.AddScoped<CreateNav>();
        services.AddScoped<UpdateNav>();
        services.AddScoped<UpdateDatabaseNav>();
        services.AddScoped<GetRole>();
        services.AddScoped<CreateRole>();
        services.AddScoped<UpdateRole>();
        services.AddScoped<GetUser>();
        services.AddScoped<RoleWiseMenu>();
        services.AddScoped<UpdateUserRole>();
        services.AddScoped<GetSorolNav>();
        services.AddScoped<GetParentNavSorol>();
        services.AddScoped<CreateNavSorol>();
        services.AddScoped<UpdateSorolNavs>();
        services.AddScoped<UpdateSorolSoftDatabaseNav>();
        services.AddScoped<CreateSorolRole>();
        services.AddScoped<GetSorolRole>();
        services.AddScoped<GetMenuRoleByIdSorol>();
        services.AddScoped<UpdateMenuIdForRoelSorol>();
        services.AddScoped<GetAllUserSorol>();
        services.AddScoped<GetMenuByRoleSorol>();
        services.AddScoped<AssignUserMenuSorol>();
        services.AddSingleton<IEncryption>(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            var key = config["Encryption:Key"];  // read from appsettings.json
            return new EncryptionHelper(key);
        });

        return services;
    }
}
