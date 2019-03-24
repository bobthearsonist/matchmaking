# setup

first we need to install docker

https://www.docker.com/products/docker-desktop

after you have installed docker you need to standup the image for the server

```
docker pull microsoft/mssql-server-linux
docker run -d --name matchmaker-db -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=f8DL9JkFx$1qrN4' -p 1433:1433 microsoft/mssql-server-linux
docker ps -a
```

thats it, your server is running. now you need to run the database setup script from kristins work.

for osx you can either use the command line or azure data studio found here https://docs.microsoft.com/en-us/sql/azure-data-studio/download?view=sql-server-2017

```powershell
$npm install -g sql-cli
```

for windows  you can use several managers for the db, SSMS works or you may want to install the cmd line tools, i think you can also use the azure tool for windows.

after you setup your server you need to run the app. you can do this in vs code or visual studio, it works in anything that will run dotnet. in fact you can run it form the console by running this command from the PlayerMatcherService folder.

```
dotnet build
dotnet run
```

to test the service you can use curl to list the users in the database or give a specific userid as a param. additional services will be added and documented with swagger.

```
curl -i -k https://localhost:5001/api/user
curl -i -k https://localhost:5001/api/user/1
```