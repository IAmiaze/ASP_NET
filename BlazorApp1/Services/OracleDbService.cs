using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BlazorApp1.Services
{
    public class OracleDbService
    {
        private readonly IConfiguration _config;

        public OracleDbService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<DataTable> GetDataAsync()
        {
            // Get the connection string from configuration and validate it
            string connStr = _config.GetConnectionString("OracleDb") 
                ?? throw new InvalidOperationException("Oracle connection string 'OracleDb' is missing in configuration.");

            using OracleConnection conn = new OracleConnection(connStr);
            await conn.OpenAsync();

            using OracleCommand cmd = new OracleCommand("SELECT * FROM EMOB.MB_CUSTOMER_MST  WHERE ROWNUM <= 4", conn);
            using OracleDataReader reader = await cmd.ExecuteReaderAsync();

            DataTable dt = new DataTable();
            dt.Load(reader);

            return dt;
        }
    }
}
