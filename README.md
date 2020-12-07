### Change SDK versions

https://www.c-sharpcorner.com/article/switching-between-net-core-sdk-versions/

dotnet new globaljson --sdk-version 3.0.100-preview-010184 --force


### Function Apps

https://docs.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Cbrowser


### Create resources

az storage account create --name bccgnsteststr --location eastus --resource-group bccg-ns-test-rg --sku Standard_LRS

az functionapp create --resource-group bccg-ns-test-rg --consumption-plan-location eastus2 --runtime dotnet --functions-version 3 --name bccg-ns-test-func --storage-account bccgnsteststr


### Publish

func azure functionapp publish bccg-ns-test-func


### Test run

func start

- curl 'http://localhost:7071/api/CheckTargetStatus?mode=-l'
- curl 'http://localhost:7071/api/CheckNeweggStatus?mode=-l'
- curl 'http://localhost:7071/api/CheckGamestopStatus?mode=-l'

- curl 'https://bccg-ns-test-func.azurewebsites.net/api/checktargetstatus?mode=-l&code=woxKa6bScWocvrGV6zZIjoOoHdVI3V5yxWz1bhekISzzFuafL5GkKg=='
- curl 'https://bccg-ns-test-func.azurewebsites.net/api/checkneweggstatus?mode=-l&code=uIsqGlUhAv7FVZhIHaJin6U4A050ak0l2ucHnkq6sCaajUCyBAR/jw=='
- curl 'https://bccg-ns-test-func.azurewebsites.net/api/checkgamestopstatus?mode=-l&code=5t2DeTTf4aslgSY2TUy3QDOfEeGO9muNvP4nPH8tZRRb6A8OTHEBPA=='


### Get process id for debug

ps -A | grep func


### Target key

personal: ff457966e64d5e877fdbad070f276d18ecec4a01


### Publish console

https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli

dotnet publish -c Release -r 'osx.10.14-x64' --self-contained true

cp -R ~/workspace/play-with-me/console/bin/Release/net5.0/osx.10.14-x64/publish/ ~/workspace/console-apps/play-with-me

./play-with-me -l -e

### TODO

- Add Target cart checkout

- Add Newegg cart checkout
- Add support for searching Newegg

- Add Gamestop search
- Add Gamestop store lookup (response captured)
- Add Gamestop cart

- Add Walmart

- Add flexible search to work for anything not just PS5