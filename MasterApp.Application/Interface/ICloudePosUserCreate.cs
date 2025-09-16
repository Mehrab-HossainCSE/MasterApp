using MasterApp.Application.Common.Models;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Interface;

public interface ICloudePosUserCreate
{
    Task<IResult> CreateUserCloudePos(CloudPosUserDto dto, string token);
    Task<string> GetCloudePosApiKey(CloudPosApiKeyDto dto);
}
