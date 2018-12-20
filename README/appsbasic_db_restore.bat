echo copy db json files to container
docker cp %MoreJeeProject%/apps-basic/Apps.Basic.Service/wwwroot/AppBackup/. appsbasicservice_c:/app/wwwroot/AppBackup
docker restart appsbasicservice_c
echo all done!
pause
