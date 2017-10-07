using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Common;
using Luval.Reflection;

namespace Luval.Orm
{
    public class SqlServerLanguageProvider : AnsiSqlLanguageProvider
    {

        private SqlLanguageProviderHelper _helper;

        #region Constructors

        public SqlServerLanguageProvider() : this(DbConfiguration.Get<ISqlExpressionProvider>(), DbConfiguration.Get<IObjectAccesor>())
        {

        }

        public SqlServerLanguageProvider(ISqlExpressionProvider expressionProvider, IObjectAccesor objectAccesor) : base(expressionProvider, new SqlServerDialectProvider(), objectAccesor)
        {
            _helper = new SqlLanguageProviderHelper(objectAccesor, DialectProvider);
        } 

        #endregion

        public override string Upsert<T>(IEnumerable<T> items)
        {
            if (items == null || !items.Any()) return string.Empty;
            var itemType = items.First().GetType();
            var tableDef = _helper.GetTableDefinition(itemType);
            var matchCriteriaArray = tableDef.GetUniqueKeys().Select(i => string.Format("S.{0} = T.{0}", i.ColumnName)).ToList();
            var updateStatements = tableDef.GetNonPrimaryKeyColumns().Select(i => string.Format("T.{0} = S.{0}", i.ColumnName)).ToArray();
            var sb = new StringBuilder();
            sb.AppendLine(GetTableVariable(tableDef));
            foreach(var item in items)
            {
                var values = tableDef.Columns.Select(i => _helper.GetColumnSqlValue(item, i)).ToList();
                sb.AppendFormat("INSERT INTO @TV_{0} VALUES ({1})\n", tableDef.TableName, string.Join(",", values));
            }
            sb.AppendLine();
            sb.AppendFormat("MERGE {0} T\nUSING @TV_{0} S\n", _helper.GetQualifiedTableName(tableDef), tableDef.TableName);
            sb.AppendFormat("ON ({0})\n", string.Join(",", matchCriteriaArray));
            sb.AppendFormat("WHEN MATCHED\n\tTHEN UPDATE\n\tSET\n");
            sb.AppendFormat("\t\t\t{0}\n", string.Join(",", updateStatements));
            sb.AppendFormat("WHEN NOT MATCHED BY TARGET\nTHEN INSERT\n");
            sb.AppendFormat("\t({0})\n", string.Join(",", tableDef.GetNonPrimaryKeyColumns().Select(i => i.ColumnName)));
            sb.AppendFormat("\tVALUES\n\t({0})\n", string.Join(",", tableDef.GetNonPrimaryKeyColumns().Select(i => string.Format("S.{0}", i.ColumnName))));
            sb.AppendLine(";");
            return null;
        }


        private string GetTableVariable(TableDefinition table)
        {
            var sb = new StringBuilder();
            var colDefs = table.Columns.Select(i => string.Format("[{0}] {1}", i.ColumnName, SqlServerTypeMapping.FromDoNet(i.FieldType))).ToList();
            sb.AppendFormat("DECLARE @TV_{0} TABLE (\n", table.TableName);
            sb.AppendLine(string.Join(", ", colDefs));
            sb.AppendLine(")");
            return sb.ToString();
        }

        public string 

        public override bool IsUpsertSupported { get { return true; } }

        public override string GetLastIdentityInsert()
        {
            return "SELECT SCOPE_IDENTITY()";
        }
    }
}
