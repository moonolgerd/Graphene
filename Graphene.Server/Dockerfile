#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Graphene.Server/Graphene.Server.csproj", "Graphene.Server/"]
RUN dotnet restore "Graphene.Server/Graphene.Server.csproj"
COPY . .
WORKDIR "/src/Graphene.Server"
RUN dotnet build "Graphene.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Graphene.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Graphene.Server.dll"]