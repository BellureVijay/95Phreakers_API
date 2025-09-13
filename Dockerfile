FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app
COPY . .

RUN dotnet restore 95Phreakers_WebAPI.sln
RUN dotnet build 95Phreakers_WebAPI.sln -c Release -o /app/build
RUN dotnet publish 95PhrEAKer-webapi/95PhrEAKer.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "95PhrEAKer.API.dll"]
