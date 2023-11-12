FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RaspberryApiTest/RaspberryApiTest.csproj", "RaspberryApiTest/"]
RUN dotnet restore "RaspberryApiTest/RaspberryApiTest.csproj"
COPY . .
WORKDIR "/src/RaspberryApiTest"
RUN dotnet build "RaspberryApiTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RaspberryApiTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaspberryApiTest.dll"]
