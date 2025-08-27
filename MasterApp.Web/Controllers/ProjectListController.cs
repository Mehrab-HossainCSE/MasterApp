using MasterApp.Application.Com.Login;
using MasterApp.Application.Common.Models;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.Setup.MasterApp;
using MasterApp.Application.Setup.MasterApp.NavMasterApp;
using MasterApp.Web.MasterDto;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading;

namespace MasterApp.Web.Controllers;


[ApiController]
[Route("api/[controller]/[action]")]
public class ProjectListController : ControllerBase
{
    private readonly GetProjectList _getProjectList;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly CreateProject _createProjectHandler;
    private readonly LoginCommand _loginCommand;
    private readonly UpdateProject _updateProjectList;
    private readonly DeleteProject _deleteProject;
    private readonly GetNavProjectByUser _getNavProjectByUser;
    private readonly GetAllUser _getAllUser;
    private readonly UserCreate _userCreate;
    private readonly UserProjectPermission _userProjectPermission;
    private readonly UpdateUserInfo _updateUserInfo;
    public ProjectListController(CreateProject createProjectHandler,
        IWebHostEnvironment webHostEnvironment, GetProjectList getProjectList,
        LoginCommand loginCommand, UpdateProject updateProjectList, DeleteProject deleteProject, GetNavProjectByUser getNavProjectByUser, 
        GetAllUser getAllUser, UserCreate userCreate, UserProjectPermission userProjectPermission, UpdateUserInfo updateUserInfo)
    {
        _createProjectHandler = createProjectHandler;
        _webHostEnvironment = webHostEnvironment;
        _getProjectList = getProjectList;
        _loginCommand = loginCommand;
        _updateProjectList = updateProjectList;
        _deleteProject = deleteProject;
        _getNavProjectByUser = getNavProjectByUser;
        _getAllUser = getAllUser;
        _userCreate = userCreate;
        _userProjectPermission = userProjectPermission;
        _updateUserInfo = updateUserInfo;
    }
    [HttpGet]
    public async Task<IActionResult> GetNavProjectList([FromQuery] string UserID)
    {
        var result = await _getNavProjectByUser.HandleAsync(UserID);
        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] LoginDto model, CancellationToken cancellationToken)
    {
        var result = await _loginCommand.ExecuteAsync(model, cancellationToken);
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> UserCreate([FromBody] UserCreateDto dto)
    {
        var result = await _userCreate.HandleAsync(dto);
        return Ok(result);
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
                NavigateUrl = dto.NavigateUrl,
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
    [HttpPost]
    public async Task<IActionResult> UpdateProject([FromForm] UpdateProjectDto dto)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // Map DTO to Command
            var command = new UpdateProjectCommand
            {
                Id = dto.Id,
                Title = dto.Title,
                NavigateUrl = dto.NavigateUrl,
                LoginUrl = dto.LoginUrl,
                LogoFile = dto.LogoFile,
                WebRootPath = _webHostEnvironment.WebRootPath,
                IsActive = dto.IsActive
            };

            // Execute command
            var result = await _updateProjectList.HandleUpdate(command);

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

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteProject(int Id)
    {
        var result = await _deleteProject.HandleAsync(Id, _webHostEnvironment.WebRootPath);

        if (result.Succeeded)
        {
            return Ok(new { success = true });
        }
        else
        {
            return BadRequest(new { success = false });
        }
    }

    [HttpGet]
    public async Task<IActionResult> getMasterAppUser()
    {
        var result = await _getAllUser.HandleAsync();

        if (result.HasError)
        {
            return BadRequest(result.Messages);
        }

        return Ok(result);
    }
  
    [HttpPost]
    public async Task<IActionResult> UserProjectUpdate([FromBody] ProjectUpdateDto dto)
    {
        var result = await _userProjectPermission.UpdateUserMenuListAsync(dto);
        if (result>0)
        {
            return Ok(new { succeeded = true });
        }
        else
        {
            return BadRequest(new { succeeded = false });
        }
       
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UserCreateDto dto)
    {
        var result = await _updateUserInfo.UpdateUserAsync(dto);
        if (result > 0)
        {
            return Ok(new { succeeded = true });
        }
        else
        {
            return BadRequest(new { succeeded = false });
        }

    }

}