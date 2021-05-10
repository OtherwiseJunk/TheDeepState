#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG TOKEN

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
ARG TOKEN
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ARG TOKEN
WORKDIR /src/
COPY ["DeepState/DeepState.csproj", "./"]
RUN dotnet restore "DeepState.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TheDeepState.sln" -c Release -o /app/build

FROM build AS publish
ARG TOKEN
RUN dotnet publish "TheDeepState.sln" -c Release -o /app/publish

FROM base AS final
ARG TOKEN
ENV DEEPSTATE=$TOKEN
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeepState.dll"]
