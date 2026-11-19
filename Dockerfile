# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_HTTP_PORTS=80
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
ARG USER
WORKDIR /src
COPY ["Unite.Composer/Unite.Composer.csproj", "Unite.Composer/"]
COPY ["Unite.Composer.Download/Unite.Composer.Download.csproj", "Unite.Composer.Download/"]
COPY ["Unite.Composer.Web/Unite.Composer.Web.csproj", "Unite.Composer.Web/"]
RUN --mount=type=secret,id=github_token \
    dotnet nuget add source https://nuget.pkg.github.com/dkfz-unite/index.json -n github -u ${USER} -p "$(cat /run/secrets/github_token)" --store-password-in-clear-text \
    && dotnet restore "Unite.Composer/Unite.Composer.csproj" \
    && dotnet restore "Unite.Composer.Download/Unite.Composer.Download.csproj" \
    && dotnet restore "Unite.Composer.Web/Unite.Composer.Web.csproj" \
    && dotnet nuget remove source github

FROM restore AS build
COPY . .
WORKDIR "/src/Unite.Composer.Web"
RUN dotnet build --no-restore "Unite.Composer.Web.csproj" -c Release

FROM build AS publish
RUN dotnet publish --no-build "Unite.Composer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unite.Composer.Web.dll"]
