using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Tests.Stubs
{
    public class DataReader : IDataReader 
    {
        public void Dispose()
        {
            
        }

        public string GetName(int i)
        {
            return null;
        }

        public string GetDataTypeName(int i)
        {
            return null;
        }

        public Type GetFieldType(int i)
        {
            return GetType();
        }

        public object GetValue(int i)
        {
            return null;
        }

        public int GetValues(object[] values)
        {
            return 0;
        }

        public int GetOrdinal(string name)
        {
            return 0;
        }

        public bool GetBoolean(int i)
        {
            return default(bool);
        }

        public byte GetByte(int i)
        {
            return default(byte);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return default(long);
        }

        public char GetChar(int i)
        {
            return default(char);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return default(long);
        }

        public Guid GetGuid(int i)
        {
            return default(Guid);
        }

        public short GetInt16(int i)
        {
            return default(short);
        }

        public int GetInt32(int i)
        {
            return default(int);
        }

        public long GetInt64(int i)
        {
            return default(long);
        }

        public float GetFloat(int i)
        {
            return default(float);
        }

        public double GetDouble(int i)
        {
            return default(double);
        }

        public string GetString(int i)
        {
            return null;
        }

        public decimal GetDecimal(int i)
        {
            return default(decimal);
        }

        public DateTime GetDateTime(int i)
        {
            return default(DateTime);
        }

        public IDataReader GetData(int i)
        {
            return null;
        }

        public bool IsDBNull(int i)
        {
            return false;
        }

        public int FieldCount { get; private set; }

        object IDataRecord.this[int i]
        {
            get { return null; }
        }

        object IDataRecord.this[string name]
        {
            get { return null; }
        }

        public void Close()
        {
            
        }

        public DataTable GetSchemaTable()
        {
            return null;
        }

        public bool NextResult()
        {
            return false;
        }

        public bool Read()
        {
            return false;
        }

        public int Depth { get; private set; }
        public bool IsClosed { get; private set; }
        public int RecordsAffected { get; private set; }
    }
}
