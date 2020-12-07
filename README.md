# Change SDK versions

https://www.c-sharpcorner.com/article/switching-between-net-core-sdk-versions/

dotnet new globaljson --sdk-version 3.0.100-preview-010184 --force


# Function Apps

https://docs.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Cbrowser


# Create resources

az storage account create --name bccgnsteststr --location eastus --resource-group bccg-ns-test-rg --sku Standard_LRS

az functionapp create --resource-group bccg-ns-test-rg --consumption-plan-location eastus2 --runtime dotnet --functions-version 3 --name bccg-ns-test-func --storage-account bccgnsteststr


# Publish

func azure functionapp publish bccg-ns-test-func


# Test run

func start

### Local
- curl 'http://localhost:7071/api/CheckTargetStatus?mode=-l'
- curl 'http://localhost:7071/api/CheckNeweggStatus?mode=-l'
- curl 'http://localhost:7071/api/CheckGamestopStatus?mode=-l'

### Azure
- curl 'https://bccg-ns-test-func.azurewebsites.net/api/checktargetstatus?mode=-l&code=woxKa6bScWocvrGV6zZIjoOoHdVI3V5yxWz1bhekISzzFuafL5GkKg=='
- curl 'https://bccg-ns-test-func.azurewebsites.net/api/checkneweggstatus?mode=-l&code=uIsqGlUhAv7FVZhIHaJin6U4A050ak0l2ucHnkq6sCaajUCyBAR/jw=='
- curl 'https://bccg-ns-test-func.azurewebsites.net/api/checkgamestopstatus?mode=-l&code=5t2DeTTf4aslgSY2TUy3QDOfEeGO9muNvP4nPH8tZRRb6A8OTHEBPA=='


# Get process id for debug

ps -A | grep func


# Target key

personal: ff457966e64d5e877fdbad070f276d18ecec4a01


# Publish console MacOS

https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli

### Publish to folder as self contained
dotnet publish -c Release -r 'osx.10.14-x64' --self-contained true

### Copy to local apps
mkdir ~/workspace/console-apps/play-with-me/macos
rm -rf ~/workspace/console-apps/play-with-me/macos
cp -R ~/workspace/play-with-me/console/bin/Release/net5.0/osx.10.14-x64/publish/ ~/workspace/console-apps/play-with-me/macos

# Publish console Linux

### Publish to folder as self contained
dotnet publish -c Release -r 'linux-x64' --self-contained true

### Copy to local apps
mkdir ~/workspace/console-apps/play-with-me/linux
rm -rf ~/workspace/console-apps/play-with-me/linux
cp -R ~/workspace/play-with-me/console/bin/Release/net5.0/linux-x64/publish/ ~/workspace/console-apps/play-with-me/linux

# Run published 

./play-with-me -l -e

# Connect to VM

ssh -i mainkey.pem azureuser@52.152.162.60

### Copy to VM
scp -r -i mainkey.pem ~/workspace/console-apps/play-with-me/linux azureuser@52.152.162.60:apps/play-with-me/

# TODO

### Target
- Add Target cart checkout

### Newegg
- Add Newegg cart checkout
- Add support for searching Newegg

### Gamestop
- Add Gamestop search
- Add Gamestop store lookup (response captured)
- Add Gamestop cart

### Walmart
- Add Walmart

### Future
- Add flexible search to work for anything not just PS5
- Add multi-threading to the console

# Azure Functions Billing

https://www.nigelfrank.com/blog/ask-the-expert-measuring-the-cost-of-azure-functions/