using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using Microsoft.AspNetCore.Http;
using System.Linq;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace MasterApp.Application.Setup.MasterApp;

public class UpdateProject(IDbConnectionFactory _context, IEncryption _encryption)
{
    public async Task<IResult> HandleUpdate(UpdateProjectCommand request)
    {
        try
        {
            using var connection = _context.CreateConnection("MasterAppDB");

            // First fetch existing project
            var existingProject = await connection.QuerySingleOrDefaultAsync<ProjectDtos>(
                "SELECT * FROM ProjectList WHERE Id = @Id",
                new { request.Id });

            if (existingProject == null)
                return Result.Fail("Project not found.");

            string uniqueFileName = existingProject.LogoUrl; // Keep old logo by default

            // If a new logo is provided
            if (request.LogoFile != null)
            {
                // Validate file
                if (!IsValidImageFile(request.LogoFile))
                    return Result.Fail("Invalid image file. Only jpg, jpeg, png, gif files under 5MB are allowed.");

                // Delete old logo if it exists
                if (!string.IsNullOrEmpty(existingProject.LogoUrl))
                {
                    string oldFilePath = Path.Combine(request.WebRootPath, existingProject.LogoUrl.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                // Create uploads folder if not exists
                string uploadsFolder = Path.Combine(request.WebRootPath, "ProjectLogo");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Save new file
                string guidPart = Guid.NewGuid().ToString("N").Substring(0, 8);
                uniqueFileName = $"/ProjectLogo/project_{guidPart}{Path.GetExtension(request.LogoFile.FileName)}";
                string filePath = Path.Combine(uploadsFolder, Path.GetFileName(uniqueFileName));

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await request.LogoFile.CopyToAsync(fileStream);
            }

            // Update database
            var sql = @"
            UPDATE ProjectList
            SET Title = @Title,
                NavigateUrl = @NavigateUrl,
                LoginUrl = @LoginUrl,
                LogoUrl = @LogoUrl,
                IsActive = @IsActive,
                UserName = ISNULL(@UserName, UserName),
                Password = ISNULL(@Password, Password)
            WHERE Id = @Id";

            string? encryptedPassword = null;
            if (!string.IsNullOrEmpty(request.Password))
            {
                encryptedPassword = _encryption.Encrypt(request.Password);
            }
        
            var parameters = new
            {
                Title = request.Title,
                NavigateUrl = request.NavigateUrl,
                LoginUrl = request.LoginUrl,
                LogoUrl = uniqueFileName,
                IsActive = request.IsActive,
                Id = request.Id,
                UserName = string.IsNullOrEmpty(request.UserName) ? null : request.UserName,
                Password = encryptedPassword
            };

            await connection.ExecuteAsync(sql, parameters);

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

        // Check file size (max 5MB)
        if (file.Length > 5 * 1024 * 1024)
            return false;

        // Check file extension
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
            return false;

        // Check MIME type
        var allowedMimeTypes = new[]
        {
                "image/jpeg",
                "image/jpg",
                "image/png",
                "image/gif"
            };

        return allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant());
    }

}
