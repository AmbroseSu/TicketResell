# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TicketResellApplication/TicketResellApplication.csproj", "TicketResellApplication/"]
COPY ["BusinessObject/BusinessObject.csproj", "BusinessObject/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["Service/Service.csproj", "Service/"]
RUN dotnet restore "./TicketResellApplication/TicketResellApplication.csproj"

# Install dotnet-ef tool

# Set PATH for dotnet-ef tool
# Copy the rest of the source files and build the project
COPY . .
WORKDIR "/src/TicketResellApplication"
RUN dotnet build "./TicketResellApplication.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TicketResellApplication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS="http://+:8080"

ENTRYPOINT ["dotnet", "TicketResellApplication.dll"]

