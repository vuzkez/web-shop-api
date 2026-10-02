# Web Shop API

Лёгкий RESTful API для интернет‑магазина — пет‑проект на C#, созданный, чтобы попрактиковаться в проектировании API, работе с EF Core и опробовать MediatR с подходом CQRS в упрощённом виде (без разделения на разные базы данных).

## Содержание
- [Функции](#функции)
- [Технологии](#технологии)
- [Архитектура](#архитектура)
- [Быстрый старт](#быстрый-старт)
- [API](#api-основные-эндпоинты)
- [Аутентификация и безопасность](#аутентификация-и-безопасность)
- [Планы](#планы)

## Функции
- Управление товарами: просмотр, поиск, фильтрация (CRUD для администратора)
- Категории и атрибуты товаров
- Корзина: добавление и удаление позиций, расчёт итоговой суммы
- Оформление заказов: создание, просмотр, изменение статуса (покупатель + админ)
- Регистрация и вход (JWT, ASP.NET Identity), роли user и admin
- Отправка писем через SMTP
- Интеграция Google reCAPTCHA
- Документация API (Swagger с поддержкой Bearer-токена)

## Технологии
- C#, ASP.NET Core Web API, .NET 9
- Entity Framework Core (SQL Server), миграции
- ASP.NET Identity + JWT Bearer
- MediatR (CQRS lite)
- FluentValidation
- MailKit (отправка email)
- reCAPTCHA (`reCAPTCHA.AspNetCore`)
- Swagger / Swashbuckle

## Архитектура

Структура проекта:

`Controllers`: HTTP-эндпоинты 
`Applications`: Прикладная логика: команды и запросы MediatR с обработчиками, валидаторы, behaviors, сервисы (JWT, email), настройки 
`Domain`: Сущности 
`Infrastructure/Data`: `AppDbContext`, инициализация БД 
`ExceptionHandler`: Глобальная обработка исключений 
`Migrations`: Миграции EF Core 

### MediatR и CQRS (lite)
Команды и запросы отправляются через MediatR и обрабатываются отдельными handler-классами. Чтение и запись разделены на уровне кода, но используется одна БД и один `DbContext`.

В пайплайн MediatR подключены два behavior:
1. `LoggingBehavior` — логирование запросов
2. `ValidationBehavior` — валидация команд и запросов через FluentValidation до вызова handler-а

### Обработка ошибок
`GlobalExceptionHandler` (реализация `IExceptionHandler`) перехватывает исключения и возвращает ответ в формате `ProblemDetails`.

### Инициализация БД
В окружении Development при старте вызывается `DbInitializer`.

## Быстрый старт

### Требования
- .NET SDK 9.0
- SQL Server

### Запуск
```
git clone https://github.com/vuzkez/web-shop-api.git
cd web-shop-api
dotnet run
```

Swagger доступен по адресу `/swagger` в окружении Development.

### Конфигурация
Создайте `appsettings.Development.json` (или используйте переменные окружения / user-secrets) по образцу `appsettings.template.json`.

- `ConnectionStrings:DefaultConnection` — строка подключения к SQL Server
- `Jwt` — `Key` (не короче 32 символов), `Issuer`, `Audience`, `ExpiryInMinutes`
- `ReCaptcha` — `SiteKey`, `SecretKey`
- `Email` — `SmtpHost`, `SmtpPort`, `SmtpUsername`, `SmtpPassword`, `FromEmail`, `FromName`
- `App:BaseUrl` — базовый адрес приложения

Пример:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WebShopDb;User Id=sa;Password=your_password;TrustServerCertificate=True;"
  },
  "ReCaptcha": {
    "SiteKey": "",
    "SecretKey": ""
  },
  "App": {
    "BaseUrl": "https://localhost:7040"
  },
  "Email": {
    "SmtpHost": "sandbox.smtp.mailtrap.io",
    "SmtpPort": 587,
    "SmtpUsername": "Name",
    "SmtpPassword": "Password",
    "FromEmail": "@myshop.com",
    "FromName": "MyShop"
  },
  "Jwt": {
    "Key": "",
    "Issuer": "MyShopApi",
    "Audience": "MyShopClient",
    "ExpiryInMinutes": 60
  },
  "AllowedHosts": "*"
}
```

### Миграции
Нужен инструмент `dotnet-ef` (`dotnet tool install --global dotnet-ef`):
```
dotnet ef database update
```

## API (основные эндпоинты)

- **Товары**
  - `GET /api/products` — список товаров (пагинация, фильтры)
  - `GET /api/products/{id}` — информация о товаре
  - `POST /api/products` — создать товар (admin)
  - `PUT /api/products/{id}` — обновить товар (admin)
  - `DELETE /api/products/{id}` — удалить товар (admin)

- **Корзина**
  - `GET /api/cart` — текущее состояние корзины
  - `POST /api/cart/items` — добавить товар
  - `PUT /api/cart/items/{itemId}` — изменить количество
  - `DELETE /api/cart/items/{itemId}` — удалить позицию

- **Заказы**
  - `POST /api/orders` — создать заказ
  - `GET /api/orders` — заказы пользователя (admin видит все)
  - `GET /api/orders/{id}` — детали заказа
  - `PUT /api/orders/{id}/status` — изменить статус (admin)

- **Аутентификация**
  - `POST /api/auth/register` — регистрация
  - `POST /api/auth/login` — вход, возвращает JWT

Для защищённых маршрутов нужен заголовок `Authorization: Bearer <token>`.

## Аутентификация и безопасность
- JWT-токены с проверкой issuer, audience, срока действия и подписи
- ASP.NET Identity для хранения пользователей и политики паролей
- Ролевая авторизация (user, admin)
- Валидация входных данных через FluentValidation (MediatR behavior)
- EF Core использует параметризованные запросы, что защищает от SQL-инъекций
- Google reCAPTCHA
- Единый формат ошибок (`ProblemDetails`)

## Планы
- Добавить unit- и интеграционные тесты (xUnit, NSubstitute)
- Добавить Dockerfile и docker-compose
