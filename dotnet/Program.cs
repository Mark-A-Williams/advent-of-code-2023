﻿using AOC2023.Net;

var start = DateTime.Now;

var result = Day3.ExecutePart1();

var end = DateTime.Now;

Console.WriteLine(result);
Console.WriteLine($"{(end - start).TotalMilliseconds}ms elapsed");
