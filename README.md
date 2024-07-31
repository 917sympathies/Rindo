# How to start?

# Localy:

### Server:
### 1) Make sure that in server/Rindo.Infrastructure/DependencyInjection/DependencyInjection.cs in the database configuration you have "DbConnectionString" in connection string section
### 2.1) From IDE: just start the project
### 2.2) From cmd:
### 2.2.1) Open server folder
### 2.2.2) Open Rindo.API project folder (cd Rindo.API)
### 2.2.3) run "dotnet run" in cmd
### Client:
### 1) Make sure that in client/src/requests.ts baseUrl is set to "http://localhost:5000"
### 2) Open client folder
### 3) run "npm run dev"

# Docker:

### It's ready for docker-compose from the start, so all you need to do is:
### 1) Open project folder in cmd
### 2) Open server folder (cd server)
### 3) run "docker compose up"
### 4) run
