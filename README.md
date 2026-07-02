# 🛒 SaleDot POS – Desktop Point of Sale System

Этот репозиторий содержит **дипломный проект 2022 года**: настольную систему автоматизации точки продаж. Приложение помогает вести товары, клиентов, поставщиков, продажи, закупки, кассовые операции и базовую финансовую отчетность.

Проект реализован как **WPF-приложение для Windows** на **C# / .NET Framework 4.8**. Для работы с данными используются **MySQL**, **Dapper** и **Entity Framework**.

## 📦 Архитектура

Проект включает основные модули POS-системы и учетной платформы:

| Компонент | Назначение |
|-----------|------------|
| `Dashboard` | Главная панель со статистикой продаж, клиентов, поставщиков и персонала |
| `product` | Управление товарами, остатками, ценами, скидками и штрих-кодами |
| `finance` | Продажи, закупки, счета, балансы, расходы, кассовые операции |
| `user` | Клиенты, поставщики, сотрудники и роли пользователей |
| `report` | Просмотр отчетов через ReportViewer |
| `others` | Настройки базы данных, печати, SMS, SQL-запросы и служебные окна |
| `data/dapper` | Репозитории и доступ к MySQL через Dapper |
| `bll` | Бизнес-логика, печать, работа с настройками и утилиты |

Приложение ориентировано на локальный запуск в учебной среде и имитирует рабочую систему учета для небольшой торговой точки.

## 🚀 Быстрый старт

> ⚠️ Требования: Windows, Visual Studio 2022, .NET Framework 4.8 Developer Pack и MySQL Server.

### 1. Клонируйте репозиторий

```bash
git clone https://github.com/steel-snake-sx/saledot-pos.git
cd saledot-pos
```

### 2. Откройте solution

```text
SaleDot/SaleDot.sln
```

### 3. Восстановите зависимости и соберите проект

В Visual Studio выберите конфигурацию `Debug|x86` или `Release|x86`, восстановите NuGet-пакеты и выполните сборку.

Для проверки через MSBuild:

```powershell
MSBuild.exe SaleDot/SaleDot.sln /p:Configuration=Debug /p:Platform=x86 /restore
```

### 4. Настройте MySQL

При первом запуске приложение проверяет подключение к MySQL. Если подключение не установлено, откроется окно настройки базы данных.

Значения по умолчанию:

| Параметр | Значение |
|----------|----------|
| `Server` | `localhost` |
| `Database` | `saledot` |
| `User` | `root` |
| `Password` | `1234` |

### 5. Запустите приложение

После успешной сборки запустите проект из Visual Studio. Тестовый вход администратора:

```text
Login: superadmin
Password: sa@bb
```

## 🧩 Технологии

- [.NET Framework 4.8](https://learn.microsoft.com/en-us/dotnet/framework/)
- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
- [MySQL](https://www.mysql.com/)
- [Dapper](https://github.com/DapperLib/Dapper)
- [Entity Framework 6](https://learn.microsoft.com/en-us/ef/ef6/)
- [MaterialDesignThemes](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
- [Telerik UI for WPF](https://www.telerik.com/products/wpf/overview.aspx)
- [Microsoft ReportViewer](https://learn.microsoft.com/en-us/sql/reporting-services/application-integration/integrating-reporting-services-using-reportviewer-controls)
- Microsoft Office Interop Excel

## 🔧 Структура каталогов

```text
SaleDot/
├── SaleDot.sln
├── SaleDot/
│   ├── SaleDot.POS.csproj
│   ├── App.xaml
│   ├── Login.xaml
│   ├── bll/
│   ├── data/
│   │   ├── dapper/
│   │   └── viewmodel/
│   ├── Views/
│   │   ├── finance/
│   │   ├── product/
│   │   ├── user/
│   │   ├── others/
│   │   └── report/
│   └── assets/
```

Резервные каталоги Visual Studio за 2022 год (`SaleDot_Backup_*`, `MigrationBackup`), а также `bin`, `obj` и `.vs` исключены из Git. Для запуска актуальной версии используется проект `SaleDot/SaleDot`.

## ⚙️ Конфигурация

Основные параметры задаются через настройки приложения и окно `DatabaseSettingWindow`:

| Настройка | Назначение |
|-----------|------------|
| `DatabaseServer` | Адрес MySQL-сервера |
| `DatabaseName` | Имя базы данных |
| `DatabaseUsername` | Пользователь MySQL |
| `DatabasePassword` | Пароль пользователя MySQL |
| `PrinterPageWidth` | Ширина страницы чека |
| `NumberOfReceiptToPrint` | Количество печатаемых чеков |
| `BarcodeMode` | Режим работы POS-окна со штрих-кодом |

Локальный файл `SaleDot/SaleDot/data/databaseconnection.json` содержит параметры подключения конкретной машины и исключен из Git через `.gitignore`.

## 🧪 Проверка

Проект можно проверить сборкой solution в Visual Studio или через MSBuild:

```powershell
MSBuild.exe SaleDot/SaleDot.sln /p:Configuration=Debug /p:Platform=x86 /restore
```

После запуска проверьте основные сценарии:

- вход под `superadmin`;
- открытие главной панели;
- открытие справочника товаров;
- открытие POS-окна;
- подключение к MySQL через окно настроек.

## 🗄️ База данных

Приложение рассчитано на MySQL. В коде предусмотрено создание базы из файла `data/saledot.sql`, однако в текущем архивном состоянии репозитория этот SQL-файл отсутствует.

Для полноценного запуска может потребоваться восстановить SQL-дамп дипломного проекта или вручную создать схему по моделям и репозиториям из каталога `SaleDot/SaleDot/data`.

## 🧼 Статус проекта

Это архивный учебный проект 2022 года. Он демонстрирует реализацию POS-системы для дипломной работы и может требовать доработок перед реальным использованием: восстановления схемы БД, обновления зависимостей, ревизии безопасности и очистки старых резервных файлов.

## 📎 Лицензия

MIT License. Подробнее см. `LICENSE`.
