cd ../src/Console

dotnet publish -c Release -r 'linux-x64' --self-contained true

cd ../..

scp -r -i mainkey.pem ~/workspace/play-with-me/src/Console/bin/Release/net5.0/linux-x64/publish azureuser@52.152.162.60:apps/play-with-me/
