@echo off

echo 1)  CD To "Apps-Legacy" folder,please checkout the output folder
cd ..
echo %CD%

echo 2) unload projects
docker-compose down

echo 3) rebuild projects

docker-compose build

echo 4) install projects
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up

pause