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
    public CloudPosDBKMARTController(CreateNavCloudPosDBKMART createNavCloudPosDBKMART, GetParentNavCloudPosDBKMART getParentNavCloudPosDBKMART, GetNavCloudPosDBKMART getNavCloudPosDBKMART, UpdateNavCloudPosDBKMART updateNavCloudPosDBKMART, UpdateDatabaseNavCloudPosDBKMART updateDatabaseNavCloudPosDBKMART)
    {
        _createNavCloudPosDBKMART = createNavCloudPosDBKMART;
        _getParentNavCloudPosDBKMART = getParentNavCloudPosDBKMART;
        _getNavCloudPosDBKMART = getNavCloudPosDBKMART;
        _updateNavCloudPosDBKMART = updateNavCloudPosDBKMART;
        _updateDatabaseNavCloudPosDBKMART = updateDatabaseNavCloudPosDBKMART;
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
    public async Task<IActionResult> UpdateDatabaseNavCloudPosDBKMART([FromBody] List<CreateNavInputDto> dto)
    {
        var result = await _updateDatabaseNavCloudPosDBKMART.UpdateNavAsync(dto);

        return result.Succeeded
      ? Ok(result)
      : BadRequest(result);
    }


}