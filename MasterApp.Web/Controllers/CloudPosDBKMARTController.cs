using MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;
using MasterApp.Application.SlaveDto;
using Microsoft.AspNetCore.Mvc;

namespace MasterApp.Web.Controllers;


[ApiController]
[Route("api/[controller]/[action]")]
public class CloudPosDBKMARTController : ControllerBase
{

    private readonly CreateNavCloudPosDBKMART _createNavCloudPosDBKMART;
    private readonly GetParentNavCloudPosDBKMART _getParentNavCloudPosDBKMART;
    private readonly GetNavCloudPosDBKMART _getNavCloudPosDBKMART;
    private readonly UpdateNavCloudPosDBKMART _updateNavCloudPosDBKMART;
    private readonly UpdateDatabaseNavCloudPosDBKMART _updateDatabaseNavCloudPosDBKMART;
    private readonly RoleCreateCloudPosDBKMART _roleCreateCloudPosDBKMART;
    private readonly GetRoleCloudPosDBKMART _getRoleCloudPosDBKMART;
    private readonly GetMenuIdToTheRoleCloudPosDBKMART _getMenuIdToTheRoleCloudPosDBKMART;
    private readonly UpdateMenuIdToTheRoleCloudPosDBKMART _updateMenuIdToTheRoleCloudPosDBKMART;
    private readonly GetUserCloudPosDBKMART _getUserCloudPosDBKMART;
    private readonly GetRoleDDCloudPosDBKMART _getRoleDDCloudPosDBKMART;
    private readonly AssignUserMenuCloudPosDBKMART _assignUserMenuCloudPosDBKMART;
    private readonly UpdateDatabaseNavCloudPosDBKMART _updateDatabaseNavCloudPosDBKMART1;
    public CloudPosDBKMARTController(CreateNavCloudPosDBKMART createNavCloudPosDBKMART,
        GetParentNavCloudPosDBKMART getParentNavCloudPosDBKMART,
        GetNavCloudPosDBKMART getNavCloudPosDBKMART,
        UpdateNavCloudPosDBKMART updateNavCloudPosDBKMART,
        UpdateDatabaseNavCloudPosDBKMART updateDatabaseNavCloudPosDBKMART,
        RoleCreateCloudPosDBKMART roleCreateCloudPosDBKMART,
        GetRoleCloudPosDBKMART getRoleCloudPosDBKMART,
        GetMenuIdToTheRoleCloudPosDBKMART getMenuIdToTheRoleCloudPosDBKMART,
        GetUserCloudPosDBKMART getUserCloudPosDBKMART,
        GetRoleDDCloudPosDBKMART getRoleDDCloudPosDBKMART,
        UpdateMenuIdToTheRoleCloudPosDBKMART updateMenuIdToTheRoleCloudPosDBKMART,
        AssignUserMenuCloudPosDBKMART assignUserMenuCloudPosDBKMART,
        UpdateDatabaseNavCloudPosDBKMART updateDatabaseNavCloudPosDBKMART1)
    {
        _createNavCloudPosDBKMART = createNavCloudPosDBKMART;
        _getParentNavCloudPosDBKMART = getParentNavCloudPosDBKMART;
        _getNavCloudPosDBKMART = getNavCloudPosDBKMART;
        _updateNavCloudPosDBKMART = updateNavCloudPosDBKMART;
        _updateDatabaseNavCloudPosDBKMART = updateDatabaseNavCloudPosDBKMART;
        _roleCreateCloudPosDBKMART = roleCreateCloudPosDBKMART;
        _getRoleCloudPosDBKMART = getRoleCloudPosDBKMART;
        _getMenuIdToTheRoleCloudPosDBKMART = getMenuIdToTheRoleCloudPosDBKMART;
        _updateMenuIdToTheRoleCloudPosDBKMART = updateMenuIdToTheRoleCloudPosDBKMART;
        _getUserCloudPosDBKMART = getUserCloudPosDBKMART;
        _getRoleDDCloudPosDBKMART = getRoleDDCloudPosDBKMART;
        _assignUserMenuCloudPosDBKMART = assignUserMenuCloudPosDBKMART;
        _updateDatabaseNavCloudPosDBKMART1 = updateDatabaseNavCloudPosDBKMART1;
       
    }

    [HttpPost]
    public async Task<IActionResult> CreateNavCloudPosDBKMART([FromForm] CreateNavCloudPosDBKMARTDto dto)
    {
        var result = await _createNavCloudPosDBKMART.InsertAsync(dto);
        if (result > 0)
            return Ok(new { success = true, message = "Data inserted successfully" });

        return BadRequest(new { success = false, message = "Duplicate SERIAL" });
    }
    [HttpPost]
    public async Task<IActionResult> UpdateDatabaseNavCloudPosDBKMART([FromBody] List<CreateNavInputDto> dto)
    {
        var result = await _updateDatabaseNavCloudPosDBKMART1.UpdateNavAsync(dto);

        if (result.Succeeded)
        {
            return Ok(new { success = true, message = result.Messages.FirstOrDefault() });
        }

        return BadRequest(new { success = false, message = result.Messages.FirstOrDefault() });
    }
    [HttpPost]
    public async Task<IActionResult> UpdateNavCloudPosDBKMART([FromForm] CreateNavCloudPosDBKMARTDto dto)
    {
        var result = await _updateNavCloudPosDBKMART.UpdateAsync(dto);
        if (result > 0)
            return Ok(new { success = true, message = "Data update successfully" });

        return BadRequest(new { success = false, message = "Failed to update data" });
    }

    [HttpPost]
    public async Task<IActionResult> GetParentNavCloudPosDBKMART()
    {
        var result = await _getParentNavCloudPosDBKMART.GetParentsAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetNavCloudPosDBKMART()
    {
        var result = await _getNavCloudPosDBKMART.GetNavsAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetRoleWiseMenuCloudPosDBKMART([FromBody] int ID)
    {
        var result = await _getMenuIdToTheRoleCloudPosDBKMART.GetMenusByRoleAsync(ID);

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> RoleCreateCloudPosDBKMART([FromBody] RoleCreateDto dto)
    {
        if (dto == null)
        {
            return BadRequest(new
            {
                Success = false,
                Message = "Invalid input data."
            });
        }

        var result = await _roleCreateCloudPosDBKMART.CreateRoleAsync(dto);

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
    public async Task<IActionResult> GetRoleCloudPosDBKMART()
    {
        var result = await _getRoleCloudPosDBKMART.GetAllRolesAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateMenuIdToTheRoleCloudPosDBKMART([FromBody] RoleUpdateDto dto)
    {
        var result = await _updateMenuIdToTheRoleCloudPosDBKMART.UpdateMenuIdsForRoleAsync(dto);

        if (result.Succeeded)
            return Ok(new { success = true, message = "Menu IDs updated successfully." });

        return BadRequest(new { success = false, message = "Failed to update menu IDs." });
    }
    [HttpPost]
    public async Task<IActionResult> GetAllUser()
    {
        var result = await _getUserCloudPosDBKMART.GetAllUserAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetAllRole()
    {
        var result = await _getRoleDDCloudPosDBKMART.GetAllRolesAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AssignUserMenu([FromBody] UserMenuDto dto)
    {
        var result = await _assignUserMenuCloudPosDBKMART.AssignUserMenu(dto);

        if (result.Succeeded)
            return Ok(new { success = true, message = "Menu IDs updated successfully." });

        return BadRequest(new { success = false, message = "Failed to update menu IDs." });
    }
}