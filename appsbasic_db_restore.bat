@echo off
echo start copying db json files to container
docker cp %MoreJeeProject%/apps-basic/Apps.Basic.Service/wwwroot/AppBackup/. appsbasicservice_c:/app/wwwroot/AppBackup
echo restart docker 
docker restart appsbasicservice_c
echo all done!
pause
