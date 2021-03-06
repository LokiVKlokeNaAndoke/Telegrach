# Telegrach
![CI](https://github.com/LokiVKlokeNaAndoke/Telegrach/workflows/CI/badge.svg?branch=master)
[![](https://tokei.rs/b1/github/LokiVKlokeNaAndoke/Telegrach?category=lines)](https://github.com/XAMPPRocky/tokei)
[![](https://tokei.rs/b1/github/LokiVKlokeNaAndoke/Telegrach?category=code)](https://github.com/XAMPPRocky/tokei)
[![](https://tokei.rs/b1/github/LokiVKlokeNaAndoke/Telegrach?category=comments)](https://github.com/XAMPPRocky/tokei)

## Dependencies
### Manjaro
Run this:

`pacman -Syu ttf-croscore ttf-dejavu ttf-ubuntu-font-family ttf-inconsolata ttf-liberation`

## Server deployment
You can either use `docker-compose` or do it manually.
### Using docker-compose
1. Make sure you have `docker` and `docker-compose` installed
2. Run these commands:
```bash
git clone https://github.com/LokiVKlokeNaAndoke/Telegrach.git
cd Telegrach
docker-compose up -d
```
3. You now have a container with Telegrach server installed.
### Deploying manually
1. Install the postgres database
2. Clone the repository:
```bash
git clone https://github.com/LokiVKlokeNaAndoke/Telegrach.git
cd Telegrach
```
3. Execute the `compile.sh` bash script in the `protobuffers` directory:
```bash
bash protobuffers/compile.sh
```
4. Set all the ENV variables from [this list](#env-variables)
5. Run the `create_schema.py` script in the `Server` directory to create a schema in your DB
6. Start the server via the `start_server.py` script in the `Server` directory and you're good to go

### ENV variables
Variable | Explanation
--- | ---
TELEGRACH_DB_HOST | The address of a database instance
TELEGRACH_DB_USER | The username
TELEGRACH_DB_PORT | The port of a database instance
TELEGRACH_SCHEMA_NAME | The name of a schema
TELEGRACH_DB_NAME | The name of a database
TELEGRACH_DB_PW | (*optional*) the password to a database instance if required

The values of these variables are combined into a connection string which is used to connect to the postgres database:

`postgresql://{TELEGRACH_DB_USER}[:TELEGRACH_DB_PW]@{TELEGRACH_DB_HOST}:{TELEGRACH_DB_PORT}/{TELEGRACH_DB_NAME}`

## Building client manually
1. Clone the repository:
```bash
git clone https://github.com/LokiVKlokeNaAndoke/Telegrach.git
cd Telegrach
```
2. Execute the `compile.sh` bash script in the `protobuffers` directory:
```bash
bash protobuffers/compile.sh
```
3. Build the application like a dotnet core project (dotnet CLI is required): 
```bash
cd DesktopFrontend
dotnet build --configuration Release
```
4. Run the app:
```bash
dotnet DesktopFrontend/bin/Release/netcoreapp3.1/DesktopFrontend.dll
```