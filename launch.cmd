REM for /d /r . %%d in (bin,obj) do @if exist "%%d" rd /s/q "%%d"
START cmd /k "cd ./Scenario4/MedicalWebsite && dotnet run -f net46 https://*:5106"
START cmd /k "cd ./Scenario4/SamplesApi && dotnet run -f net46 http://*:5107"
echo Applications are running ...