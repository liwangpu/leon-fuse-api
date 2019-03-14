@echo off
echo start copying ocelot.Leon.json to APIGateway
docker cp %MoreJeeProject%/apps-common/Apps.Base.APIGateway/ocelot.Leon.json apps-apigateway-leon-c:/app
echo  copy finish
echo restart APIGateway
docker restart apps-apigateway-leon-c
echo restart finish
echo all done!
pause