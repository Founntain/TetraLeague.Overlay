FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TetraLeague.Overlay/TetraLeague.Overlay.csproj", "./"]
COPY ["TetraLeague.Overlay.Network/TetraLeague.Overlay.Network.csproj", "./"]
RUN dotnet restore "TetraLeague.Overlay.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TetraLeague.Overlay/TetraLeague.Overlay.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TetraLeague.Overlay/TetraLeague.Overlay.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -r linux-x64

FROM base AS final

USER root

RUN apt-get update && \
    apt-get install -y libfontconfig1 libfreetype6 fontconfig && \
    apt-get clean

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TetraLeague.Overlay.dll"]