FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY apps-common/Apps.Base.APIGateway/Apps.Base.APIGateway.csproj apps-common/Apps.Base.APIGateway/
RUN dotnet restore apps-common/Apps.Base.APIGateway/Apps.Base.APIGateway.csproj
COPY . .
WORKDIR /src/apps-common/Apps.Base.APIGateway
RUN dotnet build Apps.Base.APIGateway.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Apps.Base.APIGateway.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
#RUN apt-get update && apt-get install -y iputils-ping
ENTRYPOINT ["dotnet", "Apps.Base.APIGateway.dll"]
