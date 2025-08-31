using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MasterApp.Application.Setup.MasterApp;

public class CreateProject
{
    private readonly IDbConnectionFactory _context;
    private readonly IEncryption _encryption;

    public CreateProject(IDbConnectionFactory context, IEncryption encryption)
    {
        _context = context;
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
                // Validate file
                if (!IsValidImageFile(request.LogoFile))
                {
                    return Result.Fail("Invalid image file. Only jpg, jpeg, png, gif files under 5MB are allowed.");
                }

                // Create uploads directory if it doesn't exist
                string uploadsFolder = Path.Combine(request.WebRootPath, "ProjectLogo");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                string guidPart = Guid.NewGuid().ToString("N").Substring(0, 8);
                uniqueFileName = $"project_{guidPart}{Path.GetExtension(request.LogoFile.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.LogoFile.CopyToAsync(fileStream);
                }
            }

            // Insert into database using Dapper
            var sql = @"
                    INSERT INTO ProjectList (Title, NavigateUrl, LoginUrl, LogoUrl, IsActive, UserName,Password)
                    VALUES (@Title, @NavigateUrl, @LoginUrl, @LogoUrl, @IsActive,@UserName,@Password);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                Title = request.Title,
                NavigateUrl = request.NavigateUrl,
                LoginUrl = request.LoginUrl,
                LogoUrl = uniqueFileName != null ? $"/ProjectLogo/{uniqueFileName}" : null,
                IsActive = request.IsActive,
                Password = string.IsNullOrWhiteSpace(request.Password) || request.Password == "null"
                            ? null
                            : _encryption.Encrypt(request.Password),

               UserName = string.IsNullOrWhiteSpace(request.UserName) || request.UserName == "null"
                            ? null
                            : request.UserName
            };

            using var connection = _context.CreateConnection("MasterAppDB");
            var newProjectId = await connection.ExecuteScalarAsync<int>(sql, parameters);

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


