# PaintStore_backEnd

|Version|Quality|Coverage|State|
|---|---|---|---|
|![alt text](https://img.shields.io/badge/version-1.0.1-brightgreen.svg) |[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=wojtek-rak_PaintStore_BackEnd&metric=alert_status)](https://sonarcloud.io/dashboard?id=wojtek-rak_PaintStore_BackEnd)|[![alt text](https://img.shields.io/badge/Coverage-63.6%25-red.svg)](https://sonarcloud.io/component_measures?id=wojtek-rak_PaintStore_BackEnd&metric=coverage)| ![alt text](https://img.shields.io/badge/state-in%20development-red.svg)|


REST service supporting social media service.

Description
=====
Full endpoint docs: [docs-paintstore.azurewebsites.net/swagger](https://docs-paintstore.azurewebsites.net/swagger)



To configure:
-------
Add appsettings.json file with this text,
```json
    
 {
  "ConnectionStrings": {
    "PaintStoreDatabase": "(your_conection_string)"
  },
  "AppIdentitySettings": {
    "CouldName": "(your_CouldName)",
    "ApiKey": "(your_ApiKey)",
    "SecretApiKey": "(your_SecretApiKey)",
    },
    "Logging": {
      "IncludeScopes": false,
      "Debug": {
        "LogLevel": {
          "Default": "Warning"
        }
      },
      "Console": {
        "LogLevel": {
          "Default": "Warning"
        }
      }
    }
  }

```


Implemented Features
------
#### Auto Countings for:
```
Post likes in Post
Comment likes in Comment
Post count in User
Comment count in Post
Followed user in User
Following user in User
CategoryType count in CategoryType
CategoryTool count in CategoryTool
```
#### Validation for:
```
Post likes
Comment likes
Follows
```

