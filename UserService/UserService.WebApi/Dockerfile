# Use the official .NET ASP.NET Core Runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR "/app"
EXPOSE 8080
EXPOSE 8081

# Install OpenSSL for generating certificates
RUN apt-get update && apt-get install -y openssl

# Create a directory to store certificates
RUN mkdir /https

# Generate a self-signed certificate
RUN openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout /https/localhost.key -out /https/localhost.crt -subj "/CN=localhost"

# Create a PFX file for Kestrel
RUN openssl pkcs12 -export -out /https/localhost.pfx -inkey /https/localhost.key -in /https/localhost.crt -passout pass:n5Y5tS6d6F

# Set environment variables for Kestrel
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/localhost.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=n5Y5tS6d6F

# Use the official .NET SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src"

# Copy the project files to the container
COPY ["UserService.WebApi/UserService.WebApi.csproj", "UserService.WebApi/"]
COPY ["UserService.Infrastructure/UserService.Infrastructure.csproj", "UserService.Infrastructure/"]
COPY ["UserService.Application/UserService.Application.csproj", "UserService.Application/"]
COPY ["UserService.Core/UserService.Core.csproj", "UserService.Core/"]


# Restore dependencies for all projects
RUN dotnet restore "./UserService.WebApi/UserService.WebApi.csproj"

# Copy all project files to the container
COPY . .

# Set the working directory to the Web API project
WORKDIR "/src/UserService.WebApi"

# Build the project
RUN dotnet build "UserService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN node --version

# Use the base image to run the project
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "UserService.WebApi.dll"]
