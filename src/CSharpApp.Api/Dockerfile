FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ["CSharpApp.sln", "./"]
COPY ["src/CSharpApp.Api/CSharpApp.Api.csproj", "src/CSharpApp.Api/"]
COPY ["src/CSharpApp.Application/CSharpApp.Application.csproj", "src/CSharpApp.Application/"]
COPY ["src/CSharpApp.Core/CSharpApp.Core.csproj", "src/CSharpApp.Core/"]
COPY ["src/CSharpApp.Infrastructure/CSharpApp.Infrastructure.csproj", "src/CSharpApp.Infrastructure/"]

RUN dotnet restore

COPY . .
RUN dotnet publish "src/CSharpApp.Api/CSharpApp.Api.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /publish
COPY --from=build /publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CSharpApp.Api.dll"]
