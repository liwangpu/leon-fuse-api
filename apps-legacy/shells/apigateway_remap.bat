
@echo off
echo start copying ocelot.json to container
docker cp %MoreJeeProject%/apps-legacy/ocelot.json dmzapigateway_c:/app
echo restart APIGateway
docker restart dmzapigateway_c
echo all done!
pause
