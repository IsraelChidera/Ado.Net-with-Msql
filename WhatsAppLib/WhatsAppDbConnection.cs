using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace WhatsAppLib
{
    public class WhatsAppDbConnection: IDisposable
    {
        private readonly string _connectionString;
        private bool _disposed;
        private SqlConnection _dbConnection = null;

        public WhatsAppDbConnection():this(@"Data Source=ISRAEL-CHIDERA\SQLEXPRESS01;Initial Catalog=WhatsAppDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {

        }

        public WhatsAppDbConnection(string connectionString)
        {
            _connectionString = connectionString;            
        }

        public async Task<SqlConnection> OpenConnection()
        {
            _dbConnection = new SqlConnection(_connectionString);
            await _dbConnection.OpenAsync();
            return _dbConnection;
        }

        public async Task CloseConnection()
        {
            if(_dbConnection?.State != ConnectionState.Closed )
            {
                await _dbConnection?.CloseAsync();
            }
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbConnection?.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
