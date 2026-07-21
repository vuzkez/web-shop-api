# RESTful API для интернет-магазина, разработанный на **ASP.NET Core 9** с использованием современных архитектурных подходов: CQRS, MediatR, Clean Architecture, JWT-аутентификация.

## Возможности

**Аутентификация** - регистрация, вход, подтверждение email (stmp заглушка), jwt Токен
**Защита** - captcha при входе и регистрации
**Товары** - полный CRUD для админа
**Корзина** - только для авторизованных
**Заказы** - создание заказа из корзины, история заказов
**Валидация** - FluentValidation через MediatR pipeline
**Обработка ошибок** - глобальный обработчик с возвратом ProblemDetails
**Логирование** - логирование через MediatR Behavior

## Технологии

**.NET 9** - основная платформа
**ASP.NET Core Web API** - создание REST API
**Entity Framework Core** - ORM для работы с sql server
**MediatR** - библиотека с реализацией CQRS(просто разделены на комманды и запросы) и pipeline behaviors
**FluentValidation** - валидация запросов(ещё можно добавить)
**JWT** - аутентицикация
**reCAPTCHA.AspNetCore** - пакет для защиты с помощью google reCaptcha
**MailKit** - отправка email 
**Swagger** - для тестирования endpoints
