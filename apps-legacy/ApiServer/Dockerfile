FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ApiServer/ApiServer.csproj ApiServer/
COPY BambooCore/BambooCore.csproj BambooCore/
COPY BambooCommon/BambooCommon.csproj BambooCommon/
COPY ApiModel/ApiModel.csproj ApiModel/
RUN dotnet restore ApiServer/ApiServer.csproj
COPY . .
# COPY ApiServer/wwwroot/AppBackup/navigations.json wwwroot/AppBackup/
# COPY ApiServer/wwwroot/AppBackup/user-nav-details.json wwwroot/AppBackup/
# COPY ApiServer/wwwroot/AppBackup/user-navs.json wwwroot/AppBackup/
WORKDIR /src/ApiServer
RUN dotnet build ApiServer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ApiServer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiServer.dll"]
