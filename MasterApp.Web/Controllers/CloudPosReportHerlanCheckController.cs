using MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;
using MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.NavSettingCloudPosReportHerlanCheck;
using MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.RoleManagementCloudPosReportHerlanCheck;
using MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.UserManagementCloudPosReportHerlanCheck;
using MasterApp.Application.SlaveDto;
using Microsoft.AspNetCore.Mvc;

namespace MasterApp.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CloudPosReportHerlanCheckController(GetNavCloudPosReportHerlanCheck _getNavCloudPosReportHerlanCheck,
    CreateNavCloudPosReportHerlanCheck _createNavCloudPosReportHerlanCheck,
    GetParentNavCloudPosReportHerlanCheck _getParentNavCloudPosReportHerlanCheck,
    UpdaterNavCloudPosReportHerlanCheck _updaterNavCloudPosReportHerlanCheck,
    UpdateDatabaseNavCloudPosReportHerlanCheck _updateDatabaseNavCloudPosReportHerlanCheck,
    GetUserCloudPosReportHerlanCheck _getUserCloudPosReportHerlanCheck,
    GetRoleDDCloudPosReportHerlanCheck _getRoleDDCloudPosReportHerlanCheck,
    RoleCreateCloudPosReportHerlanCheck _roleCreateCloudPosReportHerlanCheck,
    GetRoleCloudPosReportHerlanCheck _getRoleCloudPosReportHerlanCheck,
    GetMenuIdToTheRoleCloudPosReportHerlanCheck _getMenuIdToTheRoleCloudPosReportHerlanCheck,
    UpdateMenuIdToTheRoleCloudPosReportHerlanCheck _updateMenuIdToTheRoleCloudPosReportHerlanCheck,
    AssigUserMenuCloudPosReportHerlanCheck _assigUserMenuCloudPosReportHerlanCheck
    ) : ControllerBase
{
        
    [HttpPost]
    public async Task<IActionResult> GetNavCloudPosDBKMART()
    {
        var result = await _getNavCloudPosReportHerlanCheck.GetNavsJsonAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCloudPosReportHerlanCheck([FromForm] CreateNavCloudPosDBKMARTDto dto)
    {
        var result = await _createNavCloudPosReportHerlanCheck.InsertAsync(dto);
        if (result > 0)
            return Ok(new { success = true, message = "Data inserted successfully" });

        return BadRequest(new { success = false, message = "Duplicate SERIAL" });
    }
    [HttpPost]
    public async Task<IActionResult> GetParentNavCloudPosReportHerlanCheck()
    {
        var result = await _getParentNavCloudPosReportHerlanCheck.GetParentsAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateNavCloudPosReportHerlanCheck([FromForm] CreateNavCloudPosDBKMARTDto dto)
    {
        var result = await _updaterNavCloudPosReportHerlanCheck.UpdateAsync(dto);
        if (result > 0)
            return Ok(new { success = true, message = "Data update successfully" });

        return BadRequest(new { success = false, message = "Failed to update data" });
    }
    [HttpPost]
    public async Task<IActionResult> UpdateDatabaseNavCloudPosReportHerlanCheck([FromBody] List<CreateNavInputDto> dto)
    {
        var result = await _updateDatabaseNavCloudPosReportHerlanCheck.UpdateNavAsync(dto);

        if (result.Succeeded)
        {
            return Ok(new { success = true, message = result.Messages.FirstOrDefault() });
        }

        return BadRequest(new { success = false, message = result.Messages.FirstOrDefault() });
    }
    [HttpPost]
    public async Task<IActionResult> GetAllUser()
    {
        var result = await _getUserCloudPosReportHerlanCheck.GetAllUserAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetAllRole()
    {
        var result = await _getRoleDDCloudPosReportHerlanCheck.GetAllRolesAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> RoleCreateCloudPosReportHerlanCheck([FromBody] RoleCreateCloudPosReportHerlanCheckDto dto)
    {
        if (dto == null)
        {
            return BadRequest(new
            {
                Success = false,
                Message = "Invalid input data."
            });
        }

        var result = await _roleCreateCloudPosReportHerlanCheck.CreateRoleAsync(dto);

        if (result.Succeeded)
        {
            return Ok(new
            {
                Success = true,
                Message = result.Messages.FirstOrDefault() ?? "Role created successfully.",
                RoleId = result.Data
            });
        }

        return BadRequest(new
        {
            Success = false,
            Message = result.Messages.FirstOrDefault() ?? "Role creation failed.",
            Errors = result.Messages
        });
    }
    [HttpPost]
    public async Task<IActionResult> GetRoleCloudPosReportHerlanCheck()
    {
        var result = await _getRoleCloudPosReportHerlanCheck.GetAllRolesAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetRoleWiseMenuCloudPosReportHerlanCheck([FromBody] string Role_Name)
    {
        var result = await _getMenuIdToTheRoleCloudPosReportHerlanCheck.GetMenusByRoleAsync(Role_Name);

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateMenuIdToTheRoleCloudPosReportHerlanCheck([FromBody] RoleUpdateCloudPosReportHerlanCheckDto dto)
    {
        var result = await _updateMenuIdToTheRoleCloudPosReportHerlanCheck.UpdateMenuIdsForRoleAsync(dto);

        if (result.Succeeded)
            return Ok(new { success = true, message = "Menu IDs updated successfully." });

        return BadRequest(new { success = false, message = "Failed to update menu IDs." });
    }

    [HttpPost]
    public async Task<IActionResult> AssignUserMenu([FromBody] UserMenuDto dto)
    {
        var result = await _assigUserMenuCloudPosReportHerlanCheck.AssignUserMenu(dto);

        if (result.Succeeded)
            return Ok(new { success = true, message = "Menu IDs updated successfully." });

        return BadRequest(new { success = false, message = "Failed to update menu IDs." });
    }

}
