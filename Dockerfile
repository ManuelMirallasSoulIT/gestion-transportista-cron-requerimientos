FROM ghcr.io/architecture-it/net:6.0-sdk as build
WORKDIR /app
COPY . .
RUN dotnet restore
WORKDIR "/app/src/Api"
RUN dotnet build "qweqwe.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "qweqwe.csproj" -c Release -o /app/publish

FROM ghcr.io/architecture-it/net:6.0
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "qweqwe.dll"]
