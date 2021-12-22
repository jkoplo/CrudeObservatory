// See https://aka.ms/new-console-template for more information


using libplctag;
using libplctag.DataTypes.Simple;
using OptimizedLibplctagReads;
using System.Diagnostics;

Console.WriteLine("Hello, World!");
// Instantiate the tag with the appropriate mapper and datatype


var allTags = new List<TagDint>();
var length = 1000;

var stopwatch = new Stopwatch();

stopwatch.Start();

for (int i = 0; i < length; i++)
{
    var myTag = new TagDint()
    {
        //Name is the full path to tag. 
        Name = $"TestDINT{i.ToString("0000")}",
        //Gateway is the IP Address of the PLC or communication module.
        Gateway = "10.10.10.17",
        //Path is the location in the control plane of the CPU. Almost always "1,0".
        Path = "1,0",
        PlcType = PlcType.ControlLogix,
        Protocol = Protocol.ab_eip,
        Timeout = TimeSpan.FromSeconds(5),
    };
    allTags.Add(myTag);
}

stopwatch.Stop();
Console.WriteLine($"Tag instantiation = {stopwatch.ElapsedMilliseconds} msec");

stopwatch.Restart();
await Task.WhenAll(allTags.Select(x => x.InitializeAsync()));
stopwatch.Stop();
Console.WriteLine($"Tag InitAsync = {stopwatch.ElapsedMilliseconds} msec");


ReadExamples.ReadForEach(allTags);
await ReadExamples.ReadForEachAsync(allTags);


for (int i = 0; i < 5; i++)
{
    await ReadExamples.ReadWhenAllAsync(allTags);
}

ReadExamples.ReadParallelForEach(allTags);

ReadExamples.ReadAsyncParallelForEach(allTags);

await ReadExamples.ReadAsyncParallelForEachAsync(allTags);



Console.ReadKey();

//foreach (var item in allTags)
//{
//    Console.WriteLine($"{item.Name} = {item.Value}");
//}


