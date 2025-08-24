using MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;
using MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;
using Microsoft.AspNetCore.Mvc;

namespace MasterApp.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BillingSoftwareController(
    GetNav _getNavs

    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetNav()
    {
        var result = await _getNavs.GetNavsAsync();

        if (result == null || !result.Any())
            return NotFound("No parent menus found.");

        return Ok(result);
    }
}
