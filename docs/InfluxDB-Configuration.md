# Using InfluxDB with Crude Observatory

**Warning**: This program is under heavy development. Please use it, test and file issues, but don't depend on it for production scenarios.

- [Using InfluxDB with Crude Observatory](#using-influxdb-with-crude-observatory)
  - [Download, install, and configure InfluxDB](#download-install-and-configure-influxdb)
  - [Configure an access token](#configure-an-access-token)
  - [Configure a Crude Observatory Acquisition Config](#configure-a-crude-observatory-acquisition-config)
  - [Run Crude Observatory](#run-crude-observatory)
  - [View Data](#view-data)
  - [Advanced](#advanced)

## Download, install, and configure InfluxDB
1. Download InfluxDB OSS: [InfluxDB OSS v2.1](https://docs.influxdata.com/influxdb/v2.1/install/)
   1. Installation instructions vary for different OS platforms
   2. InfluxDB is written in golang and is self-contained. The official instructions are generally to download, extract to a reasonable directory, and run `influxd`.
2. Follow the official instructions to configure InfluxDB (either through WebUI or CLI)
   1. 'Organization' is used to group and allow access for users. It can be named anything that makes sense. 
   2. 'Initial bucket' is used to group measurements/data and has a retention period. It can also be named anything that makes sense. 
   3. For data sourcs, choose 'Configure Later' unless you'd also like to set up some host metrics (CPU/Memory/Influx metrics/etc)


## Configure an access token
1. Navigate to the 'Data' tab and choose the 'API Tokens' page
2. Click 'Generate API Token' then 'Read/Write' - we're creating a token we'll give to Crude Observatory in order to enable it to send data to InfluxDB
   1. 'Description' can be anything you like (`CrudeObservatory`)
   2. Under the 'Write' section, give scoped access to only the bucket you want to store data from Crude Observatory (initial bucket you created in first config?)
3. Once the token is created, click on it to show the token value. It should be a long string similar to: `jz8waw_J2w1fNM8JCvOYctW-WYAZdFTXXdaMxmZsExH4FQMihKZNcZ071X0RzqaczBoq1MOnK9AI7knS45Ybsg==`
   1. You'll need to paste this token into the Crude Observatory acquisition config later

## Configure a Crude Observatory Acquisition Config
1. In most cases it's easy to start with the `AcqConfig.json` that is distributed with the Crude Observatory executable
2. In the section for `DataTargets` configure a InfluxDB target like so:
   
    ```json
    {
        "Type": "InfluxDB",
        "Measurement": "<user choice>",
        "Bucket": "<from install step>",
        "Organization": "<from install step>",
        "Token": "<from configure access token>",
        "Url": "http://localhost:8086"
    }
    ```

   1. Use values from [Download, install, and configure InfluxDB](#download-install-and-configure-influxdb) and [Configure an access token](#configure-an-access-token)
   2. The 'Measurement' value can be any string value. It's a good idea to associate the 'Measurement' to a single acquisition config.
3. Configure the rest of the acquisition file to suit the targets/triggers/intervals.

## Run Crude Observatory
```
.\CrudeObservatory.CLI --AcqConfigPath .\AcqConfig.json
```

## View Data
In the InfluxDB WebUI navigate to 'Explore'. Select your bucket, measurement, and fields that you're interested in and choose graphing options then press 'Submit'. There's lots of ways to format graphs or even build dashboards.

## Advanced

This document shows a simple setup. For more information about using InfluxDB, refer to their official docs.

**Note: On Windows, InfluxDB stores all configuration and data to `C:\Users\<username>\.influxdbv2` folder. Wipe this folder to reset. Back it up if your data is important. Be mindful of the security implications of this folder since it stores all secrets/keys.**

Things that are possible:
1. Setting InfluxDB to run as a service at startup
2. Running InfluxDB on a separate machine with (potentially) several deployed instances of Crude Observatory sending data to it
3. Connecting Grafana, Python, you name it to InfluxDB