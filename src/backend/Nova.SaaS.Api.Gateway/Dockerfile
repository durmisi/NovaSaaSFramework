#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/backend/Nova.SaaS.Api.Gateway/Nova.SaaS.Api.Gateway.csproj", "src/backend/Nova.SaaS.Api.Gateway/"]
RUN dotnet restore "src/backend/Nova.SaaS.Api.Gateway/Nova.SaaS.Api.Gateway.csproj"
COPY . .
WORKDIR "/src/src/backend/Nova.SaaS.Api.Gateway"
RUN dotnet build "Nova.SaaS.Api.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nova.SaaS.Api.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nova.SaaS.Api.Gateway.dll"]