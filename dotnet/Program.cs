using AOC2023.Net;

var start = DateTime.Now;

var result = Day6.ExecutePart2();

var end = DateTime.Now;

Console.WriteLine(result);
Console.WriteLine($"{(end - start).TotalMilliseconds}ms elapsed");
