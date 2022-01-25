using System;
using System.Linq;
using System.Threading.Tasks;
using InfluxClient;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;


// You can generate an API token from the "API Tokens Tab" in the UI
//const string token = "MCG2SrbuIsLIkxlkFUEchvEEtIsolYeB0aqyK7sIuM-f9-mebJJOBYopAKP5DUmGr4YF-qNZlfyto5G-RH6ATw==";
const string token = "Obj8rUUOK2EkJ1y4gp0aKCZhFfANZMXsNbt2SSEN56mB8Foqx9zs3g4yJvJl7A0b3B4uqL_TY4zHufAKusLnYQ==";

const string bucket = "test-bucket";
const string org = "test-org";

using var client = InfluxDBClientFactory.Create("http://localhost:8086", token);

//const string data = "mem,host=host1 used_percent=37.43234543";
//using (var writeApi = client.GetWriteApi())
//{
//    writeApi.WriteRecord(bucket, org, WritePrecision.Ns, data);
//}

var point = PointData
  .Measurement("mem")
  .Tag("host", "host2")
  .Field("used_percent", 38.43234543)
  .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

using (var writeApi = client.GetWriteApi())
{
    writeApi.WritePoint(bucket, org, point);
}

//var mem = new Mem { Host = "host3", UsedPercent = 39.43234543, Time = DateTime.UtcNow };

//using (var writeApi = client.GetWriteApi())
//{
//    writeApi.WriteMeasurement(bucket, org, WritePrecision.Ns, mem);
//}

//var query = "from(bucket: \"bucket01\") |> range(start: -1h)";
//var tables = await client.GetQueryApi().QueryAsync(query, org);

//foreach (var record in tables.SelectMany(table => table.Records))
//{
//    Console.WriteLine($"{record}");
//}
