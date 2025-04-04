# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy all files and build
COPY . ./
RUN dotnet publish -c Release -o /out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 80
ENTRYPOINT ["dotnet", "VisitTracker.API.dll"]
