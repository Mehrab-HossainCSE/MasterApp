using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using System.Text.Json;
using System.Data;
using System.Collections.Concurrent;

namespace MasterApp.Application.Setup.MasterApp
{
    public class TempRoleCreate
    {
        private readonly IDbConnectionFactory _context;
        private readonly IBillingSoftUserCreate _billingSoftUserCreate;
        public TempRoleCreate(IDbConnectionFactory context, IBillingSoftUserCreate billingSoftUserCreate)
        {
            _context = context;
            _billingSoftUserCreate = billingSoftUserCreate;
        }

        public async Task<RoleUpdateResult> UpdateRoleWiseMenuAsync(TemRoleCreateDto dto)
        {
            var result = new RoleUpdateResult();

            try
            {
                // Step 1: Update Master Database (must succeed first)
                var masterDbResult = await UpdateMasterDatabaseAsync(dto);
                if (!masterDbResult.IsSuccess)
                {
                    result.MasterDbStatus = masterDbResult.Message;
                    result.IsSuccess = false;
                    return result;
                }

                result.MasterDbStatus = "Success";
                string roleName = masterDbResult.Data;

                // Step 2: Parse project IDs
                var projectIds = dto.projectId.Split(',')
                    .Select(id => int.Parse(id.Trim()))
                    .ToList();

                // Step 3: Process projects in parallel
                var tasks = projectIds.Select(projectId =>
                    ProcessProjectAsync(projectId, dto, roleName, _billingSoftUserCreate)
                ).ToList();

                var projectResults = await Task.WhenAll(tasks);

                // Step 4: Aggregate results
                result.ProjectResults = projectResults.ToList();
                result.IsSuccess = true;
                result.SuccessCount = projectResults.Count(r => r.IsSuccess);
                result.FailureCount = projectResults.Count(r => !r.IsSuccess);

                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MasterDbStatus = $"Critical error: {ex.Message}";
                return result;
            }
        }

        private async Task<MasterDbResult> UpdateMasterDatabaseAsync(TemRoleCreateDto dto)
        {
            using var connection = _context.CreateConnection("MasterAppDB");
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Update RoleWiseMenu table
                string projectMenusJson = JsonSerializer.Serialize(dto.projectMenus);

                string updateQuery = @"
                    UPDATE [MasterAppDB].[dbo].[RoleWiseMenu]
                    SET MenuIdList = @MenuIdList
                    WHERE Id = @RoleId";

                await connection.ExecuteAsync(updateQuery, new
                {
                    MenuIdList = projectMenusJson,
                    RoleId = dto.roleId
                }, transaction);

                // Get Role Name
                string roleNameQuery = @"
                    SELECT TOP 1 RoleName
                    FROM [MasterAppDB].[dbo].[RoleWiseMenu]
                    WHERE Id = @RoleId";

                string roleName = await connection.QueryFirstOrDefaultAsync<string>(
                    roleNameQuery,
                    new { RoleId = dto.roleId },
                    transaction
                );

                if (string.IsNullOrEmpty(roleName))
                {
                    transaction.Rollback();
                    return new MasterDbResult
                    {
                        IsSuccess = false,
                        Message = "Role not found in RoleWiseMenu table."
                    };
                }

                transaction.Commit();
                return new MasterDbResult
                {
                    IsSuccess = true,
                    Message = "Master database updated successfully",
                    Data = roleName
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new MasterDbResult
                {
                    IsSuccess = false,
                    Message = $"Master DB error: {ex.Message}"
                };
            }
            finally
            {
                connection.Close();
            }
        }

        private async Task<ProjectResult> ProcessProjectAsync(int projectId, TemRoleCreateDto dto, string roleName, IBillingSoftUserCreate billingSoftUserCreate)
        {
            var result = new ProjectResult
            {
                ProjectId = projectId,
                ProjectName = GetProjectName(projectId)
            };

            try
            {
                if (projectId == 1)
                {
                    await ProcessCloudPosProject(dto, roleName);
                    result.IsSuccess = true;
                    result.Message = "Role and permissions created successfully";
                }
                else if (projectId == 4)
                {
                    await ProcessBillingProject(dto, roleName, billingSoftUserCreate);
                    result.IsSuccess = true;
                    result.Message = "Role and permissions created successfully";
                }
                else if (projectId == 6)
                {
                    await ProcessSorolSoftProject(dto, roleName);
                    result.IsSuccess = true;
                    result.Message = "Role and permissions created successfully";
                }
                // Add more project handlers here
                 //else if (projectId == 7)
                 //{
                 //    await ProcessAnotherProject(dto, roleName);
                 //    result.IsSuccess = true;
                 //    result.Message = "Role and permissions created successfully";
                 //}
                else
                {
                    result.IsSuccess = true;
                    result.Message = "No specific processing required for this project";
                }

                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Error: {ex.Message}";
                return result;
            }
        }

        private async Task ProcessBillingProject(TemRoleCreateDto dto, string roleName, IBillingSoftUserCreate billingSoftUserCreate)
        {
            // Step 1: Create/Update Role via API
            var roleDto = new BillingRoleCreateDto
            {

                roleName = roleName,
                description = roleName,
                isActive = true,
                CreatorId = "system",
               
            };

            var saveRoleResult = await _billingSoftUserCreate.SaveRoleAsync(roleDto);

            if (!saveRoleResult.Succeeded)
            {
                throw new Exception($"Failed to save role: {saveRoleResult.Messages}");
            }

            // Step 2: Get menu IDs for billing project (projectId = 4)
            var menusForBilling = dto.projectMenus
                .FirstOrDefault(p => p.projectId == "4")?.menuIds ?? new List<string>();

            // Filter out empty/null menuIds, ensure uniqueness, and convert to int
            var validMenuIds = menusForBilling
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Select(m => m.Trim())
                .Distinct()
                .Select(m => int.TryParse(m, out int menuId) ? menuId : 0)
                .Where(m => m > 0)
                .ToList();

            if (validMenuIds.Any())
            {
                // Step 3: Assign menus to role via API
                var addMenusResult = await _billingSoftUserCreate.AddMenuPagesToRoleAsync(int.Parse(dto.roleId), validMenuIds);

                if (!addMenusResult.Succeeded)
                {
                    throw new Exception($"Failed to assign menus: {addMenusResult.Messages}");
                }
            }
        }

        private async Task ProcessCloudPosProject(TemRoleCreateDto dto, string roleName)
        {
            using var connection = _context.CreateConnection("CloudPosDBKMART");
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Step 1: Create Role (dependent operation must complete first)
                string createRoleQuery = @"
                    IF NOT EXISTS (SELECT 1 FROM [Role_1] WHERE RoleId = @RoleId)
                    BEGIN
                        INSERT INTO [Role_1]
                            ([RoleId], [RoleName], [Description], [IsActive], [CreatorId], [CreateDate], [UpdatorId], [UpdateDate])
                        VALUES (@RoleId, @RoleName, @RoleName, 1, 1, GETDATE(), 1, GETDATE());
                    END
                    ELSE
                    BEGIN
                        UPDATE [Role_1]
                        SET [RoleName] = @RoleName,
                            [Description] = @RoleName,
                            [UpdatorId] = 1,
                            [UpdateDate] = GETDATE()
                        WHERE RoleId = @RoleId;
                    END";

                await connection.ExecuteAsync(createRoleQuery, new
                {
                    RoleId = dto.roleId,
                    RoleName = roleName
                }, transaction);

                // Step 2: Delete old permissions (dependent on role existing)
                string deletePermissionQuery = @"
                    DELETE FROM [Permission_1]
                    WHERE RoleId = @RoleId";

                await connection.ExecuteAsync(deletePermissionQuery, new
                {
                    RoleId = dto.roleId
                }, transaction);

                // Step 3: Insert new permissions (dependent on role existing)
                var menusForProject6 = dto.projectMenus
                    .FirstOrDefault(p => p.projectId == "1")?.menuIds ?? new List<string>();

                // Filter out empty/null menuIds and ensure uniqueness
                var validMenuIds = menusForProject6
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .Select(m => m.Trim())
                    .Distinct()
                    .ToList();

                if (validMenuIds.Any())
                {
                    // Get the current max PermissionId once
                    string getMaxIdQuery = @"
                        SELECT ISNULL(MAX([PermissionId]), 0) 
                        FROM [Permission_1]";

                    int currentMaxId = await connection.ExecuteScalarAsync<int>(getMaxIdQuery, transaction: transaction);
                    int permissionIdCounter = currentMaxId + 1;

                    // Check if MenuId column is INT or VARCHAR
                    foreach (var menuId in validMenuIds)
                    {
                        // Use MERGE to handle duplicates (insert if not exists)
                        string insertPermissionQuery = @"
                            MERGE INTO [Permission_1] AS target
                            USING (SELECT @RoleId AS RoleId, @MenuId AS MenuId) AS source
                            ON (target.RoleId = source.RoleId AND target.MenuId = source.MenuId)
                            WHEN NOT MATCHED THEN
                                INSERT ([PermissionId], [RoleId], [MenuId], [CanView], [CreatorId], [CreateDate])
                                VALUES (@PermissionId, @RoleId, @MenuId, 1, 1, GETDATE());";

                        await connection.ExecuteAsync(insertPermissionQuery, new
                        {
                            PermissionId = permissionIdCounter,
                            RoleId = dto.roleId,
                            MenuId = menuId
                        }, transaction);

                        permissionIdCounter++;
                    }
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private async Task ProcessSorolSoftProject(TemRoleCreateDto dto, string roleName)
        {
            using var connection = _context.CreateConnection("SorolSoftACMasterDB");
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Step 1: Create Role (dependent operation must complete first)
                string createRoleQuery = @"
                    IF NOT EXISTS (SELECT 1 FROM [AC_Master_DB].[dbo].[Role_1] WHERE RoleId = @RoleId)
                    BEGIN
                        INSERT INTO [AC_Master_DB].[dbo].[Role_1]
                            ([RoleId], [RoleName], [Description], [IsActive], [CreatorId], [CreateDate], [UpdatorId], [UpdateDate])
                        VALUES (@RoleId, @RoleName, @RoleName, 1, 1, GETDATE(), 1, GETDATE());
                    END
                    ELSE
                    BEGIN
                        UPDATE [AC_Master_DB].[dbo].[Role_1]
                        SET [RoleName] = @RoleName,
                            [Description] = @RoleName,
                            [UpdatorId] = 1,
                            [UpdateDate] = GETDATE()
                        WHERE RoleId = @RoleId;
                    END";

                await connection.ExecuteAsync(createRoleQuery, new
                {
                    RoleId = dto.roleId,
                    RoleName = roleName
                }, transaction);

                // Step 2: Delete old permissions (dependent on role existing)
                string deletePermissionQuery = @"
                    DELETE FROM [AC_Master_DB].[dbo].[Permission_1]
                    WHERE RoleId = @RoleId";

                await connection.ExecuteAsync(deletePermissionQuery, new
                {
                    RoleId = dto.roleId
                }, transaction);

                // Step 3: Insert new permissions (dependent on role existing)
                var menusForProject6 = dto.projectMenus
                    .FirstOrDefault(p => p.projectId == "6")?.menuIds ?? new List<string>();

                // Filter out empty/null menuIds and ensure uniqueness
                var validMenuIds = menusForProject6
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .Select(m => m.Trim())
                    .Distinct()
                    .ToList();

                if (validMenuIds.Any())
                {
                    // Get the current max PermissionId once
                    string getMaxIdQuery = @"
                        SELECT ISNULL(MAX([PermissionId]), 0) 
                        FROM [AC_Master_DB].[dbo].[Permission_1]";

                    int currentMaxId = await connection.ExecuteScalarAsync<int>(getMaxIdQuery, transaction: transaction);
                    int permissionIdCounter = currentMaxId + 1;

                    // Check if MenuId column is INT or VARCHAR
                    foreach (var menuId in validMenuIds)
                    {
                        // Use MERGE to handle duplicates (insert if not exists)
                        string insertPermissionQuery = @"
                            MERGE INTO [AC_Master_DB].[dbo].[Permission_1] AS target
                            USING (SELECT @RoleId AS RoleId, @MenuId AS MenuId) AS source
                            ON (target.RoleId = source.RoleId AND target.MenuId = source.MenuId)
                            WHEN NOT MATCHED THEN
                                INSERT ([PermissionId], [RoleId], [MenuId], [CanView], [CreatorId], [CreateDate])
                                VALUES (@PermissionId, @RoleId, @MenuId, 1, 1, GETDATE());";

                        await connection.ExecuteAsync(insertPermissionQuery, new
                        {
                            PermissionId = permissionIdCounter,
                            RoleId = dto.roleId,
                            MenuId = menuId
                        }, transaction);

                        permissionIdCounter++;
                    }
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private string GetProjectName(int projectId)
        {
            return projectId switch
            {
                 6 => "SorolSoft AC Master",
                // Add more project names as needed
                 1=> "CloudPos Project",
                 4 => "Billing Project",
                _ => $"Project {projectId}"
            };
        }
    }

    // Result Models
    public class RoleUpdateResult
    {
        public bool IsSuccess { get; set; }
        public string MasterDbStatus { get; set; }
        public List<ProjectResult> ProjectResults { get; set; } = new();
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }

        public string GetSummary()
        {
            if (!IsSuccess)
                return $"Failed: {MasterDbStatus}";

            var successProjects = ProjectResults.Where(p => p.IsSuccess).Select(p => p.ProjectName);
            var failedProjects = ProjectResults.Where(p => !p.IsSuccess).Select(p => $"{p.ProjectName} ({p.Message})");

            var summary = $"Master DB: {MasterDbStatus}. ";

            if (SuccessCount > 0)
                summary += $"Successfully processed {SuccessCount} project(s): {string.Join(", ", successProjects)}. ";

            if (FailureCount > 0)
                summary += $"Failed to process {FailureCount} project(s): {string.Join(", ", failedProjects)}.";

            return summary;
        }
    }

    public class ProjectResult
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class MasterDbResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}