FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Clinic.Api/Clinic.Api.csproj", "Clinic.Api/"]
COPY ["Clinic.Application/Clinic.Application.csproj", "Clinic.Application/"]
COPY ["Clinic.Domain/Clinic.Domain.csproj", "Clinic.Domain/"]
COPY ["Clinic.Infrastructure/Clinic.Infrastructure.csproj", "Clinic.Infrastructure/"]

RUN dotnet restore "Clinic.Api/Clinic.Api.csproj"

COPY . .

WORKDIR "/src/Clinic.Api"

RUN dotnet build "Clinic.Api.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "Clinic.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "Clinic.Api.dll"]