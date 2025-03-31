# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy the .csproj file and restore dependencies
COPY BsdOrderBook.Host.csproj ./
RUN dotnet restore

# Copy the rest of the project files
COPY . . 
RUN dotnet publish -c Release -o /app --no-restore

# Stage 2: Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "BsdOrderBook.Host.dll"]
