# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /app

# Salin seluruh source code dan solusi
COPY . .

# Restore dan publish
RUN dotnet restore
RUN dotnet publish src/FakeAPI.Api/FakeAPI.Api.csproj -c Release -o /app/publish

# Stage 2: Runtime pakai Alpine
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview-alpine AS runtime
WORKDIR /app

# Salin hasil publish dari build
COPY --from=build /app/publish .

# Buka port
EXPOSE 5121
# EXPOSE 7008

# Jalankan aplikasi
ENTRYPOINT ["dotnet", "FakeAPI.Api.dll"]
