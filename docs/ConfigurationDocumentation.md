

- [Configuration](#configuration)
  - [Triggers](#triggers)
    - [Immediate](#immediate)
    - [Delay](#delay)
  - [Intervals](#intervals)
    - [Periodic](#periodic)
  - [Data Sources](#data-sources)
    - [libplctag](#libplctag)
    - [SineWave](#sinewave)
  - [Data Targets](#data-targets)
    - [CSV](#csv)

# Configuration

## Triggers

### Immediate
The immediate trigger is as simple as it sounds
Example Configuration:
```json
"StartTrigger/EndTrigger": {
    "Type": "Immediate"
  }
```
`Type` - Must be set to "Immediate"

### Delay
The delay trigger waits for the specified interval after Acquisition Set is started
Example Configuration:
```json
  "StartTrigger/EndTrigger": {
    "Type": "Delay",
    "DelaySeconds": 120.0
  }
```
`Type` - Must be set to "Delay".
`DelaySeconds` - Specified in seconds, but resolution is accurate down to milleseconds

## Intervals

### Periodic
A periodic interval happens on a regular basis after the specified timeout. A periodic interval also inserts a "Nominal Time" in Unix epoch style into the data stream.
Example Configuration:
```json
  "Interval": {
    "Type": "Periodic",
    "PeriodSec": 0.5
  }
```
`Type` - Must be set to "Periodic".
`PeriodSec` - Specified in seconds, but resolution is accurate down to milleseconds

## Data Sources
### libplctag
The libplctag source allows reading tag data from various PLCs. See more information here: [libplctag](https://github.com/libplctag/libplctag/wiki/API).
Example Configuration:
```json
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
        }
      ]
    }
```
`Type` - Must be set to "libplctag".
`Gateway` - IP address or hostname to PLC controller.
`Path` - Control plane path for PLC, almost always "1,0".
`Protocol` - `ab_eip` for Allen-Bradley PLCs and `modbus_tcp` for Modbus TCP PLCs.
`PlcType` - Specifies the type of controller from the following list: `controllogix, plc5, slc500, logixpccc, micro800, micrologix, omron-njnx`
`TimeoutSeconds` - The timeout for communications.
`Tags` - An array of tags that should be read
- `Name` - The name or path of the tag.
- `TagType` - **Important** - The type of tag from the following list: `Bool, Bool1D, Bool2D, Bool3D, Dint, Dint1D, Dint2D, Dint3D, Int, Int1D, Int2D, Int3D, Lint, Lint1D, Lint2D, Lint3D, Lreal, Lreal1D, Lreal2D, Lreal3D, Real, Real1D, Real2D, Real3D, Sint, Sint1D, Sint2D, Sint3D, String, String1D, String2D, String3D`

### SineWave
The SineWave target is just a simple calculation of a sine value based on PC time. It's mostly for testing purposes.
Example Configuration:
```json
    {
      "Type": "SineWave",
      "PeriodSec": 1,
      "Alias": "Sine2"
    }
```
`Type` - Must be set to "SineWave".
`PeriodSec` - Period of the sine wave.
`Alias` - Name used to report the sine wave value in the data stream.

## Data Targets

### CSV
This data target outputs the data stream to a CSV file, writing data after every acquisition. It leverages [CSVHelper](https://joshclose.github.io/CsvHelper/).
Example Configuration:
```json
  "DataTarget": {
    "Type": "CSV",
    "FilePath": "DataTarget.csv"
  }  
```
`Type` - Must be set to "CSV".
`FilePath` - Can be relative or absolute.
