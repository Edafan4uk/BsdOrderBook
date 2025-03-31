# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy project file and restore as distinct layers
COPY src/BsdOrderBook.Host/*.csproj BsdOrderBook.Host/
COPY src/BsdOrderBook.Domain/*.csproj BsdOrderBook.Domain/
COPY src/BsdOrderBook.Application/*.csproj BsdOrderBook.Application/
COPY src/BsdOrderBook.Infrastructure/*.csproj BsdOrderBook.Infrastructure/
RUN dotnet restore BsdOrderBook.Host/BsdOrderBook.Host.csproj

# Copy source code and publish app
COPY src/BsdOrderBook.Host/ BsdOrderBook.Host/
COPY src/BsdOrderBook.Domain/ BsdOrderBook.Domain/
COPY src/BsdOrderBook.Application/ BsdOrderBook.Application/
COPY src/BsdOrderBook.Infrastructure/ BsdOrderBook.Infrastructure/

# publish builds and publishes complexapp
FROM build AS publish
WORKDIR /source/BsdOrderBook.Host
RUN dotnet publish --no-restore -o /app

# Stage 2: Runtime Image
# final is the final runtime stage for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BsdOrderBook.Host.dll"]