FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#base is the name of the stage
WORKDIR /app
#port to work on 
EXPOSE 8080
EXPOSE 8081
#build runtime image
#multistage
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release
copy *.csproj . 

#this means copy from the project to the working directory
WORKDIR /src
#COPY ["RunGroopWebApp/RunGroopWebApp. csproj", "RunGroopWebApp/"]
#COPY ["RunGroopWebApp/FormulaOne. DataService. csproj", "FormulaOne. DataService/"]
#COPY ["RunGroopWebApp/FormulaOne. Entities. csproj", "FormulaOne. Entities/"]
#restore libraries
#RUN dotnet restore "RunGroopWebApp/RunGroopWebApp.csproj"
#copy everything
COPY . .
WORKDIR "/src/RunGroopWebApp"
#build and publish release
RUN dotnet build "RunGroopWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/builc

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RunGroopWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/put
from base as final
workdir /app
copy --from=publish /app/publish .
entrypoint ["dotnet", "RunGroopWebApp.dll"]#you need the framework and the dll file
#docker build -t flapi .
