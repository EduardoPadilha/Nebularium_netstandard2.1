@echo "Push dos pacotes para o Baget"
powershell -command "dotnet tool restore"
powershell -command "dotnet cake"
pause