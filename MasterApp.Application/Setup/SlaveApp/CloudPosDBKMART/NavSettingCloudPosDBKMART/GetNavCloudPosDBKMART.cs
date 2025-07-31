using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class GetNavCloudPosDBKMART
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetNavCloudPosDBKMART(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<NavDto>> GetNavsAsync()
    {
        using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");
        const string sql = @"
            SELECT 
                SERIAL,
                PARENT_ID,
                DESCRIPTION,
                URL,
                PER_ROLE,
                ENTRY_BY,
                ENTRY_DATE,
                ORDER_BY,
                FA_CLASS,
                MENU_TYPE,
                SHOW_EDIT_PERMISSION,
                SERIAL AS ID
            FROM MENU
            ORDER BY ORDER_BY, SERIAL";

        var navItems = (await connection.QueryAsync<NavDto>(sql)).ToList();

        // Clean up any self-referencing items to prevent infinite recursion
        foreach (var item in navItems.Where(n => n.SERIAL == n.PARENT_ID))
        {
            item.PARENT_ID = 0; // Move self-referencing items to root level
        }

        return BuildTree(navItems, 0); // Start with parentId = 0 for root items
    }

    private List<NavDto> BuildTree(List<NavDto> navItems, decimal parentId, HashSet<decimal> visited = null)
    {
        // Initialize visited set on first call to prevent infinite recursion
        if (visited == null)
            visited = new HashSet<decimal>();

        return navItems
            .Where(n => n.PARENT_ID == parentId && !visited.Contains(n.SERIAL))
            .OrderBy(n => n.ORDER_BY)
            .ThenBy(n => n.SERIAL)
            .Select(n =>
            {
                // Add current item to visited set
                var newVisited = new HashSet<decimal>(visited) { n.SERIAL };

                return new NavDto
                {
                    SERIAL = n.SERIAL,
                    PARENT_ID = n.PARENT_ID,
                    DESCRIPTION = n.DESCRIPTION,
                    URL = n.URL,
                    PER_ROLE = n.PER_ROLE,
                    ENTRY_BY = n.ENTRY_BY,
                    ENTRY_DATE = n.ENTRY_DATE,
                    ORDER_BY = n.ORDER_BY,
                    FA_CLASS = n.FA_CLASS,
                    MENU_TYPE = n.MENU_TYPE,
                    SHOW_EDIT_PERMISSION = n.SHOW_EDIT_PERMISSION,
                    ID = n.ID,
                    Children = BuildTree(navItems, n.SERIAL, newVisited)
                };
            })
            .ToList();
    }
}
