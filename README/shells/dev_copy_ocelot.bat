@echo off
echo start copying ocelot.Production.json to APIGateway
docker cp %MoreJeeProject%/apps-common/Apps.Base.APIGateway/ocelot.Production.json apps-apigateway-dev-c:/app
echo  copy finish
echo restart APIGateway
docker restart apps-apigateway-dev-c
echo restart finish
echo all done!
pause