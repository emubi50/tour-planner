FROM node:20 AS frontend-build

WORKDIR /app/frontend

COPY frontend/ ./

RUN npm install

RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend-build

WORKDIR /app

COPY backend/ .

RUN dotnet ef migrations add InitialCreate

RUN dotnet publish TourPlanner.Api -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS backend

WORKDIR /app

COPY --from=backend-build /publish .

COPY --from=frontend-build /app/frontend/dist/frontend/browser/ /app/public

EXPOSE 8080

ENTRYPOINT ["dotnet", "TourPlanner.Api.dll"]