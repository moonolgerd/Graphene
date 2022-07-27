
```powershell
docker run --rm -it `
-p 5099:80 -p 7099:443 `
-e ASPNETCORE_URLS="https://+;http://+" `
-e ASPNETCORE_HTTPS_PORT=7099 `
-e ASPNETCORE_Kestrel__Certificates__Default__Password=<> `
-e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx `
-e Authentication__Okta__OktaDomain=<> `
-e Logging__LogLevel__Default=Debug `
-e Logging__LogLevel__Microsoft.AspNetCore=Debug `
-v $env:USERPROFILE\.aspnet\https:/https/ `
grapheneserver:latest
```