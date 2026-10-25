FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props stylecop.json .editorconfig ./
COPY WebApi/WebApi.csproj WebApi/
COPY WebApp/WebApp.csproj WebApp/
RUN dotnet restore WebApp/WebApp.csproj

COPY WebApi/ WebApi/
COPY WebApp/ WebApp/
RUN dotnet publish WebApp/WebApp.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebApp.dll"]
