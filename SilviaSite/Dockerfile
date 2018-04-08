FROM microsoft/aspnetcore:1.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:1.0 AS build
WORKDIR /src
COPY *.sln ./
COPY ESmartAdminPanel/ESmartAdminPanel.csproj ESmartAdminPanel/
RUN dotnet restore
COPY . .
WORKDIR /src/ESmartAdminPanel
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ESmartAdminPanel.dll"]
