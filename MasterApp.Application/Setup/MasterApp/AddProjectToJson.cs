using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;

namespace MasterApp.Application.Setup.MasterApp;

public class AddProjectToJson
{
    private readonly IEncryption _encryption;

    public AddProjectToJson(IEncryption encryption)
    {
        _encryption = encryption;
    }

    public async Task<IResult> Handle(CreateProjectCommand request)
    {
        try
        {
            // Generate unique filename for logo if provided
            string uniqueFileName = null;
            if (request.LogoFile != null)
            {
                if (!IsValidImageFile(request.LogoFile))
                {
                    return Result.Fail("Invalid image file. Only jpg, jpeg, png, gif files under 5MB are allowed.");
                }

                string uploadsFolder = Path.Combine(request.WebRootPath, "ProjectLogo");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string guidPart = Guid.NewGuid().ToString("N").Substring(0, 8);
                uniqueFileName = $"project_{guidPart}{Path.GetExtension(request.LogoFile.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.LogoFile.CopyToAsync(fileStream);
                }
            }

            // JSON file path
            string jsonFilePath = Path.Combine(request.WebRootPath, "ProjectList", "Project.json");

            // Ensure directory exists
            if (!Directory.Exists(Path.GetDirectoryName(jsonFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath)!);
            }

            // Read existing JSON
            List<ProjectNavDto> projects = new();
            if (File.Exists(jsonFilePath))
            {
                string existingJson = await File.ReadAllTextAsync(jsonFilePath);
                if (!string.IsNullOrWhiteSpace(existingJson))
                {
                    projects = JsonSerializer.Deserialize<List<ProjectNavDto>>(existingJson) ?? new();
                }
            }

            // Generate new Id (max + 1)
            int newId = projects.Any() ? projects.Max(p => p.Id) + 1 : 1;

            // Create new project
            var newProject = new ProjectNavDto
            {
                Id = newId,
                Title = request.Title,
                NavigateUrl = request.NavigateUrl,
                LoginUrl = request.LoginUrl,
                LogoUrl = uniqueFileName != null ? $"/ProjectLogo/{uniqueFileName}" : null,
                IsActive = request.IsActive,
                UserName = string.IsNullOrWhiteSpace(request.UserName) || request.UserName == "null"
                            ? null
                            : request.UserName,
                Password = string.IsNullOrWhiteSpace(request.Password) || request.Password == "null"
                            ? null
                            : _encryption.Encrypt(request.Password)
            };

            // Add to list
            projects.Add(newProject);

            // Write back to JSON
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(projects, jsonOptions);
            await File.WriteAllTextAsync(jsonFilePath, updatedJson);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail($"An error occurred while creating the project: {ex.Message}");
        }
    }

    private bool IsValidImageFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        if (file.Length > 5 * 1024 * 1024)
            return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
            return false;

        var allowedMimeTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
        return allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant());
    }
}

