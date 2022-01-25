using InfluxDB.Client.Core;

namespace InfluxClient
{
    [Measurement("mem")]
    internal class Mem
    {
        [Column("host", IsTag = true)] public string Host { get; set; }
        [Column("used_percent")] public double? UsedPercent { get; set; }
        [Column(IsTimestamp = true)] public DateTime Time { get; set; }
    }
}
