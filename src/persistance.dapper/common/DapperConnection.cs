using MySql.Data.MySqlClient;
using StackExchange.Profiling;
using System;
using System.Data;

namespace persistance.dapper.common
{
    public sealed class DapperConnection : IDapperConnection, IDisposable
    {
        private bool disposed;
        public DapperConnection(string connectionString)
        {
            Connection = new StackExchange.Profiling.Data.ProfiledDbConnection(new MySqlConnection(connectionString), MiniProfiler.Current);
        }

        // TODO: Encapsulate it when DapperExtension is available for .Net Core 2.0
        public IDbConnection Connection { get; set; }

        public void Open()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }
        
        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {

                if (this.Connection.State != ConnectionState.Closed)
                {
                    this.Connection.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
