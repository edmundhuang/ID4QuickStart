# IdentityServer quickstart

### Create IdentityServer project
1. Create from is4empty template
```
md quickstart
cd quickstart

md src
cd src

dotnet new is4empty -n IdentityServer
```

2. create a solution
```
cd ..
dotnet new sln -n Quickstart
```

3. add project to solution
```
dotnet sln add .\src\IdentityServer\IdentityServer.csproj
```

4. Modify lauchsettings.json
From
```
"applicationUrl": "https://localhost:5001"
```
To
```
"applicationUrl": "http://localhost:5000"
```

5. Run project to check server status  
[http://localhost:5000/.well-known/openid-configuration](http://localhost:5000/.well-known/openid-configuration)

### Adding an Api
```
dotnet new web -n Api
```

