FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 62884
EXPOSE 44324

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY HasseDiagramService/HasseDiagramService.csproj HasseDiagramService/
RUN dotnet restore HasseDiagramService/HasseDiagramService.csproj
COPY . .
WORKDIR /src/HasseDiagramService
RUN dotnet build HasseDiagramService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish HasseDiagramService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HasseDiagramService.dll"]
