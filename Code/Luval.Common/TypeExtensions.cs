using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public static class TypeExtensions
    {
        public static object GetDefaultValue(this Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return default(bool);
                case TypeCode.Byte:
                    return default(byte);
                case TypeCode.Char:
                    return default(char);
                case TypeCode.DBNull:
                    return default(DBNull);
                case TypeCode.DateTime:
                    return default(DateTime);
                case TypeCode.Decimal:
                    return default(decimal);
                case TypeCode.Double:
                    return default(double);
                case TypeCode.Int16:
                    return default(short);
                case TypeCode.Int32:
                    return default(int);
                case TypeCode.Int64:
                    return default(long);                
                case TypeCode.Single:
                    return default(Single);
                case TypeCode.String:
                    return default(string);
                case TypeCode.UInt16:
                    return default(ushort);
                case TypeCode.UInt32:
                    return default(uint);
                case TypeCode.UInt64:
                    return default(ulong);
                default:
                    return Activator.CreateInstance(type);
            }
        }
    }
}
