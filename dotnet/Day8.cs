namespace AOC2023.Net;

public class Day8
{
    public static int ExecutePart1()
    {
        var lines = File.ReadAllLines("../inputs/8.txt");
        var instructions = lines[0];

        var nodes = lines.Skip(2).Select(ParseInputLine)
            .ToDictionary(node => node.Id, node => node);

        var currentNode = nodes["AAA"];

        var instructionsFollowed = 0;
        while (currentNode.Id != "ZZZ")
        {
            var index = instructionsFollowed % instructions.Length;
            var instruction = instructions[index];

            var nextNode = instruction == 'L'
                ? nodes[currentNode.LeftNext]
                : nodes[currentNode.RightNext];

            instructionsFollowed++;
            currentNode = nextNode;
        }

        return instructionsFollowed;
    }

    public static int ExecutePart2()
    {
        var lines = File.ReadAllLines("../inputs/8.txt");
        var instructions = lines[0];

        var nodes = lines.Skip(2).Select(ParseInputLine)
            .ToDictionary(node => node.Id, node => node);

        var currentNodes = nodes.Where(n => n.Key[2] == 'A').Select(n => n.Value).ToList();
        var instructionsFollowed = 0;

        bool areWeThereYet = false;
        do
        {
            var index = instructionsFollowed % instructions.Length;
            var instruction = instructions[index];

            List<Node> nextNodes = [];
            foreach (var node in currentNodes)
            {
                nextNodes.Add(instruction == 'L' ? nodes[node.LeftNext] : nodes[node.RightNext]);
            }

            var numberOfZ = nextNodes.Count(n => n.Id[2] == 'Z');

            if (numberOfZ > 3)
            {
                Console.WriteLine($"{instructionsFollowed} instructions in, {numberOfZ} simultaneous Zs");
            }

            areWeThereYet = numberOfZ == nextNodes.Count;

            instructionsFollowed++;
            currentNodes = nextNodes;

            if (instructionsFollowed % 10_000_000 == 0) Console.WriteLine($"{instructionsFollowed} instructions in");
        }
        while (!areWeThereYet);

        return instructionsFollowed;
    }

    private static Node ParseInputLine(string line)
        => new(line.Substring(0, 3), line.Substring(7, 3), line.Substring(12, 3));

    private record Node(string Id, string LeftNext, string RightNext);
}
