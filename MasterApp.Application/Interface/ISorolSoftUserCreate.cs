using MasterApp.Application.Common.Models;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.Setup.MasterApp;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Interface;

public interface ISorolSoftUserCreate
{
    Task<IResult> CreateUserSorol(SorolUserCreateDto dto, string token);
    Task<string> getSorolToken(SorolTokenDto dto);
    Task<GetAllUserSorolDto?> GetUserByUsername(string userName);
    Task<IResult> UpdateUserSorol(SorolUserUpdateDto dto, string token);
   // Task<string> GetCompanySorol();
}
