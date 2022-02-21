# Dockerfile for ETDValidator

# sdk build environment
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .

# testing entrypoint
#ENTRYPOINT [ "dotnet", "ETDValidator.dll"]

# deployment entrypoint
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ETDValidator.dll