docker image build -t markwilliamsenable/aoc2023:test4 -f ./dotnet/Dockerfile .
docker image push markwilliamsenable/aoc2023:test4
docker container run --name AoC2023 markwilliamsenable/aoc2023:test4
