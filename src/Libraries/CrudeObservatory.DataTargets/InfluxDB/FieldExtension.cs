using InfluxDB.Client.Writes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataTargets.InfluxDB
{
    internal static class FieldExtension
    {
        internal static PointData.Builder SetFieldByObjectType(this PointData.Builder builder, string name, object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Object:
                    builder.Field(name, JsonConvert.SerializeObject(value));
                    break;
                case TypeCode.Boolean:
                    builder.Field(name, (bool)value);
                    break;
                case TypeCode.Byte:
                    builder.Field(name, (byte)value);
                    break;
                case TypeCode.Char:
                    builder.Field(name, (char)value);
                    break;
                case TypeCode.UInt16:
                    builder.Field(name, (ushort)value);
                    break;
                case TypeCode.UInt32:
                    builder.Field(name, (uint)value);
                    break;
                case TypeCode.UInt64:
                    builder.Field(name, (ulong)value);
                    break;
                case TypeCode.SByte:
                    builder.Field(name, (sbyte)value);
                    break;
                case TypeCode.Int16:
                    builder.Field(name, (short)value);
                    break;
                case TypeCode.Int32:
                    builder.Field(name, (int)value);
                    break;
                case TypeCode.Int64:
                    builder.Field(name, (long)value);
                    break;
                case TypeCode.Single:
                    builder.Field(name, (float)value);
                    break;
                case TypeCode.Double:
                    builder.Field(name, (double)value);
                    break;
                case TypeCode.Decimal:
                    builder.Field(name, (decimal)value);
                    break;
                case TypeCode.DateTime:
                    //HACK: Make the opinionated decision that DateTime = Unix msec
                    var dateTime = (DateTime)value;
                    long unixTimeStampInSeconds = new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
                    builder.Field(name, unixTimeStampInSeconds);
                    break;
                case TypeCode.String:
                    builder.Field(name, (string)value);
                    break;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    //HACK: Write empty string
                    builder.Field(name, "");
                    break;
                default:
                    break;
            }
            return builder;
        }

    }
}
