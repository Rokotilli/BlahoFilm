# FilmService
## RegisterFilmController
- **GET /api/RegisterFilm/getsas** Отримати SaS токен для загрузки файла в сховище. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Параметри запиту:

blobName (string): Назва об'єкта Blob.

- **POST /api/RegisterFilm/register** - Зареєструвати новий фільм. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Поля тіла запиту (application/json):

Poster (file): Постер фільму.  
PosterPartOne (file)?: Перша частина розділеного простеру фільму (необов'язково).  
PosterPartTwo (file)?: Друга частина розділеного простеру фільму (необов'язково).  
PosterPartThree (file)?: Третя частина розділеного простеру фільму (необов'язково).  
Title (string): Назва фільму.  
Description (string): Опис фільму.    
Duration (string): Тривалість фільму.  
AgeRestriction (int): Вікове обмеження
Year (int): Рік випуску фільму.  
Director (string): Режисер фільму.  
Rating (int): Рейтинг фільму.  
Actors (string): Актори фільму.  
TrailerUri (string): Посилання на трейлер фільму.  
Studios (string): Студії фільму (через кому).  
Voiceovers (string): Озвучки фільму (через кому).  
Genres (string): Жанри фільму (через кому).  
Tags (string): Теги фільму (через кому).  

- **POST /api/RegisterFilm/uploadedvoiceover** - Додати в базу данних лінк на файл в сховищі. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (application/json):

FilmId (int): Ідентифікатор фільму.  
VoiceoverId (string): Ідентифікатор озвучки.  
FileUri (string): URI файлу.  

## RatingController
- **GET /api/Rating** - Отримати рейтинг фільму. Результати: Ok(результат), NotFound
  
Параметри запиту:

filmId (int): Ідентифікатор фільму.

- **POST /api/Rating** - Додати нову оцінку фільму. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Параметри запиту:

filmId (int): Ідентифікатор фільму.  
rate (int): Оцінка фільму.  

## FilmsController
- **GET /api/Films** - Отримати відпагінований список фільмів. Результати: Ok(результат), NotFound

Параметри запиту:

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  

- **GET /api/Films/countpages** - Отримати кількість сторінок. Результати: Ok(результат), NotFound
  
Параметри запиту:

pageSize (int): Розмір сторінки.

- **GET /api/Films/countpagesbyfilters** - Отримати кількість сторінок за фільтрами

Параметри запиту:  

pageSize (int): Розмір сторінки.  

Поля тіла запиту (application/json):  

Довільна кількість фільтрів  
До прикладу:  

{  
    "Genres": ["Action", "Drama"],  
    "Tags": ["Classic", "Must Watch"],  
    "Studios": ["Warner Bros", "Paramount"],  
    "Voiceovers": ["English", "Ukrainian"]  
}  

- **GET /api/Films/byid** - Отримати фільм за ідентифікатором. Результати: Ok(результат), NotFound
  
Параметри запиту:

id (int): Ідентифікатор фільму.

- **GET /api/Films/byids** - Отримати масив фільмів за ідентифікаторами: Результати: Ok(результат), NotFound
  
Поля тіла запиту (application/json):

ids (масив int): Масив ідентифікаторів фільмів.

- **GET /api/Films/bytitle** - Отримати фільми за назвою. Результати: Ok(результат), NotFound

Параметри запиту:

title (string): Назва фільму.

- **GET /api/Films/byfilters** - Отримати фільми за довільною кількість фільтрів. Результати: Ok(результат), NotFound
  
Параметри запиту:  

pageSize (int): Розмір сторінки.  

Поля тіла запиту (application/json):  

Довільна кількість фільтрів  
До прикладу:  

{  
    "Genres": ["Action", "Drama"],  
    "Tags": ["Classic", "Must Watch"],  
    "Studios": ["Warner Bros", "Paramount"],  
    "Voiceovers": ["English", "Ukrainian"]  
}  

- **GET /api/Films/fileuri** - Отримати лінк на файл. Результати: Ok(результат), NotFound
  
Параметри запиту:  

filmId (int): Ідентифікатор фільму.  
voiceoverId (int): Ідентифікатор озвучки.  

- **GET /api/Films/getsas** - Отримати SaS токен для завантажування файлу зі сховища. Результати: Ok, BadRequest(текст помилки)
  
Параметри запиту:  

blobName (string): Назва файлу в сховищі.  

- **GET /api/Films/genres** - Отримати всі жанри. Результати: Ok(результат), NotFound  

- **GET /api/Films/tags** - Отримати всі теги. Результати: Ok(результат), NotFound  
 
- **GET /api/Films/voiceovers** - Отримати всі озвучки. Результати: Ok(результат), NotFound  

- **GET /api/Films/studios** - Отримати всі студії. Результати: Ok(результат), NotFound  

## CommentsController
- **GET /api/Comments** - Отримати всі коментарі. Результати: Ok(результат), NotFound  
  
Параметри запиту:  

filmId (int): Ідентифікатор фільму.  

- **POST /api/Comments** - Створити новий коментар. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Поля тіла запиту (application/json):  

FilmId (int): Ідентифікатор фільму.  
ParentCommentId (int)?: Ідентифікатор батьківського коментаря (необов'язково).  
Text (string): Текст коментаря.  

- **DELETE /api/Comments** - Видалити коментар. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Параметри запиту:  

commentId (int): Ідентифікатор коментаря.  

- **PUT /api/Comments** - Змінити коментар. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Поля тіла запиту (application/json):  

Id (int): Ідентифікатор коментаря.  
Text (string): Текст коментаря.  

- **POST /api/Comments/like** - Додати/видалити лайк до коментаря. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Параметри запиту:  

commentId (int): Ідентифікатор коментаря.  

- **POST /api/Comments/dislike** - Додати/видалити дізлайк до коментаря. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Параметри запиту:  

commentId (int): Ідентифікатор коментаря.  

# UserService
## AuthController
- **POST /api/Auth/register** - Створити нового користувача. Результати: Ok, BadRequest(текст помилки)  

Поля тіла запиту (application/json):  

Email (string): Пошта користувача  
Password (string): Пароль користувача  

- **POST /api/Auth/authenticate** - Аутентифікувати користувача. Результати: Ok, BadRequest(текст помилки), NotFound, Forbidden  

Поля тіла запиту (application/json):  

Email (string): Пошта користувача  
Password (string): Пароль користувача  

- **PUT /api/Auth/refreshjwt** - Оновити JWT токен. Результати: Ok, BadRequest(текст помилки)  

- **DELETE /api/Auth/logout** - Вихід користувача з системи. Результат: Ok, BadRequest(текст помилки), Unauthorized  

- **POST /api/Auth/google** - Аутентифікувати користувача через Google. Результати: Ok, BadRequest(текст помилки)  

Поля тіла запиту (multipart/form-data):  

token (string): Токен доступу виданий Google  

- **GET /api/Auth/migrateuser** - Додати користувачу Google аутентифікацію. Результати: Ok, BadRequest(текст помилки), NotFound, Conflict  

Параметри запиту:  

token (string): Токен користувача  

- **GET /api/Auth/emailconfirm** - Підтвердити пошту користувача. Результати: Ok, BadRequest(текст помилки), Conflict  

Параметри запиту:  

token (string): Токен користувача  

## UsersController
- **GET /api/Users/byid** - Отримати користувача за ідентифікатором. Результати: Ok(результат), NotFound  

Параметри запиту:

id (int): Ідентифікатор користувача

- **GET /api/Users/byids** - Отримати користувачів за ідентифікаторами: Результати: Ok(результат), NotFound  

Поля тіла запиту (application/json):

ids (масив int): Масив Ідентифікаторів користувача

- **PUT /api/Users/avatar** - Змінити аватар користувача. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Поля тіла запиту (multipart/form-data):

avatar (file): Аватар користувача  

- **POST /api/Users/sendemailchangepassword** - Надіслати лист користувачу на пошту для зміни пароля. Результати: Ok, BadRequest(текст помилки), NotFound  

Поля тіла запиту (multipart/form-data):  

email (string): Поштовий адрес користувача  

- **PUT /api/Users/changepassword** - Змінити пароль користувача. Результати: Ok, BadRequest(текст помилки), NotFound  

Параметри запиту:  

token (string): Токен виданий сервісом для зміни пароля  

Поля тіла запиту (multipart/form-data):  

password (string): Новий пароль  

- **POST /api/Users/sendemailchangeemailaddress** - Надіслати лист користувачу на пошту для зміни пошти. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Поля тіла запиту (application/json):  

password (string): Пароль користувача  
email (string): Новий поштовий адрес користувача  

- **PUT /api/Users/changeemail** - Змінити пошту користувача. Результати: Ok, BadRequest(текст помилки)  

Параметри запиту:  

token (string): Токен виданий сервісом для зміни пошти    

- **PUT /api/Users/totaltime** - Змінити загальний час перегляду користувача. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Параметри запиту:  

seconds (int): Кількість секунд за сеанс перегляду  

- **PUT /api/Users/changenusername** - Змінити ім'я користувача. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Параметри запиту:  

username (string): Нове ім'я користувача

## HistoryController
- **GET /api/History** - Отримати історії користувача. Результати: Ok(результат), NotFound  

- **POST /api/History** - Додати історію користувачу. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Поля тіла запиту (application/json):  

MediaWithType (object){  
MediaId (int): Ідентифікатор медія  
MediaTypeId (int): Ідентифікатор типу медіа  
}  
PartNumber (int)?: Номер частини  
SeasonNumber (int)?: Номер сезона  
TimeCode (string): Позиція користувача на таймлайні  

## BookMarksController
- **GET /api/BookMarks** - Отримати закладки користувача. Результати: Ok(результат), NotFound  

- **POST /api/BookMarks** - Додати закладку користувачу. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Поля тіла запиту (application/json):  

MediaId (int): Ідентифікатор медія  
MediaTypeId (int): Ідентифікатор типу медіа  

# TransactionService
## FundraisingController  
- **GET /api/Fundraising** - Отримати відпагіновані збори. Результати: Ok(результат), NotFound  

Параметри запиту:  

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  

- **POST /api/Fundraising - Додати збір. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Поля тіла запиту (application/json):  

Title (string): Назва збору  
Description (string): Опис збору  
TotalAmount (decimal): Загальна кількість коштів  

- **PUT /api/Fundraising** - Змінити статус збору. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Параметри запиту:  

fundraisingId (int): Ідентифікатор збору  

Поля тіла запиту (application/json):  

Title (string): Назва збору  
Description (string): Опис збору  
TotalAmount (decimal): Загальна кількість коштів  

## SubscriptionController
- **GET /api/Subscription/subscriptions** - Отримати підписки користувача. Результати: Ok(результат), NotFound, Unauthorized  

- **POST /api/Subscription/subscribe** - Додати нову підписку користувачу. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Поля тіла запиту (application/json):  

OrderId (string): Ідентифікатор замовлення  
SubscriptionId (string): Ідентифікатор підписки  

- **PUT /api/Subscription/changestatus** - Змінити статус підписки користувача. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Параметри запиту:  

reason (string): Причина зміни статусу  
