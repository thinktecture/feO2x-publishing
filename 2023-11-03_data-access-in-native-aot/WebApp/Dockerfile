FROM alpine:3.18 AS prepare
WORKDIR /app
RUN adduser -u 1000 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0.100-rc.2-alpine3.18 AS build
RUN apk update && apk upgrade
RUN apk add --no-cache curl clang build-base zlib-dev
WORKDIR /code
COPY ./WebApp.csproj .
ARG RUNTIME_ID=linux-musl-x64
RUN dotnet restore -r $RUNTIME_ID
COPY . .
RUN dotnet publish \
    -c Release \
    -r $RUNTIME_ID \
    -o /app \
    --no-restore 

FROM prepare AS final
COPY --chown=appuser --from=build /app/WebApp ./WebApp
ENTRYPOINT ["./WebApp"]