using MasterApp.Application.MasterAppDto;
using MasterApp.Application.Setup.MasterApp;
using MasterApp.Web.MasterDto;
using Microsoft.AspNetCore.Mvc;

namespace MasterApp.Web.Controllers;


[ApiController]
[Route("api/[controller]/[action]")]
public class ProjectListController : ControllerBase
{
    private readonly GetProjectList _getProjectList;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly CreateProject _createProjectHandler;
    public ProjectListController(CreateProject createProjectHandler,
        IWebHostEnvironment webHostEnvironment, GetProjectList getProjectList)
    {
        _createProjectHandler = createProjectHandler;
        _webHostEnvironment = webHostEnvironment;
        _getProjectList = getProjectList;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromForm] CreateProjectDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // Map DTO to Command
            var command = new CreateProjectCommand
            {
                Title = dto.Title,
                ApiUrl = dto.ApiUrl,
                LoginUrl = dto.LoginUrl,
                LogoFile = dto.LogoFile,
                WebRootPath = _webHostEnvironment.WebRootPath,
                IsActive = dto.IsActive
            };

            // Execute command
            var result = await _createProjectHandler.Handle(command);

            if (result.Succeeded)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false });
            }
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, new { success = false, Message = "An unexpected error occurred." });
        }
    }
    [HttpGet]
    public async Task<IActionResult> getProject()
    {
        var result = await _getProjectList.HandleAsync();

        if (result.HasError)
        {
            return BadRequest(result.Messages);
        }

        return Ok(result);
    }
}