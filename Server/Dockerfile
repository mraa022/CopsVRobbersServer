# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 890
EXPOSE 443 

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final-env
WORKDIR /app
COPY --from=build-env /app/out .


RUN mkdir -p /app/certificates
COPY /certificates/aspnetapp.pfx /app/certificates
# Set the entry point
ENTRYPOINT ["dotnet", "Server.dll"]



# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 
# USER app 
# WORKDIR /app 
# EXPOSE 8080
# EXPOSE 8081

# FROM  mcr.microsoft.com/dotnet/aspnet:8.0 AS build 
# ARG BUILD_CONFIGURATION=Release 
# WORKDIR /src 
# COPY ["Server/Server.csproj","Server/"]
# RUN dotnet restore "./Server./Server.csproj"

# COPY . .
# WORKDIR "/src/Server" 
# RUN dotnet build "./Server.csproj" -c $BUILD_CONFIGURATION -o /app/build 


# FROM build AS publish 
# ARG BUILD_CONFIGURATION=Release 
# RUN dotnet publish "./Server.csproj"  -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# FROM base AS final 
# WORKDIR /app 
# COPY --from=publish /app/publish .
# # COPY --from=build-env /app/out .
# ENTRYPOINT [ "dotnet", "Server.dll" ]
