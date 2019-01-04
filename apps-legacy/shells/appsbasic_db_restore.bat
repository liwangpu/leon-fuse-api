@echo off
echo start copying db json files to container
docker cp %MoreJeeProject%/apps-legacy/ApiServer/wwwroot/AppBackup/. dmzapiserver_c:/app/wwwroot/AppBackup
echo restart docker 
docker restart dmzapiserver_c
echo all done!
pause
