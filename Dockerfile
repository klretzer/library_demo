# Base image for final build.
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS app_base
WORKDIR /app

# Restore project dependencies.
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS app_restore
WORKDIR /src
COPY ["LibraryDemo.Api/LibraryDemo.Api.csproj", "LibraryDemo.Api/"]
COPY ["LibraryDemo.Core/LibraryDemo.Core.csproj", "LibraryDemo.Core/"]
COPY ["LibraryDemo.Data/LibraryDemo.Data.csproj", "LibraryDemo.Data/"]
COPY ["LibraryDemo.Models/LibraryDemo.Models.csproj", "LibraryDemo.Models/"]
COPY ["LibraryDemo.Tests/LibraryDemo.Tests.csproj", "LibraryDemo.Tests/"]
RUN dotnet restore "LibraryDemo.Api/LibraryDemo.Api.csproj"

# Restore test project dependencies.
FROM app_restore AS test_restore
WORKDIR /src/LibraryDemo.Tests
RUN dotnet restore

# Build test project.
FROM test_restore AS test_build
WORKDIR /src
COPY . .
RUN dotnet build --no-restore "LibraryDemo.Tests/LibraryDemo.Tests.csproj"

# Run tests.
FROM test_build AS test_run
WORKDIR /src/LibraryDemo.Tests
ENTRYPOINT ["dotnet", "test", "--no-build"]

# Build and publish the app.
FROM app_restore AS app_publish
ARG CONFIG=Debug
WORKDIR /src
COPY . .
RUN dotnet publish "LibraryDemo.Api" -c $CONFIG --no-restore -o /publish /p:UseAppHost=false

# Fetch output from the app_publish stage and run the app.
FROM app_base AS final
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8
RUN apk add --no-cache \
    icu-data-full \
    icu-libs
COPY --from=app_publish /publish .
ENTRYPOINT ["dotnet", "LibraryDemo.Api.dll"]