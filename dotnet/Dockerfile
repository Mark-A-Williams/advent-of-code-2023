FROM mcr.microsoft.com/dotnet/sdk:8.0

RUN mkdir -p /app

COPY ./dotnet /app
COPY ./inputs /inputs

WORKDIR /app

RUN dotnet build

ENTRYPOINT [ "dotnet", "run" ]
