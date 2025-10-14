using MasterApp.Application.Interface;
using MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;
using MasterApp.Application.Setup.SlaveApp.BillingSoftware.RoleManagement;
using MasterApp.Application.Setup.SlaveApp.BillingSoftware.UserManagement;
using MasterApp.Application.SlaveDto;
using Microsoft.AspNetCore.Mvc;

namespace MasterApp.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BillingSoftwareController(
    GetNav _getNavs,
    GetParentNav _getParentNav,
    CreateNav _createNav,
    UpdateNav updateNav,
    UpdateDatabaseNav updateDatabaseNav,
    GetRole getRole,
    CreateRole createRole,
    UpdateRole updateRole,
    GetUser getUser,
    RoleWiseMenu roleWiseMenu,
    UpdateUserRole updateUserRole,
    IBillingSoftUserCreate billingSoftUserCreate,
    GetNavBilling getNavBilling
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetNav()
    {
        var result = await _getNavs.GetNavsJsonAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetNavBilling()
    {
        var result = await getNavBilling.GetNavsAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetParentNav()
    {
        var result = await _getParentNav.GetParentsAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNav([FromForm] BillingSoftNevCreateDto dto)
    {
        var result = await _createNav.InsertAsync(dto);
        if (result > 0)
            return Ok(new { success = true, message = "Data inserted successfully" });

        return BadRequest(new { success = false, message = "Duplicate SERIAL" });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateNav([FromForm] BillingSoftNevCreateDto dto)
    {
        var result = await updateNav.UpdateAsync(dto);
        if (result > 0)
            return Ok(new { success = true, message = "Data update successfully" });

        return BadRequest(new { success = false, message = "Failed to update data" });
    }
    [HttpPost]
    public async Task<IActionResult> UpdateDatabaseNav([FromBody] List<BillingSoftwareDatabaseDto> dto)
    {
        var result = await updateDatabaseNav.UpdateNavAsync(dto);

        if (result.Succeeded)
        {
            return Ok(new { success = true, message = result.Messages.FirstOrDefault() });
        }

        return BadRequest(new { success = false, message = result.Messages.FirstOrDefault() });
    }


    [HttpPost]
    public async Task<IActionResult> GetRoles()
    {
        var result = await getRole.GetAllRolesAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleBilling dto)
    {
        var result = await createRole.InsertRoleAsync(dto);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRole([FromBody] CreateRoleBilling dto)
    {
        var result = await updateRole.UpdateRoleAsync(dto);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAllUser()
    {
        var result = await getUser.GetAllUserAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> RoleWiseMenu([FromQuery] int RoleId)
    {
        var result = await roleWiseMenu.GetAllUserAsync(RoleId);

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateUserRole([FromBody] UpdateRoleBillingDto dto)
    {
        var result = await updateUserRole.UpdateRoleAsync(dto);

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetRoleBilling()
    {
        var result = await billingSoftUserCreate.GetRoleBilling();

        return Ok(result);
    }
}
