using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm.SqlServer
{
    public class SqlServerExceptionHandler : IDbExceptionHandler
    {
        public DbException Handle(Exception dataException)
        {
            return Handle(dataException.Message, dataException);
        }

        public DbException Handle(string message, Exception dataException)
        {
            var sqlEx = ((SqlException) dataException);
            switch (sqlEx.Number)
            {
                case 547:
                    return HandleForeignKey(sqlEx);
                case 2601:
                    return HandleUniqueException(sqlEx);
                case 2627:
                    return HandleUniqueException(sqlEx);
            }
            return new DbException(sqlEx.Message, sqlEx)
                {
                    ErrorNumber = sqlEx.Number, Source = sqlEx.Source
                };
        }

        private DbException HandleUniqueException(SqlException exception)
        {
            var result = new DbException(exception.Message, exception)
                {
                    IsDuplicateKeyViolation = true
                };
            return result;
        }

        private DbException HandleForeignKey(SqlException exception)
        {
            var result = new DbException(exception.Message, exception)
            {
                IsForeignKeyViolation = true
            };
            return result;
        }
    }
}
