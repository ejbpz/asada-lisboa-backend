ARG BUILD_CONFIGURATION=Release

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS builder
WORKDIR /src
COPY [ "AsadaLisboaBackend.Utils/", "AsadaLisboaBackend.Utils/" ]
COPY [ "AsadaLisboaBackend.Models/", "AsadaLisboaBackend.Models/" ]
COPY [ "AsadaLisboaBackend.RepositoryContracts/", "AsadaLisboaBackend.RepositoryContracts/" ]
COPY [ "AsadaLisboaBackend.ServiceContracts/", "AsadaLisboaBackend.ServiceContracts/" ]
COPY [ "AsadaLisboaBackend.Repositories/", "AsadaLisboaBackend.Repositories/" ]
COPY [ "AsadaLisboaBackend.Services/", "AsadaLisboaBackend.Services/" ]
COPY [ "AsadaLisboaBackend/", "AsadaLisboaBackend/" ]

WORKDIR /src/AsadaLisboaBackend
RUN dotnet restore "./AsadaLisboaBackend.csproj"
RUN dotnet build "./AsadaLisboaBackend.csproj" -c "$BUILD_CONFIGURATION" -o /src/build
RUN dotnet publish "./AsadaLisboaBackend.csproj" -c "$BUILD_CONFIGURATION" -o /src/publish -r linux-musl-x64

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS production
WORKDIR /src
COPY --from=builder /src/publish .
ENTRYPOINT [ "dotnet", "AsadaLisboaBackend.dll" ]