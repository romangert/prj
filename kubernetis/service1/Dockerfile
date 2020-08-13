ARG VERSION=2.2
ARG	PORT_APP=80
ARG	PROXY_APP=http://mcafeeproxy01.ntdomain.educ.gov.il:8080

FROM mcr.microsoft.com/dotnet/core/aspnet:$VERSION AS base
ENV HTTP_PROXY=http://mcafeeproxy01.ntdomain.educ.gov.il:8080 \
	HTTPS_PROXY=http://mcafeeproxy01.ntdomain.educ.gov.il:8080 \
	NO_PROXY=dskmalamj025 \
	ASPNETCORE_URLS=http://+:$PORT_APP
WORKDIR /app
EXPOSE $PORT_APP

FROM mcr.microsoft.com/dotnet/core/sdk:$VERSION AS build
ENV HTTP_PROXY=http://devproxy.ntdomain.educ.gov.il:8080 \
	HTTPS_PROXY=http://devproxy.ntdomain.educ.gov.il:8080 \
	NO_PROXY=dskmalamj025
WORKDIR /src
COPY ["service1.csproj", "."]
RUN dotnet restore "service1.csproj"
COPY . .
RUN dotnet build "service1.csproj"

FROM build AS publish
RUN dotnet publish "service1.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "service1.dll"]
