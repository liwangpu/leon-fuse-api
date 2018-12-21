:: 该bat主要为了将项目路径保存到MoreJeeProject环境变量

@echo off
cd ../../
echo start saving project base path to system environment
setx MoreJeeProject %cd%
echo all done!
pause
