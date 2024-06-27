FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Rindo.API/Rindo.API.csproj", "Rindo.API/"]
COPY ["Rindo.Application/Rindo.Application.csproj", "Rindo.Application/"]
COPY ["Rindo.Domain/Rindo.Domain.csproj", "Rindo.Domain/"]
COPY ["Rindo.Chat/Rindo.Chat.csproj", "Rindo.Chat/"]
COPY ["Rindo.Infrastructure/Rindo.Infrastructure.csproj", "Rindo.Infrastructure/"]
RUN dotnet restore "Rindo.API/Rindo.API.csproj"
COPY . .
WORKDIR "/src/Rindo.API"
RUN dotnet build "Rindo.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Rindo.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rindo.API.dll"]
