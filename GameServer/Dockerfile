FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GameServer/GameServer.csproj", "./GameServer/"]
COPY ["Shared/Shared.csproj", "./Shared/"]
RUN dotnet restore GameServer/GameServer.csproj

COPY GameServer/ ./GameServer/
COPY Shared/ ./Shared/
RUN dotnet build ./GameServer/GameServer.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish ./GameServer/GameServer.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "GameServer.dll"]
