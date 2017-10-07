using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public static class SqlServerTypeMapping
    {
        public static string FromDoNet(Type type)
        {
            return FromDoNet(type, 1000);
        }

        public static string FromDoNet(Type type, Int16 defaultSize)
        {
            var typeName = type.Name.ToLowerInvariant();
            var result = string.Format("varchar({0})", defaultSize);
            switch (typeName)
            {
                case "int64":
                    result = "bigint";
                    break;
                case "boolean":
                    result = "bit";
                    break;
                case "datetime":
                    result = "datetime";
                    break;
                case "decimal":
                    result = "decimal";
                    break;
                case "double":
                    result = "float";
                    break;
                case "int":
                    result = "int";
                    break;
                case "int32":
                    result = "int";
                    break;
                case "single":
                    result = "real";
                    break;
                case "int16":
                    result = "smallint";
                    break;
                case "byte":
                    result = "tinyint";
                    break;
            }
            return result;
        }
    }
}
