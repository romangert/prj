FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

COPY ./MongoTest/MongoTest.csproj .
COPY ./NugetPackages ./NugetPackages
RUN dotnet restore --source ./NugetPackages # http://malamdevnugetserver/nuget
COPY . .
RUN dotnet publish ./MongoTest/MongoTest.csproj -c Debug -o /publish

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /publish
EXPOSE 5000
COPY --from=build /publish .
ENTRYPOINT [ "dotnet","MongoTest.dll" ]