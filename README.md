# CrudeObservatory
An application to acquire data from industrial control systems (PLCs and other controllers)

**This project is in early development. It should be considered a technical demo and not relied on for production work loads. A small set of features is currently implemented with more planned in the future. There will be breaking changes as things get developed/refactored until a stable release is made.**

I appreciate community help with testing, submitting issues, and requesting features.

Current assumptions/limitations are documented here: [Limitations & Assumptions](docs/Limitations-Assumptions.md)

## Overview
Crude Observatory is built as a flexible way to acquire data from multiple sources and route that data to storage/endpoints. It's built using separate modules that can be combined via configuration to accomplish different acquisition scenarios. It takes a lot of inspiration from logging frameworks (such as NLog/Serilog) and shipper agents (Elastic's Beats, SumoLogic's Collectors, Influx's Telegraf).

## Design
To build an "Acquisition Set" configuration must be specified for the following:

1. Start Trigger - Triggers the start of acquisition processing.
   1. Ex. `Immediate` - Begin acquiring at program launch.
2. End Trigger - Triggers the end of an acquisition set.
   1. Ex. `Delay` - Acquire data for 120 seconds.
3. Interval - Specifies how often to acquire samples.
   1. Ex. `Periodic` - Every 500 msec, read data.
4. Data Sources - Values to be acquired. You can specify multiple sources/tags/values.
   1. Ex. `libplctag` - Read data from a PLC/Controller.
5. Data Target - The item responsible for storing/transferring data points.
   1. Ex. `CSV` - Store data in the specified CSV file.

### Triggers
Used to start and stop an "Acquisition Set".
* `Immediate`
* `Delay`
* `ValueTrue` [Planned]
* `ValueFalse` [Planned]

### Intervals
Specify when to get data from the data sources.
* `Periodic`
* `RisingEdge` [Planned]
* `FallingEdge` [Planned]

### Data Sources
PLCs/Controllers/etc that provide the values to be read/stored.
* `libplctag`
* `SineWave`

### Data Targets
* `CSV`
* `Json` [Planned]
* `MQTT` [Planned]
* `SQLite` [Planned]

## Example Configuration
Below is an example configuration that acquires data every 500 msec for 2 minutes from several PLC tags and from the purely virtual `SineWave` source.

Documentation for configuration files can be found here: [Configuration Documentation](docs/ConfigurationDocumentation.md)
```json
{
  "Name": "Default Test Config",
  "StartTrigger": {
    "Type": "Immediate"
  },
  "EndTrigger": {
    "Type": "Delay",
    "DelaySeconds": 120.0
  },
  "Interval": {
    "Type": "Periodic",
    "PeriodSec": 0.5
  },
  "DataSources": [
    {
      "Type": "SineWave",
      "PeriodSec": 10,
      "Alias": "Sine1"
    },
    {
      "Type": "SineWave",
      "PeriodSec": 1,
      "Alias": "Sine2"
    },
    {
      "Type": "libplctag",
      "Gateway": "10.10.10.17",
      "Path": "1,0",
      "Protocol": "ab_eip",
      "PlcType": "ControlLogix",
      "TimeoutSeconds": 5.0,
      "Tags": [
        {
          "Name": "TestDINT0000",
          "TagType": "Dint"
        },
        {
          "Name": "TestDINT0001",
          "TagType": "Dint"
        },
        {
          "Name": "TestDINT0002",
          "TagType": "Dint"
        },
        {
          "Name": "TestDINT0003",
          "TagType": "Dint"
        }
      ]
    }
  ],
  "DataTarget": {
    "Type": "CSV",
    "FilePath": "DataTarget.csv"
  }
}
```

## Installation/Operation
Currently, the only available user interface is a CLI app. The application is built in dotnet 6 and requires the host system to have the [ASP.NET Core Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed. Packages are built for both Windows and Linux.

### Installation
Download the latest release and uncompress it to the folder of your choice.

### Operation
1. Modify or copy the included "AcqConfig.json"
2. Run the following:
   1. Windows: ```.\CrudeObservatory.CLI.exe --AcqConfigPath .\AcqConfig.json```
   2. Linux: ```.\CrudeObservatory.CLI --AcqConfigPath .\AcqConfig.json```
3. Logging and CLI output can be adjusted in `appsettings.json`