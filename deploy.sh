SUBSCRIPTION="Subscription 1"
RESOURCEGROUP="RG_MarcBraun"
ASP_NAME="ASP_MarcBraun"
ASP_SKU="S1"
FNC_NAME="fncmarcbraunhelloworld"
STG_NAME="stgmarcbraun"


az login -u $AZURE_ACCOUNTNAME
# az account subscription list --output table
az account set --subscription "$SUBSCRIPTION"
az group create --name $RESOURCEGROUP --location "eastus"
az appservice plan create --name "$ASP_NAME" --resource-group $RESOURCEGROUP --sku $ASP_SKU --is-linux
az storage account create --name $STG_NAME --resource-group $RESOURCEGROUP --location "eastus" --sku Standard_LRS
az functionapp create --name "$FNC_NAME" --resource-group $RESOURCEGROUP --plan "$ASP_NAME" --runtime dotnet --runtime-version 6.0 --functions-version 4 --os-type Linux --storage-account $STG_NAME
dotnet publish -c Release 
$(cd bin/Release/net6.0/publish && zip -r ../../../../publish.zip .)
az functionapp deployment source config-zip --resource-group "$RESOURCEGROUP" --name $FNC_NAME --src publish.zip
az functionapp config appsettings set --resource-group "$RESOURCEGROUP" --name $FNC_NAME --settings "SendGridApiKey=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"