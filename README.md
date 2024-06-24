# MessangerBack
Бэкэнд мессенджера

## Запустить
Для запуска бэкенд сервера необходима база данных PostgreSQL.

### Запустить локально
1. Заполнить файл appsettings.json по примеру файла example.appsettings.json
2. Выполнить миграцию
3. ```dotnet build```
4. ```dotnet run```

### Миграция
Справочная информация: https://learn.microsoft.com/ru-ru/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

Миграции создавать не нужно, необходимо только обновить базу данных в соответствие с миграциями.
Сделать это можно следубщим образом: 
* Visual Studio: ```Update-Database```
  
  Или
* Интерфейс командной строки .Net Core ```dotnet ef database update```
