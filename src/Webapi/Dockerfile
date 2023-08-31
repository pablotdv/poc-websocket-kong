FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Webapi/Webapi.csproj", "Webapi/"]
RUN dotnet restore "Webapi/Webapi.csproj"
COPY src/ .
WORKDIR "/src/Webapi"
RUN dotnet build "Webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Webapi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webapi.dll"]