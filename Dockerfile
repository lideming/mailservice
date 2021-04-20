#==> Build the publish files
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out -r alpine-x64 --no-self-contained

#==> Build the actual app container
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app

# Copy the published app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "mailservice.dll"]
