using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Luval.Common;
using MySql.Data.MySqlClient;

namespace Luval.Orm.MySql
{
    public class MySqlExceptionHandler : IDbExceptionHandler
    {
        public DbException Handle(Exception dataException)
        {
            return Handle(dataException.Message, dataException);
        }

        public DbException Handle(string message, Exception dataException)
        {
            if (!(dataException is MySqlException)) return GetEmptyInstance(dataException);
            var mySqlEx = (MySqlException)dataException;
            if (mySqlEx.Number == 1062)
            {
                return HandleUniqueException(message, mySqlEx);
            }
            return GetEmptyInstance(dataException);
        }

        private DbException HandleUniqueException(string message, MySqlException dataException)
        {
            var entry = string.Empty;
            var key = string.Empty;
            var findEntry = Regex.Match(dataException.Message, "'.*?'");
            if (findEntry.Success)
            {
                entry = findEntry.Value;
                key = dataException.Message.Replace("Duplicate entry {0} for key".Fi(entry), "").Trim();
            }
            return new DbException(message, dataException)
            {
                IsDuplicateKeyViolation = true,
                ErrorNumber = dataException.Number,
                KeyName = key.Replace("'", ""),
                KeyValue = entry.Replace("'", "")
            };
        }


        private DbException GetEmptyInstance(Exception dataException)
        {
            return new DbException(dataException.Message, dataException);
        }
    }
}
