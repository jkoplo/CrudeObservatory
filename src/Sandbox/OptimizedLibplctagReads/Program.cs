// See https://aka.ms/new-console-template for more information


using libplctag;
using libplctag.DataTypes.Simple;
using OptimizedLibplctagReads;
using System.Diagnostics;

Console.WriteLine("Tag Benchmark");
Console.WriteLine("1) Compare Reads");
Console.WriteLine("2) Read in loop");

// Instantiate the tag with the appropriate mapper and datatype

char choice = Console.ReadKey().KeyChar;
Console.WriteLine();

switch (choice)
{
    case '1':
        var tagsReadCompare = await TagInstantation.Instantiate(2000, TimeSpan.FromSeconds(5));
        ReadExamples.ReadForEach(tagsReadCompare);
        await ReadExamples.ReadForEachAsync(tagsReadCompare);
        await ReadExamples.ReadWhenAllAsync(tagsReadCompare);
        ReadExamples.ReadParallelForEach(tagsReadCompare);
        //ReadExamples.ReadAsyncParallelForEach(tags);
        await ReadExamples.ReadAsyncParallelForEachAsync(tagsReadCompare);
        break;

    case '2':
        var tagsReadLoop = await TagInstantation.Instantiate(2000, TimeSpan.FromMilliseconds(2000));
        var exceptionStopwatch = new Stopwatch();
        try
        {
            while (true)
            {
                exceptionStopwatch.Restart();
                await ReadExamples.ReadWhenAllAsync(tagsReadLoop);
            }
        }
        catch (Exception e)
        {
            exceptionStopwatch.Stop();
            Console.WriteLine($"Exception: {e}");
            Console.WriteLine($"Time Since start: {exceptionStopwatch.ElapsedMilliseconds}");
        }

        break;
    default:
        break;
}









Console.ReadKey();

//foreach (var item in tags)
//{
//    Console.WriteLine($"{item.Name} = {item.Value}");
//}


