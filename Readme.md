Install

dotnet dev-certs https --trusted

If you've installed dev-certs before and you still have an error try to first execute:

dotnet dev-certs https --clean

In main dir execute prepare_database.sh


To add and run migration execute in \backend\BankAnalizer\BankAnalizer.Db:

dotnet ef migrations add [migrationname] -s ..\BankAnalizer.Web\

then in main dir

dotnet ef database update --project "backend/BankAnalizer/BankAnalizer.Db" --startup-project "backend/BankAnalizer/BankAnalizer.Web"