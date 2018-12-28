@echo off
@echo:
echo 1)  CD To "Apps-Legacy" folder,please checkout the output folder
cd ..
echo %CD%

@echo:
echo 2) unload projects
docker-compose down

@echo:
echo 3) install projects
docker-compose -f docker-compose.bamboo.yml -f docker-compose.yml up


pause