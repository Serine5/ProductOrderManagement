# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY *.sln .
COPY ProductOrderManagement/*.csproj ./ProductOrderManagement/

# Restore dependencies
RUN dotnet restore

# Copy all files and publish the app in Release mode
COPY . .
RUN dotnet publish ProductOrderManagement/ProductOrderManagement.csproj -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 
EXPOSE 80
ENTRYPOINT ["dotnet", "ProductOrderManagement.dll"]
