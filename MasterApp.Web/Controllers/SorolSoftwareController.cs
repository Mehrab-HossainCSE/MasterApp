
using MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;
using MasterApp.Application.Setup.SlaveApp.SorolSoftwate.NavSetting;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using Microsoft.AspNetCore.Mvc;

namespace MasterApp.Web.Controllers;


[ApiController]
[Route("api/[controller]/[action]")]
public class SorolSoftwareController(GetSorolNav _getNav, GetParentNavSorol getParentNavSorol, CreateNavSorol createNavSorol, UpdateSorolNavs updateSorolNavs,
    UpdateSorolSoftDatabaseNav updateSorolSoftDatabaseNav) : ControllerBase
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

}
