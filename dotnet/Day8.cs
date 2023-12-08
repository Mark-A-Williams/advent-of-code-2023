namespace AOC2023.Net;

public class Day8
{
    public static int ExecutePart1()
    {
        var lines = File.ReadAllLines("../inputs/8.txt");
        var instructions = lines[0];

        var nodes = lines.Skip(2).Select(ParseInputLine)
            .ToDictionary(node => node.Id, node => node);

        return GetInstructionCountToDestination("AAA", n => n.Id == "ZZZ", nodes, instructions);
    }

    public static long ExecutePart2()
    {
        var lines = File.ReadAllLines("../inputs/8.txt");
        var instructions = lines[0];

        var nodes = lines.Skip(2).Select(ParseInputLine)
            .ToDictionary(node => node.Id, node => node);

        var spookyNodeGhosts = nodes.Where(n => n.Key[2] == 'A').Select(n => n.Value).ToList();

        var instructionsNeededByGhost = spookyNodeGhosts.Select(ghost => GetInstructionCountToDestination(
            ghost.Id,
            node => node.Id[2] == 'Z',
            nodes,
            instructions
        )).ToArray();

        return LeastCommonMultiple(instructionsNeededByGhost);
    }

    private static int GetInstructionCountToDestination(
        string startNodeId,
        Func<Node, bool> hasFinishedPredicate,
        Dictionary<string, Node> nodes,
        string instructions)
    {
        var currentNode = nodes[startNodeId];

        var instructionsFollowed = 0;
        while (!hasFinishedPredicate(currentNode))
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

    private static long LeastCommonMultiple(int[] numbers)
    {
        var foundIt = false;
        var i = 1;
        long result = 0;
        long startNum = numbers.Max();

        do
        {
            long test = startNum * i;

            if (numbers.All(n => test % n == 0))
            {
                result = test;
                foundIt = true;
            }

            i++;
        }
        while (!foundIt);

        return result;
    }

    private static Node ParseInputLine(string line)
        => new(line.Substring(0, 3), line.Substring(7, 3), line.Substring(12, 3));

    private record Node(string Id, string LeftNext, string RightNext);
}
