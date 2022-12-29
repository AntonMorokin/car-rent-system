FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["Rides/Rides.WebApi/Rides.WebApi.csproj", "Rides/Rides.WebApi/"]
RUN dotnet restore "Rides/Rides.WebApi/Rides.WebApi.csproj"
COPY . .
WORKDIR "/src/Rides/Rides.WebApi"
RUN dotnet build "Rides.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rides.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rides.WebApi.dll"]
