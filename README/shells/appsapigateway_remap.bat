
@echo off
echo start copying ocelot.json to container
docker cp %MoreJeeProject%/apps-common/Apps.Base.APIGateway/ocelot.json appsbaseapigateway_c:/app
echo restart APIGateway
docker restart appsbaseapigateway_c
echo all done!
pause
