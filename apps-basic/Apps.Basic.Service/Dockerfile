FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY apps-basic/Apps.Basic.Service/Apps.Basic.Service.csproj apps-basic/Apps.Basic.Service/
RUN dotnet restore apps-basic/Apps.Basic.Service/Apps.Basic.Service.csproj
COPY . .
WORKDIR /src/apps-basic/Apps.Basic.Service
RUN dotnet build Apps.Basic.Service.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Apps.Basic.Service.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Apps.Basic.Service.dll"]
