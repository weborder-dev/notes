FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG TARGETARCH
WORKDIR /src
COPY ./backend/src/. .

RUN dotnet restore /src/

COPY ./backend/src ./
WORKDIR /src
RUN dotnet publish /src/ \
    -c Release \
    -o /app/publish \
    -r linux-musl-x64

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
COPY --from=build /app/publish .

ENV EVS_CONNECTION_URL="nats://nats:4222"

EXPOSE 8080

ENTRYPOINT ["./notes-api"]