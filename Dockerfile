FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS restore
ARG USER
ARG TOKEN
WORKDIR /src
RUN dotnet nuget add source https://nuget.pkg.github.com/dkfz-unite/index.json -n github -u ${USER} -p ${TOKEN} --store-password-in-clear-text
COPY ["Unite.Composer/Unite.Composer.csproj", "Unite.Composer/"]
COPY ["Unite.Composer.Download/Unite.Composer.Download.csproj", "Unite.Composer.Download/"]
COPY ["Unite.Composer.Web/Unite.Composer.Web.csproj", "Unite.Composer.Web/"]
RUN dotnet restore "Unite.Composer/Unite.Composer.csproj"
RUN dotnet restore "Unite.Composer.Download/Unite.Composer.Download.csproj"
RUN dotnet restore "Unite.Composer.Web/Unite.Composer.Web.csproj"

FROM restore as build
COPY . .
WORKDIR "/src/Unite.Composer.Web"
RUN dotnet build --no-restore "Unite.Composer.Web.csproj" -c Release

FROM build AS publish
RUN dotnet publish --no-build "Unite.Composer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unite.Composer.Web.dll"]