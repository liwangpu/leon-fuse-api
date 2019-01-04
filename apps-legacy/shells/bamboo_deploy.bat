@echo off
@echo:
echo 1)  CD To "Apps-Legacy" folder,please checkout the output folder
cd ..
echo %CD%

@echo:
echo 2) unload projects
docker-compose down

@echo:
echo 3) rebuild projects

docker-compose build

@echo:
echo 4) install projects
docker-compose -f docker-compose.yml -f docker-compose.bamboo.yml up

pause