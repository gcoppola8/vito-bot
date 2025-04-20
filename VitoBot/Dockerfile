FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copia i file pubblicati dalla fase di build
COPY --from=build /out .

# Definisce le variabili d'ambiente
ENV BOT_TOKEN_ENV=""
ENV RG_NAME="Steam"
ENV VM_NAME="minecraft-01"

# Comando di avvio dell'app
ENTRYPOINT ["dotnet", "VitoBot.dll"]