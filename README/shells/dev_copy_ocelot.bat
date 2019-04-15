@echo off
echo start copying ocelot.Leon.json to APIGateway
docker cp D:/Leon/AppCode/bamboo/morejee-api/apps-common/Apps.Base.APIGateway/ocelot.YuanMin.json apps-apigateway-ym-c:/app
echo  copy finish
echo restart APIGateway
docker restart apps-apigateway-ym-c
echo restart finish
echo all done!
pause