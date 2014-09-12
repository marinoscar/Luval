﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private DbProviderFactory GetFactoryFromProvider(DatabaseProviderType provider)
        {
            return DbProviderFactories.GetFactory(GetDatabaseProviderName(provider));
        }

        private static string GetDatabaseProviderName(DatabaseProviderType providerType)
        {
            var result = "System.Data.SqlClient";
            switch (providerType)
            {
                case DatabaseProviderType.MySql:
                    result = "MySql.Data.MySqlClient";
                    break;
                case DatabaseProviderType.Postgresql:
                    result = "Npgsql";
                    break;
            }
            return result;
        }

        public string ConnectionString { get; set; }

        public IDbConnection GetConnection(DatabaseProviderType providerType)
        {
            var factory = GetFactoryFromProvider(providerType);
            var conn = factory.CreateConnection();
            conn.ConnectionString = ConnectionString;
            return conn;
        }
    }
}
