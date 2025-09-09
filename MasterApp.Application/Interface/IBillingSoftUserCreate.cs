using MasterApp.Application.Common.Models;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Interface;

public interface IBillingSoftUserCreate
{
    Task<IResult> CreateUserBilling(BillingUserCreateDto dto);
    
}
