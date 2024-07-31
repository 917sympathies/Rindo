# How to start the project?

## Localy

### Server:
1. Make sure that in server/Rindo.Infrastructure/DependencyInjection/DependencyInjection.cs you have set "DbConnectionString" as connection string
```
services.AddDbContext<RindoDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DbConnectionString"),
```
2. 
  - 2.1 From IDE: just start the project
  - 2.2 From cmd:
    - 2.2.1 Open server folder
    - 2.2.2 Open Rindo.API project folder (cd Rindo.API)
    - 2.2.3 run command below
```
"dotnet run"
```
### Client:
1. Make sure that in client/src/requests.ts baseUrl is set to "http://localhost:5000"
```
const baseUrl = "http://localhost:5000";
```
2. Open client folder
3. run command below
```
npm run dev
```

## Docker:

It's ready for docker-compose from the start, so all you need to do is:
1. Open project folder in cmd
2. Open server folder
```
cd server
```
4. run command below
```
docker compose up -d
```
6. open your browser on localhost
