# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# AJUSTE: Copia tudo da pasta atual (WishListWeb) para o container
COPY . .

# AJUSTE: Como o Dockerfile já está na pasta do projeto, o comando é direto
RUN dotnet restore "WishListWeb.csproj"
RUN dotnet publish "WishListWeb.csproj" -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WishListWeb.dll"]