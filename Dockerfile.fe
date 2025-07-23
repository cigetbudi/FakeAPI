# --- Stage 1: Build Blazor WASM ---
    FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
    WORKDIR /app
    
    # Copy semua source code
    COPY . .
    
    # Restore dan publish WASM
    RUN dotnet restore src/FakeAPI.BlazorWasm/FakeAPI.BlazorWasm.csproj
    RUN dotnet publish src/FakeAPI.BlazorWasm/FakeAPI.BlazorWasm.csproj -c Release -o /app/publish
    
    # --- Stage 2: Runtime pakai nginx ---
    FROM nginx:alpine AS final
    WORKDIR /usr/share/nginx/html
    
    # Hapus default static files
    RUN rm -rf ./*
    
    # Copy static files hasil publish WASM
    COPY --from=build /app/publish/wwwroot .
    
    # Copy custom nginx config
    COPY nginx.conf /etc/nginx/conf.d/default.conf
    
    # Expose port 80
    EXPOSE 80
    
    # Default command
    CMD ["nginx", "-g", "daemon off;"]
    