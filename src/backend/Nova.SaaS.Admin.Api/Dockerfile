#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/backend/Nova.SaaS.Admin.Api/Nova.SaaS.Admin.Api.csproj", "src/backend/Nova.SaaS.Admin.Api/"]
COPY ["src/backend/Nova.Api.Core/Nova.Api.Core.csproj", "src/backend/Nova.Api.Core/"]
RUN dotnet restore "src/backend/Nova.SaaS.Admin.Api/Nova.SaaS.Admin.Api.csproj"
COPY . .
WORKDIR "/src/src/backend/Nova.SaaS.Admin.Api"
RUN dotnet build "Nova.SaaS.Admin.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nova.SaaS.Admin.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nova.SaaS.Admin.Api.dll"]