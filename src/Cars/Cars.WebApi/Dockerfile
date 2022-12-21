FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["Cars/Cars.WebApi/Cars.WebApi.csproj", "Cars/Cars.WebApi/"]
RUN dotnet restore "Cars/Cars.WebApi/Cars.WebApi.csproj"
COPY . .
WORKDIR "/src/Cars/Cars.WebApi"
RUN dotnet build "Cars.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cars.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cars.WebApi.dll"]
