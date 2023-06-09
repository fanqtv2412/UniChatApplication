#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG http_proxy=http://proxy.fpts.com.vn:8080
ARG https_proxy=http://proxy.fpts.com.vn:8080
WORKDIR /src
COPY ["UniChatApplication.csproj", "."]
RUN dotnet restore "./UniChatApplication.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "UniChatApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniChatApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniChatApplication.dll"]