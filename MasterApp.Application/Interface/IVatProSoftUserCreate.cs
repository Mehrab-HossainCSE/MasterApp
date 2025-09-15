using MasterApp.Application.Common.Models;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.Setup.MasterApp;
using MasterApp.Application.SlaveDto;
namespace MasterApp.Application.Interface;

public interface IVatProSoftUserCreate
{
    Task<IResult> CreateUserVatPro(VatProUserCreateDto dto, string token);
    Task<string> getVatProToken(VatProTokenDto dto);
    Task<GetUserVatProDto> GetUserByUsername(string userName, string token);
    Task<IResult> UpdateUserVatPro(VatProUserUpdateDto dto, string token);
}
