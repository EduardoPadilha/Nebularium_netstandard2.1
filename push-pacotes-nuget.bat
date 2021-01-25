@echo "Push dos pacotes para o Github"
powershell -command "dotnet tool restore"
powershell -command "dotnet cake"
pause