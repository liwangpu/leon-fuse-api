FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["apps-filesystem/Apps.FileSystem.Service/Apps.FileSystem.Service.csproj", "apps-filesystem/Apps.FileSystem.Service/"]
RUN dotnet restore "apps-filesystem/Apps.FileSystem.Service/Apps.FileSystem.Service.csproj"
COPY . .
WORKDIR "/src/apps-filesystem/Apps.FileSystem.Service"
RUN dotnet build "Apps.FileSystem.Service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Apps.FileSystem.Service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Apps.FileSystem.Service.dll"]