#dotnet publish CrudeObservatory -c Release -p:PublishSingleFile=true -p:PublishTrimmed=true -p:TrimMode=link -p:DebuggerSupport=false -p:EnableUnsafeBinaryFormatterSerialization=false -p:EnableUnsafeUTF7Encoding=false -p:InvariantGlobalization=true -r win-x64 --self-contained
dotnet publish './Programs/CrudeObservatory.CLI/' -c Release
