FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["API.Domain/API.Domain.csproj", "API.Domain/"]
RUN dotnet restore "API.Domain/API.Domain.csproj"
COPY . .
WORKDIR "/src/API.Domain"
RUN dotnet build "API.Domain.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Domain.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Domain.dll"]