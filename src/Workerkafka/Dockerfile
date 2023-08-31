FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Workerkafka/Workerkafka.csproj", "Workerkafka/"]
RUN dotnet restore "Workerkafka/Workerkafka.csproj"
COPY src/ .
WORKDIR "/src/Workerkafka"
RUN dotnet build "Workerkafka.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Workerkafka.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Workerkafka.dll"]