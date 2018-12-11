FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Apps.MoreJee.Service/Apps.MoreJee.Service.csproj Apps.MoreJee.Service/
RUN dotnet restore Apps.MoreJee.Service/Apps.MoreJee.Service.csproj
COPY . .
WORKDIR /src/Apps.MoreJee.Service
RUN dotnet build Apps.MoreJee.Service.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Apps.MoreJee.Service.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Apps.MoreJee.Service.dll"]
