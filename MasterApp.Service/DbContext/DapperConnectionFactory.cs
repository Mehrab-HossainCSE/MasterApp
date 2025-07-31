using MasterApp.Application.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Service.DbContext
{
    public class DapperConnectionFactory:IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DapperConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection(string connectionName)
        {
            var connectionString = _configuration.GetConnectionString(connectionName);
            return new SqlConnection(connectionString);
        }
    }
}
