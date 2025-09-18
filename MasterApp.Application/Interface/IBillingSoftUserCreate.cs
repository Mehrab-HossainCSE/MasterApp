using MasterApp.Application.Common.Models;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.Setup.MasterApp;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Interface;

public interface IBillingSoftUserCreate
{
    Task<IResult> CreateUserBilling(BillingUserCreateDto dto);
    Task<Result<ApiResopnseBilling>> GetRoleBilling();
    Task<IResult> UpdateUserBilling(BillingUserUpdateDto dto);
    Task<Result<bool>> GetUserByUserNameBilling(string username);
}
