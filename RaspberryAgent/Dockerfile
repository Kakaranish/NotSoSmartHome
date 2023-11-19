FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RaspberryAgent/RaspberryAgent.csproj", "RaspberryAgent/"]
RUN dotnet restore "RaspberryAgent/RaspberryAgent.csproj"
COPY . .
WORKDIR "/src/RaspberryAgent"
RUN dotnet build "RaspberryAgent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RaspberryAgent.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaspberryAgent.dll"]
