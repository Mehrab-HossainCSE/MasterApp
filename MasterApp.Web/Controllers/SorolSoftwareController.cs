
using MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;
using MasterApp.Application.Setup.SlaveApp.BillingSoftware.RoleManagement;
using MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;
using MasterApp.Application.Setup.SlaveApp.SorolSoftwate.CompanySet;
using MasterApp.Application.Setup.SlaveApp.SorolSoftwate.NavSetting;
using MasterApp.Application.Setup.SlaveApp.SorolSoftwate.RoleManagement;
using MasterApp.Application.Setup.SlaveApp.SorolSoftwate.UserManagement;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using Microsoft.AspNetCore.Mvc;

namespace MasterApp.Web.Controllers;


[ApiController]
[Route("api/[controller]/[action]")]
public class SorolSoftwareController(GetSorolNav _getNav, GetParentNavSorol getParentNavSorol, CreateNavSorol createNavSorol, UpdateSorolNavs updateSorolNavs,
    UpdateSorolSoftDatabaseNav updateSorolSoftDatabaseNav, CreateSorolRole createSorolRole, GetSorolRole getSorolRole, GetMenuRoleByIdSorol getMenuRoleByIdSorol
    , UpdateMenuIdForRoelSorol updateMenuIdForRoelSorol, GetAllUserSorol getAllUserSorol, GetMenuByRoleSorol menuByRoleSorol, AssignUserMenuSorol assignUserMenuSorol,
    GetNavSorolMediaSoft getNavSorolMediaSoft,
    GetCompanyInfo companyInfo, GetRoleSorolUserAssing getRoleSorolUserAssing) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetNav()
    {
        var result = await _getNav.GetNavsAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetNavMediaSoft()
    {
        var result = await getNavSorolMediaSoft.GetNavsAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetParentNav()
    {
        var result = await getParentNavSorol.GetParentsAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateNav([FromForm] CreateSorolSoftNavDto dto)
    {
        var result = await createNavSorol.InsertAsync(dto);
        if (result > 0)
            return Ok(new { success = true, message = "Data inserted successfully" });

        return BadRequest(new { success = false, message = "Duplicate SERIAL" });
    }
    [HttpPost]
    public async Task<IActionResult> UpdateNav([FromForm] CreateSorolSoftNavDto dto)
    {
        var result = await updateSorolNavs.UpdateAsync(dto);
        if (result > 0)
            return Ok(new { success = true, message = "Data update successfully" });

        return BadRequest(new { success = false, message = "Failed to update data" });
    }
    [HttpPost]
    public async Task<IActionResult> UpdateDatabaseNav([FromBody] List<SorolSoftDatabaseDto> dto)
    {
        var result = await updateSorolSoftDatabaseNav.UpdateNavAsync(dto);

        if (result.Succeeded)
        {
            return Ok(new { success = true, message = result.Messages.FirstOrDefault() });
        }

        return BadRequest(new { success = false, message = result.Messages.FirstOrDefault() });
    }
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateSorolRoleDto dto)
    {
        var result = await createSorolRole.InsertRoleAsync(dto);

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetRoles()
    {
        var result = await getSorolRole.GetAllRolesAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetRoleWiseMenuSorol([FromBody] int ID)
    {
        var result = await getMenuRoleByIdSorol.GetMenusByRoleAsync(ID);

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateMenuIdToTheRoleSorol([FromBody] RoleMenuListUpdateDto dto)
    {
        var result = await updateMenuIdForRoelSorol.UpdateMenuIdsForRoleAsync(dto);

        if (result.Succeeded)
            return Ok(new { success = true, message = "Menu IDs updated successfully." });

        return BadRequest(new { success = false, message = "Failed to update menu IDs." });
    }
    [HttpPost]
    public async Task<IActionResult> GetAllUser()
    {
        var result = await getAllUserSorol.GetAllUserAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetmenuByRoleSorol()
    {
        var result = await menuByRoleSorol.GetAllMenuDetailsAsync();

        if (result == null )
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetmenuByRoleSorolUser()
    {
        var result = await getRoleSorolUserAssing.GetAllRolesAsync();

        if (result == null)
            return NotFound("No parent menus found.");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AssignUserMenuSorol([FromBody] UserMenuSorolDto dto)
    {
        var result = await assignUserMenuSorol.AssignUserMenu(dto);

        if (result.Succeeded)
            return Ok(new { success = true, message = "Menu IDs updated successfully." });

        return BadRequest(new { success = false, message = "Failed to update menu IDs." });
    }

    [HttpPost]
    public async Task<IActionResult> GetAllCompanyList()
    {
        var result = await companyInfo.GetAllCompanyList();

        if (result == null)
            return NotFound("No parent menus found.");

        return Ok(result);
    }
}
