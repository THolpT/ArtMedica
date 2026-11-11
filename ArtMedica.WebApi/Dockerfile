
# Общая сборочная среда
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Файлы проектов
COPY ArtMedica.Domain/ArtMedica.Domain.csproj ArtMedica.Domain/
COPY ArtMedica.Application/ArtMedica.Application.csproj ArtMedica.Application/
COPY ArtMedica.WebApi/ArtMedica.WebApi.csproj ArtMedica.WebApi/

# Восстановление зависимостей
RUN dotnet restore ArtMedica.WebApi/ArtMedica.WebApi.csproj

# Исходный код
COPY ArtMedica.Domain/ ArtMedica.Domain/
COPY ArtMedica.Application/ ArtMedica.Application/
COPY ArtMedica.WebApi/ ArtMedica.WebApi/

# Сборка приложения
WORKDIR /src/ArtMedica.WebApi

RUN dotnet publish ArtMedica.WebApi.csproj \
    -c Release \
    -o /app/publish \
    --no-restore


# ==========================================
# Контейнер миграций
# ==========================================
FROM build AS migrator

ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet tool install dotnet-ef --global --version 10.0.0

WORKDIR /src

ENTRYPOINT ["dotnet", "ef"]


# ==========================================
# Контейнер API
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

# FFmpeg
RUN apt-get update && \
    apt-get install -y --no-install-recommends ffmpeg && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "ArtMedica.WebApi.dll"]
