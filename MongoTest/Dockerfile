ARG VERSION=2.2
# ARG VERSION=3.1-alpine3.10

FROM mcr.microsoft.com/dotnet/core/aspnet:$VERSION AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:$VERSION AS build
WORKDIR /src
COPY ./MongoTest/MongoTest.csproj ./MongoTest/
COPY ./NugetPackages ./NugetPackages
RUN dotnet restore ./MongoTest/MongoTest.csproj --source ./NugetPackages 
# http://malamdevnugetserver/nuget
COPY . .

#FROM build as publish
RUN dotnet publish ./MongoTest/MongoTest.csproj -c Debug -o /app

FROM base AS runtime
RUN adduser \
  --disabled-password \
  --home /app \
  --gecos '' app \
  && chown -R app /app
USER app

WORKDIR /app
ENV DOTNET_RUNNING_IN_CONTAINER=true \
	ASPNETCORE_URLS=http://+:5000 \
	ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5000
COPY --from=build /app .
ENTRYPOINT [ "dotnet","MongoTest.dll" ]

#docker run --env ASPNETCORE_ENVIRONMENT=Development --env ASPNETCORE_URLS=http://+:5000 -p 5000:5000 -t --rm -it mongo_app