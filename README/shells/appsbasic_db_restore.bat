@echo off
echo start copying db json files to container
docker cp D:/Leon/AppCode/bamboo/morejee-api/apps-basic/Apps.Basic.Service/wwwroot/AppBackup/. apps-legacy-basic-service-ym-c:/app/wwwroot/AppBackup
echo restart docker 
docker restart apps-legacy-basic-service-ym-c
echo all done!
pause
