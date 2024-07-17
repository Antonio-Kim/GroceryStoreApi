# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore as distinct layers
COPY GroceryStore.sln ./
COPY GroceryStoreApi/GroceryStoreApi.csproj GroceryStoreApi/
RUN dotnet restore GroceryStoreApi/GroceryStoreApi.csproj

# Copy everything else and build
COPY . .
WORKDIR /src/GroceryStoreApi
RUN dotnet build GroceryStoreApi.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish GroceryStoreApi.csproj -c Release -o /app/publish

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for production
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "GroceryStoreApi.dll"]
