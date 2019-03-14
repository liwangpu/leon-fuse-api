@echo off
echo start copying db json files to container
docker cp %MoreJeeProject%/apps-basic/Apps.Basic.Service/wwwroot/AppBackup/. apps-legacy-basic-service-leon-c:/app/wwwroot/AppBackup
echo restart docker 
docker restart apps-legacy-basic-service-leon-c
echo all done!
pause
