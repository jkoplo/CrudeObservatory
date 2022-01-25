using InfluxDB.Client.Writes;
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
        internal static PointData SetFieldByObjectType(this PointData pointData, string name, object value)
        {
            var jobject = JObject.FromObject(value);

            switch (jobject.Type)
            {
                case JTokenType.Integer:
                    pointData.Field(name, (int)value);
                    break;
                case JTokenType.Float:
                    pointData.Field(name, (double)value);
                    break;
                case JTokenType.String:
                    pointData.Field(name, (string)value);
                    break;
                case JTokenType.Boolean:
                    pointData.Field(name, (bool)value);
                    break;

                case JTokenType.Object:
                case JTokenType.Array:
                case JTokenType.Constructor:
                case JTokenType.Property:
                case JTokenType.Comment:
                case JTokenType.Null:
                case JTokenType.Undefined:
                case JTokenType.Date:
                case JTokenType.Raw:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.Bytes:
                case JTokenType.None:
                case JTokenType.TimeSpan:
                    pointData.Field(name, jobject.ToString());
                    break;
            }

            return pointData;
        }

    }
}
