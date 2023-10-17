FROM alpine:3.18 AS prepare
WORKDIR /app
RUN adduser -u 1000 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0.100-rc.2-alpine3.18 AS build
RUN apk update && apk upgrade
RUN apk add --no-cache clang build-base zlib-dev
WORKDIR /code
COPY ./WebApp.csproj .
RUN dotnet restore -r linux-musl-x64
COPY . .
RUN dotnet publish \
    -c Release \
    -r linux-musl-x64 \
    -o /app \
    /p:InvariantGlobalization=true \
    --no-restore 

FROM prepare AS final
COPY --chown=appuser --from=build /app/WebApp ./WebApp