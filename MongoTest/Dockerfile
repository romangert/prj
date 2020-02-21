ARG VERSION=3.1
# ARG VERSION=3.1-alpine3.10

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
ENV HTTP_PROXY "http://10.20.14.35:8080" \
	HTTPS_PROXY "http://10.20.14.35:8080"
#RUN apt-get update && apt-get install bridge-utils 
RUN adduser \
  --disabled-password \
  --home /app \
  --gecos '' app \
  && chown -R app /app
USER app
ENV DOTNET_RUNNING_IN_CONTAINER=true \
	ASPNETCORE_URLS=http://+:5000 \
	ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ./MongoTest/MongoTest.csproj ./MongoTest/
COPY ./NuGet.Config .
COPY ./NugetPackages ./NugetPackages
RUN dotnet restore ./MongoTest/MongoTest.csproj
#--source ./NugetPackages 
# http://malamdevnugetserver/nuget
COPY . .
#FROM build as publish
RUN dotnet publish ./MongoTest/MongoTest.csproj -c Debug -o /app

FROM base AS runtime
COPY --from=build /app .
ENTRYPOINT [ "dotnet","MongoTest.dll" ]

#docker run --env ASPNETCORE_ENVIRONMENT=Development --env ASPNETCORE_URLS=http://+:5000 -p 5000:5000 -t --rm -it mongo_app
#docker build --build-arg HTTPS_PROXY=http://10.20.14.35:8080 -t test1 .