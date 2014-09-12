﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Common;

namespace Luval.Orm
{
    public class SqlServerLanguageProvider : AnsiSqlLanguageProvider
    {
        public SqlServerLanguageProvider() : this(DbConfiguration.Get<ISqlExpressionProvider>(), DbConfiguration.Get<IObjectAccesor>())
        {
            
        }

        public SqlServerLanguageProvider(ISqlExpressionProvider expressionProvider, IObjectAccesor objectAccesor) : base(expressionProvider, new SqlServerDialectProvider(), objectAccesor)
        {
        }
    }
}