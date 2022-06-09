rm data.db
rm ./Migrations/*
dotnet ef migrations add FirstMigration
dotnet ef database update
