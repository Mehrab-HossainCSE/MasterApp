using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;

namespace MasterApp.Application.Setup.MasterApp;

public class UpdateJsonProject(IEncryption _encryption)
{
    public async Task<IResult> HandleUpdate(UpdateProjectCommand request)
    {
        try
        {
            // JSON file path
            string jsonFilePath = Path.Combine(request.WebRootPath, "ProjectList", "Project.json");

            if (!File.Exists(jsonFilePath))
                return Result.Fail("Project storage file not found.");

            // Read existing JSON
            var json = await File.ReadAllTextAsync(jsonFilePath);
            var projects = JsonSerializer.Deserialize<List<ProjectJsonDtos>>(json) ?? new();

            // Find existing project
            var existingProject = projects.FirstOrDefault(p => p.Id == request.Id);
            if (existingProject == null)
                return Result.Fail("Project not found.");

            string uniqueFileName = existingProject.LogoUrl ?? string.Empty; // Keep old logo by default

            // If a new logo is provided
            if (request.LogoFile != null)
            {
                if (!IsValidImageFile(request.LogoFile))
                    return Result.Fail("Invalid image file. Only jpg, jpeg, png, gif files under 5MB are allowed.");

                // Delete old logo if exists
                if (!string.IsNullOrEmpty(existingProject.LogoUrl))
                {
                    string oldFilePath = Path.Combine(request.WebRootPath, existingProject.LogoUrl.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                // Save new file
                string uploadsFolder = Path.Combine(request.WebRootPath, "ProjectLogo");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string guidPart = Guid.NewGuid().ToString("N").Substring(0, 8);
                uniqueFileName = $"/ProjectLogo/project_{guidPart}{Path.GetExtension(request.LogoFile.FileName)}";
                string filePath = Path.Combine(uploadsFolder, Path.GetFileName(uniqueFileName));

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await request.LogoFile.CopyToAsync(fileStream);
            }

            // Handle password encryption
            string? encryptedPassword = existingProject.Password; // keep old by default
            if (!string.IsNullOrEmpty(request.Password))
            {
                if (string.IsNullOrWhiteSpace(existingProject.Password))
                {
                    encryptedPassword = _encryption.Encrypt(request.Password);
                }
                else if (existingProject.Password != request.Password)
                {
                    encryptedPassword = _encryption.Encrypt(request.Password);
                }
            }

            // Update fields
            existingProject.Title = request.Title;
            existingProject.NavigateUrl = request.NavigateUrl;
            existingProject.LoginUrl = request.LoginUrl;
            existingProject.LogoUrl = uniqueFileName;
            existingProject.IsActive = request.IsActive;
            existingProject.UserName = string.IsNullOrEmpty(request.UserName) ? existingProject.UserName : request.UserName;
            existingProject.Password = encryptedPassword;

            // Write back to JSON
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            await File.WriteAllTextAsync(jsonFilePath, JsonSerializer.Serialize(projects, jsonOptions));

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail($"An error occurred while updating the project: {ex.Message}");
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