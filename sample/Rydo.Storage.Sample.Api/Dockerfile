FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sample/Rydo.Storage.Sample.Api/Rydo.Storage.Sample.Api.csproj", "Rydo.Storage.Sample.Api/"]
COPY ["src/Rydo.Storage.DynamoDB/Rydo.Storage.DynamoDB.csproj", "Rydo.Storage.DynamoDB/"]
COPY ["src/Rydo.Storage/Rydo.Storage.csproj", "Rydo.Storage/"]
COPY ["src/Rydo.Storage.MongoDb/Rydo.Storage.MongoDb.csproj", "Rydo.Storage.MongoDb/"]
COPY ["src/Rydo.Storage.Redis/Rydo.Storage.Redis.csproj", "Rydo.Storage.Redis/"]
COPY ["sample/Rydo.Storage.Sample.Core/Rydo.Storage.Sample.Core.csproj", "Rydo.Storage.Sample.Core/"]
RUN dotnet restore "sample/Rydo.Storage.Sample.Api/Rydo.Storage.Sample.Api.csproj"
COPY . .
WORKDIR "/src/Rydo.Storage.Sample.Api"
RUN dotnet build "Rydo.Storage.Sample.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rydo.Storage.Sample.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rydo.Storage.Sample.Api.dll"]
