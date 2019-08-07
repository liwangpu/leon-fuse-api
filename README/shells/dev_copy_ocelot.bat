@echo off
echo start copying ocelot.Leon.json to APIGateway
docker cp D:/Leon/AppCode/bamboo/morejee-api/apps-common/Apps.Base.APIGateway/ocelot.Production.json app-infrastructure-apigateway-c:/app
echo  copy finish
echo restart APIGateway
docker restart app-infrastructure-apigateway-c
echo restart finish
echo all done!
pause