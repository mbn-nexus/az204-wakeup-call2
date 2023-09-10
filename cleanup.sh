SUBSCRIPTION="Subscription 1"
RESOURCEGROUP="RG_MarcBraun"

az login -u $AZURE_ACCOUNTNAME
az account set --subscription "$SUBSCRIPTION"
az group delete --name $RESOURCEGROUP --yes
