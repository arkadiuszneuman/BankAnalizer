#!/bin/bash
docker-compose up -d
dotnet tool install --global dotnet-ef
hash -r
dotnet ef database update --project "backend/BankAnalizer/BankAnalizer.Db" --startup-project "backend/BankAnalizer/BankAnalizer.Web"