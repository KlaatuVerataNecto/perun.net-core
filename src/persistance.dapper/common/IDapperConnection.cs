using System;
using System.Data;

namespace persistance.dapper.common
{
    public interface IDapperConnection : IDisposable
    {
        void Open();
        IDbConnection Connection { get; set; }
    }
}
