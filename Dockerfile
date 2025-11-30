# --- Build aşaması ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Proje dosyalarını kopyala
COPY . .

# Restore ve publish işlemleri
RUN dotnet restore WebAPI/WebAPI.csproj
RUN dotnet publish WebAPI/WebAPI.csproj -c Release -o /app

# --- Runtime aşaması ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Publish edilmiş çıktıyı kopyala
COPY --from=build /app .

# Fly.io 8080 portundan çalışır
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8080

# Uygulama giriş noktası
ENTRYPOINT ["dotnet", "WebAPI.dll"]