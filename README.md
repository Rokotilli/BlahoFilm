# FilmService
## RegisterFilmController
- **GET /api/RegisterFilm/getsas**

Параметри запиту:

blobName (string): Назва об'єкта Blob.

- **POST /api/RegisterFilm/register**

Поля тіла запиту:

Poster (file): Постер фільму.  
Title (string): Назва фільму.  
Description (string): Опис фільму.    
Duration (string): Тривалість фільму.  
AgeRestriction (int): Вікове обмеження
Year (int): Рік випуску фільму.  
Director (string): Режисер фільму.  
Rating (int): Рейтинг фільму.  
Actors (string): Актори фільму.  
StudioName (string): Назва студії, яка зняла фільм.  
TrailerUri (string): Посилання на трейлер фільму.  
Genres (string): Жанри фільму (через кому).  
Tags (string): Теги фільму (через кому).  

- **POST /api/RegisterFilm/uploadedfilm**
  
Поля тіла запиту:

Id (int): Ідентифікатор фільму.  
FileName (string): Назва файлу.  
FileUri (string): URI файлу.  

## RatingController
- **GET /api/Rating**
  
Параметри запиту:

filmId (int): Ідентифікатор фільму.

- **POST /api/Rating**
  
Параметри запиту:

filmId (int): Ідентифікатор фільму.  
rate (int): Оцінка фільму.  

## FilmsController
- **GET /api/Films**

Параметри запиту:

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  

- **GET /api/Films/countpages**
  
Параметри запиту:

pageSize (int): Розмір сторінки.

- **GET /api/Films/countpagesbygenres**
  
Параметри запиту:

pageSize (int): Розмір сторінки.

Поля тіла запиту:

genres (масив string): Масив жанрів фільму.

- **GET /api/Films/countpagesbytags**
  
Параметри запиту:

pageSize (int): Розмір сторінки.

Поля тіла запиту:

tags (масив string): Масив тегів фільму.

- **GET /api/Films/byid**
  
Параметри запиту:

id (int): Ідентифікатор фільму.

- **GET /api/Films/byids**
  
Поля тіла запиту:

ids (масив int): Масив ідентифікаторів фільмів.

- **GET /api/Films/bytitle**

Параметри запиту:

title (string): Назва фільму.

- **GET /api/Films/bygenres**
  
Параметри запиту:

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  

Поля тіла запиту:

genres (масив string): Масив жанрів фільму.

- **GET /api/Films/bytags**
  
Параметри запиту:

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  

Поля тіла запиту:

tags (масив string): Масив тегів фільму.  

## CommentsController
- **GET /api/Comments**
  
Параметри запиту:

filmId (int): Ідентифікатор фільму.

- **POST /api/Comments**
  
Поля тіла запиту:

FilmId (int): Ідентифікатор фільму.  
ParentCommentId? (int): Ідентифікатор батьківського коментаря (необов'язково).  
Text (string): Текст коментаря.  

- **DELETE /api/Comments**
  
Параметри запиту:

commentId (int): Ідентифікатор коментаря.

- **PUT /api/Comments**
  
Поля тіла запиту:

Id (int): Ідентифікатор коментаря.  
Text (string): Текст коментаря.  

- **POST /api/Comments/like**

Параметри запиту:

commentId (int): Ідентифікатор коментаря.

- **POST /api/Comments/dislike**
  
Параметри запиту:

commentId (int): Ідентифікатор коментаря.

# UserService
## AuthController
- **POST /api/Auth/register**

Поля тіла запиту:  

Email (string): Пошта користувача
Password (string): Пароль користувача  

- **POST /api/Auth/authenticate**

Поля тіла запиту:  

Email (string): Пошта користувача
Password (string): Пароль користувача  

- **PUT /api/Auth/refreshjwt**

- **DELETE /api/Auth/logout**

- **GET /api/Auth/google**

- **GET /api/Auth/migrateuser**

Параметри запиту:  

token (string): Токен користувача  

- **GET /api/Auth/emailconfirm**

Параметри запиту:  

token (string): Токен користувача  

## UsersController
- **GET /api/Users/byid**

Параметри запиту:

id (int): Ідентифікатор користувача

- **GET /api/Users/byids**

Поля тіла запиту:

ids (масив int): Масив Ідентифікаторів користувача

- **PUT /api/Users/avatar**

Поля тіла запиту:

avatar (file): Аватар користувача

- **PUT /api/Users/totaltime**  

Параметри запиту:  

seconds (int): Кількість секунд за сеанс перегляду  

- **PUT /api/Users/changenusername**  

Параметри запиту:  

username (string): Нове ім'я користувача

## HistoryController
- **GET /api/History**

- **POST /api/History**

Поля тіла запиту:  

MediaWithType (object){  
MediaId (int): Ідентифікатор медія  
MediaTypeId (int): Ідентифікатор типу медіа  
}  
PartNumber (int)?: Номер частини  
SeasonNumber (int)?: Номер сезона  
TimeCode (string): Позиція користувача на таймлайні  

## BookMarksController
- **GET /api/BookMarks**

- **POST /api/BookMarks**

Поля тіла запиту:  

MediaId (int): Ідентифікатор медія  
MediaTypeId (int): Ідентифікатор типу медіа  

# TransactionService
## FundraisingController  
- **GET /api/Fundraising**

Параметри запиту:  

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  

- **POST /api/Fundraising

Поля тіла запиту:  

Title (string): Назва збору  
Description (string): Опис збору  
TotalAmount (decimal): Загальна кількість коштів  

- **PUT /api/Fundraising**  

Параметри запиту:  

fundraisingId (int): Ідентифікатор збору  

Поля тіла запиту:  

Title (string): Назва збору  
Description (string): Опис збору  
TotalAmount (decimal): Загальна кількість коштів  

## TransactionController
- **POST /api/Transaction/subscribe**

Поля тіла запиту:  

OrderId (string): Ідентифікатор замовлення  
SubscriptionId (string): Ідентифікатор підписки  

- **PUT /api/Transaction/changestatus**

Параметри запиту:  

reason (string): Причина зміни статусу  
