# Install toolchain

docker

```bash
$docker pull microsoft/mssql-server-linux
$docker run -d --name matchmaker-db -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=f8DL9JkFx$1qrN4' -p 1433:1433 microsoft/mssql-server-linux
$docker ps -a
```

windows install cli

```bash
$npm install -g sql-cli
```

