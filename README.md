# FilmService
## RegisterFilmController
- **GET /api/RegisterFilm/getsas** - Отримати SaS токен для загрузки файла в сховище. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Параметри запиту:

blobName (string): Назва об'єкта Blob.

- **POST /api/RegisterFilm/register** - Зареєструвати новий фільм. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Поля тіла запиту (multipart/form-data):

Poster (file): Постер фільму.  
PosterPartOne (file)?: Перша частина розділеного постеру фільму (необов'язково).  
PosterPartTwo (file)?: Друга частина розділеного постеру фільму (необов'язково).  
PosterPartThree (file)?: Третя частина розділеного постеру фільму (необов'язково).  
Title (string): Назва фільму.  
Description (string): Опис фільму.  
Quality (int): Якість фільму.  
Duration (string): Тривалість фільму.  
Country (string): Країна фільму.  
AgeRestriction (int): Вікове обмеження.  
DateOfPublish (int): Дата публікації фільму.  
Director (string): Режисер фільму.  
Actors (string): Актори фільму.  
TrailerUri (string): Посилання на трейлер фільму.  
Studios (string): Студії фільму (через кому).  
Genres (string): Жанри фільму (через кому).  
Categories (string): Категорії фільму (через кому).  
Selections (string)?: Додати фільм в існуючі вибірки (через кому) (необов'язково).  

- **POST /api/RegisterFilm/uploadedfilm** - Додати в базу данних лінк на файл в сховищі. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (application/json):

FilmId (int): Ідентифікатор фільму.  
FileName (string): Назва файлу.  
FileUri (string): URI файлу.  

- **POST /api/RegisterFilm/createselection** - Створити нову вибірку. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (multipart/form-data):
 
Name (string): Назва вибірки.  
Image (file): Картинка вибірки.  

## RatingController
- **GET /api/Rating** - Отримати рейтинг фільму. Результати: Ok(результат), NotFound
  
Параметри запиту:

filmId (int): Ідентифікатор фільму.

- **POST /api/Rating** - Додати нову оцінку фільму. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Параметри запиту:

filmId (int): Ідентифікатор фільму.  
rate (int): Оцінка фільму.  

## FilmsController
- **POST /api/Films/countpagesbyfiltersandsorting** - Отримати кількість сторінок за довільною кількість фільтрів та відсортувати. Результати: Ok(результат), NotFound  

Параметри запиту:  

pageSize (int): Розмір сторінки.  
sortByDate (string)?: Сортування за датою (необов'язкого). Варіанти: "asc", "desc"  
sortByPopularity (string)?: Сортування за популярністю (необов'язкого). Варіанти: "rating", "discussing"  

Поля тіла запиту (application/json):  

Довільна кількість фільтрів  
До прикладу:  

{  
    "Genres": ["Action", "Drama"],  
    "Tags": ["Classic", "Must Watch"],  
    "Studios": ["Warner Bros", "Paramount"]  
    "Selections": ["New year", "AI"]  
}  

- **GET /api/Films/byid** - Отримати фільм за ідентифікатором. Результати: Ok(результат), NotFound
  
Параметри запиту:

id (int): Ідентифікатор фільму.

- **POST /api/Films/byids** - Отримати масив фільмів за ідентифікаторами: Результати: Ok(результат), NotFound
  
Поля тіла запиту (application/json):

ids (масив int): Масив ідентифікаторів фільмів.

- **GET /api/Films/bytitle** - Отримати фільми за назвою. Результати: Ok(результат), NotFound

Параметри запиту:

title (string): Назва фільму.

- **POST /api/Films/byfiltersandsorting** - Отримати фільми за довільною кількість фільтрів та відсортувати. Результати: Ok(результат), NotFound
  
Параметри запиту:  

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  
sortByDate (string)?: Сортування за датою (необов'язкого). Варіанти: "asc", "desc"  
sortByPopularity (string)?: Сортування за популярністю (необов'язкого). Варіанти: "rating", "discussing"  

Поля тіла запиту (application/json):  

Довільна кількість фільтрів  
До прикладу:  

{  
    "Genres": ["Action", "Drama"],  
    "Tags": ["Classic", "Must Watch"],  
    "Studios": ["Warner Bros", "Paramount"]  
    "Selections": ["New year", "AI"]  
}  

- **GET /api/Films/getsas** - Отримати SaS токен для завантажування файлу зі сховища. Результати: Ok, BadRequest(текст помилки)
  
Параметри запиту:  

blobName (string): Назва файлу в сховищі.  

- **GET /api/Films/genres** - Отримати всі жанри. Результати: Ok(результат), NotFound  

- **GET /api/Films/categories** - Отримати всі теги. Результати: Ok(результат), NotFound  

- **GET /api/Films/studios** - Отримати всі студії. Результати: Ok(результат), NotFound  

- **GET /api/Films/selections** - Отримати всі вибірки. Результати: Ok(результат), NotFound  

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

#SeriesService  
##CommentsController  
- **GET /api/Comments** - Отримати всі коментарі. Результати: Ok(результат), NotFound  
  
Параметри запиту:  

seriesPartId (int): Ідентифікатор серії серіалу.  

- **POST /api/Comments** - Створити новий коментар. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Поля тіла запиту (application/json):  

SeriesPartId (int): Ідентифікатор серії серіалу.  
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

##RatingController
- **GET /api/Rating** - Отримати рейтинг серіалу. Результати: Ok(результат), NotFound
  
Параметри запиту:

seriesId (int): Ідентифікатор серіалу.

- **POST /api/Rating** - Додати нову оцінку серіалу. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Параметри запиту:

seriesId (int): Ідентифікатор серіалу.  
rate (int): Оцінка серіалу.  

##RegisterSeriesController
- **GET /api/RegisterSeries/getsas** - Отримати SaS токен для загрузки файла в сховище. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Параметри запиту:

blobName (string): Назва об'єкта Blob.

- **POST /api/RegisterSeries/registerseries** - Зареєструвати новий серіал. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Поля тіла запиту (multipart/form-data):

Poster (file): Постер серіалу.  
PosterPartOne (file)?: Перша частина розділеного постеру серіалу (необов'язково).  
PosterPartTwo (file)?: Друга частина розділеного постеру серіалу (необов'язково).  
PosterPartThree (file)?: Третя частина розділеного постеру серіалу (необов'язково).  
Title (string): Назва серіалу.  
Description (string): Опис серіалу.  
CountSeasons (string): Кількість сезонів серіалу.  
CountParts (string): Кількість серій серіалу.  
Quality (int): Якість серіалу.  
Duration (string): Тривалість серіалу.  
Country (string): Країна серіалу.  
AgeRestriction (int): Вікове обмеження.  
DateOfPublish (int): Дата публікації серіалу.  
Director (string): Режисер серіалу.  
Actors (string): Актори серіалу.  
TrailerUri (string): Посилання на трейлер серіалу.  
Studios (string): Студії серіалу (через кому).  
Genres (string): Жанри серіалу (через кому).  
Categories (string): Категорії серіалу (через кому).  
Selections (string)?: Додати серіалу в існуючі вибірки (через кому) (необов'язково).  

- **POST /api/RegisterSeries/registerseriespart** - Зареєструвати серію серіалу. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (application/json):

SeriesId (int): Ідентифікатор серіалу.  
Name (string): Назва серії.  
SeasonNumber (int): Номер сезону.  
PartNumber (int): Номер серії.  
Duration (string): Тривалість серії. 

- **POST /api/RegisterSeries/uploadedseriespart** - Додати в базу данних лінк на файл в сховищі. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (application/json):

Id (int): Ідентифікатор серіалу.  
FileName (string): Назва файлу.  
FileUri (string): URI файлу.  
Quality (string): Якість серії.  

- **POST /api/RegisterSeries/createselection** - Створити нову вибірку. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (multipart/form-data):
 
Name (string): Назва вибірки.  
Image (file): Картинка вибірки.  

##SeriesController
- **POST /api/Series/countpagesbyfiltersandsorting** - Отримати кількість сторінок за довільною кількість фільтрів та відсортувати. Результати: Ok(результат), NotFound  

Параметри запиту:  

pageSize (int): Розмір сторінки.  
sortByDate (string)?: Сортування за датою (необов'язкого). Варіанти: "asc", "desc"  
sortByPopularity (string)?: Сортування за популярністю (необов'язкого). Варіанти: "rating", "discussing"  

Поля тіла запиту (application/json):  

Довільна кількість фільтрів  
До прикладу:  

{  
    "Genres": ["Action", "Drama"],  
    "Tags": ["Classic", "Must Watch"],  
    "Studios": ["Warner Bros", "Paramount"]  
    "Selections": ["New year", "AI"]  
}  

- **GET /api/Series/byid** - Отримати серіал за ідентифікатором. Результати: Ok(результат), NotFound
  
Параметри запиту:

id (int): Ідентифікатор серіалу.

- **POST /api/Series/byids** - Отримати масив серіалів за ідентифікаторами: Результати: Ok(результат), NotFound
  
Поля тіла запиту (application/json):

ids (масив int): Масив ідентифікаторів серіалів.

- **GET /api/Series/bytitle** - Отримати серіал за назвою. Результати: Ok(результат), NotFound

Параметри запиту:

title (string): Назва серіалу.

- **POST /api/Series/byfiltersandsorting** - Отримати серіали за довільною кількість фільтрів та відсортувати. Результати: Ok(результат), NotFound
  
Параметри запиту:  

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  
sortByDate (string)?: Сортування за датою (необов'язкого). Варіанти: "asc", "desc"  
sortByPopularity (string)?: Сортування за популярністю (необов'язкого). Варіанти: "rating", "discussing"  

Поля тіла запиту (application/json):  

Довільна кількість фільтрів  
До прикладу:  

{  
    "Genres": ["Action", "Drama"],  
    "Tags": ["Classic", "Must Watch"],  
    "Studios": ["Warner Bros", "Paramount"]  
    "Selections": ["New year", "AI"]  
}  

- **GET /api/Series/getsas** - Отримати SaS токен для завантажування файлу зі сховища. Результати: Ok, BadRequest(текст помилки)
  
Параметри запиту:  

blobName (string): Назва файлу в сховищі.  

- **GET /api/Series/genres** - Отримати всі жанри. Результати: Ok(результат), NotFound  

- **GET /api/Series/categories** - Отримати всі теги. Результати: Ok(результат), NotFound  

- **GET /api/Series/studios** - Отримати всі студії. Результати: Ok(результат), NotFound  

- **GET /api/Series/selections** - Отримати всі вибірки. Результати: Ok(результат), NotFound

##SeriesPartController
- **GET /api/SeriesPart/getbyseriesid** - Отримати серії серіалу за ідентифікатором. Результати: Ok(результат), NotFound
  
Параметри запиту:  

id (int): Ідентифікатор серіалу.  

- **GET /api/SeriesPart/getbyid** - Отримати серію серіалу за ідентифікатором. Результати: Ok(результат), NotFound

Параметри запиту:  

Id (string): Ідентифікатор серії серіалу.  

- **GET /api/SeriesPart/getbyseason** - Отримати серії серіалу за сезоном. Результати: Ok(результат), NotFound
  
Параметри запиту:  

seriesId (int): Ідентифікатор серіалу.  
season (int): Номер сезону.  

#AnimeService  
##CommentsController  
- **GET /api/Comments** - Отримати всі коментарі. Результати: Ok(результат), NotFound, BadRequest  
  
Параметри запиту:  

animeId (int): Ідентифікатор аніме.  
animePartId (int): Ідентифікатор серії аніме.  

- **POST /api/Comments** - Створити новий коментар. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Поля тіла запиту (application/json):  

AnimeId (int)?: Ідентифікатор аніме (необов'язково).  
AnimePartId (int)?: Ідентифікатор серії аніме (необов'язково).  
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

##RatingController
- **GET /api/Rating** - Отримати рейтинг аніме. Результати: Ok(результат), NotFound
  
Параметри запиту:

animeId (int): Ідентифікатор аніме.

- **POST /api/Rating** - Додати нову оцінку аніме. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Параметри запиту:

animeId (int): Ідентифікатор аніме.  
rate (int): Оцінка серіалу.  

##RegisterAnimeController
- **GET /api/RegisterAnime/getsas** - Отримати SaS токен для загрузки файла в сховище. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Параметри запиту:

blobName (string): Назва об'єкта Blob.

- **POST /api/RegisterAnime/registeranime** - Зареєструвати нове аніме. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Поля тіла запиту (multipart/form-data):

Poster (file): Постер аніме.  
PosterPartOne (file)?: Перша частина розділеного постеру аніме (необов'язково).  
PosterPartTwo (file)?: Друга частина розділеного постеру аніме (необов'язково).  
PosterPartThree (file)?: Третя частина розділеного постеру аніме (необов'язково).  
Title (string): Назва аніме.  
Description (string): Опис аніме.  
CountSeasons (string)?: Кількість сезонів аніме (необов'язково).  
CountParts (string)?: Кількість серій аніме (необов'язково).  
Quality (int): Якість аніме.  
Duration (string): Тривалість аніме.  
Country (string): Країна аніме.  
AgeRestriction (int): Вікове обмеження.  
DateOfPublish (int): Дата публікації аніме.  
Director (string): Режисер аніме.  
Actors (string): Актори аніме.  
TrailerUri (string): Посилання на трейлер аніме.  
Studios (string): Студії аніме (через кому).  
Genres (string): Жанри аніме (через кому).  
Categories (string): Категорії аніме (через кому).  
Selections (string)?: Додати аніме в існуючі вибірки (через кому) (необов'язково).  

- **POST /api/RegisterAnime/registeranimepart** - Зареєструвати серію аніме. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (application/json):

AnimeId (int): Ідентифікатор аніме.  
SeasonNumber (int): Номер сезону.  
PartNumber (int): Номер серії.  
Duration (string): Тривалість серії. 

- **POST /api/RegisterAnime/uploadedanimepart** - Додати в базу данних лінк на файл в сховищі. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (application/json):

Id (int): Ідентифікатор серії аніме.  
FileName (string): Назва файлу.  
FileUri (string): URI файлу.  

- **POST /api/RegisterAnime/uploadedanime** - Додати в базу данних лінк на файл в сховищі. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (application/json):

Id (int): Ідентифікатор аніме.  
FileName (string): Назва файлу.  
FileUri (string): URI файлу.  

- **POST /api/RegisterSeries/createselection** - Створити нову вибірку. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (multipart/form-data):
 
Name (string): Назва вибірки.  
Image (file): Картинка вибірки.  

##AnimeController
- **POST /api/Anime/countpagesbyfiltersandsorting** - Отримати кількість сторінок за довільною кількість фільтрів та відсортувати. Результати: Ok(результат), NotFound  

Параметри запиту:  

pageSize (int): Розмір сторінки.  
sortByDate (string)?: Сортування за датою (необов'язкого). Варіанти: "asc", "desc"  
sortByPopularity (string)?: Сортування за популярністю (необов'язкого). Варіанти: "rating", "discussing"  

Поля тіла запиту (application/json):  

Довільна кількість фільтрів  
До прикладу:  

{  
    "Genres": ["Action", "Drama"],  
    "Tags": ["Classic", "Must Watch"],  
    "Studios": ["Warner Bros", "Paramount"]  
    "Selections": ["New year", "AI"]  
}  

- **GET /api/Anime/byid** - Отримати аніме за ідентифікатором. Результати: Ok(результат), NotFound
  
Параметри запиту:

id (int): Ідентифікатор серіалу.

- **POST /api/Anime/byids** - Отримати масив аніме за ідентифікаторами: Результати: Ok(результат), NotFound
  
Поля тіла запиту (application/json):

ids (масив int): Масив ідентифікаторів серіалів.

- **GET /api/Anime/bytitle** - Отримати аніме за назвою. Результати: Ok(результат), NotFound

Параметри запиту:

title (string): Назва аніме.

- **POST /api/Anime/byfiltersandsorting** - Отримати аніме за довільною кількість фільтрів та відсортувати. Результати: Ok(результат), NotFound
  
Параметри запиту:  

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  
sortByDate (string)?: Сортування за датою (необов'язкого). Варіанти: "asc", "desc"  
sortByPopularity (string)?: Сортування за популярністю (необов'язкого). Варіанти: "rating", "discussing"  

Поля тіла запиту (application/json):  

Довільна кількість фільтрів  
До прикладу:  

{  
    "Genres": ["Action", "Drama"],  
    "Tags": ["Classic", "Must Watch"],  
    "Studios": ["Warner Bros", "Paramount"]  
    "Selections": ["New year", "AI"]  
}  

- **GET /api/Anime/getsas** - Отримати SaS токен для завантажування файлу зі сховища. Результати: Ok, BadRequest(текст помилки)
  
Параметри запиту:  

blobName (string): Назва файлу в сховищі.  

- **GET /api/Anime/genres** - Отримати всі жанри. Результати: Ok(результат), NotFound  

- **GET /api/Anime/categories** - Отримати всі теги. Результати: Ok(результат), NotFound  

- **GET /api/Anime/studios** - Отримати всі студії. Результати: Ok(результат), NotFound  

- **GET /api/Anime/selections** - Отримати всі вибірки. Результати: Ok(результат), NotFound

##AnimePartController
- **GET /api/AnimePart/getbyanimeid** - Отримати серії аніме за ідентифікатором. Результати: Ok(результат), NotFound
  
Параметри запиту:  

animeId (int): Ідентифікатор аніме.  

- **GET /api/AnimePart/getbyid** - Отримати серію аніме за ідентифікатором. Результати: Ok(результат), NotFound

Параметри запиту:  

Id (string): Ідентифікатор серії аніме.  

- **GET /api/AnimePart/getbyseason** - Отримати серії аніме за сезоном. Результати: Ok(результат), NotFound
  
Параметри запиту:  

animeId (int): Ідентифікатор аніме.  
season (int): Номер сезону.  

#CartoonService
##CommentsController  
- **GET /api/Comments** - Отримати всі коментарі. Результати: Ok(результат), NotFound, BadRequest  
  
Параметри запиту:  

cartoonId (int): Ідентифікатор мультфільму.  
cartoonPartId (int): Ідентифікатор серії мультфільму.  

- **POST /api/Comments** - Створити новий коментар. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Поля тіла запиту (application/json):  

CartoonId (int)?: Ідентифікатор мультфільму (необов'язково).  
CartoonPartId (int)?: Ідентифікатор серії мультфільму (необов'язково).  
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

##RatingController
- **GET /api/Rating** - Отримати рейтинг мультфільму. Результати: Ok(результат), NotFound
  
Параметри запиту:

cartoonId (int): Ідентифікатор мультфільму.

- **POST /api/Rating** - Додати нову оцінку мультфільму. Результати: Ok, BadRequest(текст помилки), Unauthorized  
  
Параметри запиту:

cartoonId (int): Ідентифікатор мультфільму.  
rate (int): Оцінка серіалу.  

##RegisterCartoonController
- **GET /api/RegisterCartoon/getsas** - Отримати SaS токен для загрузки файла в сховище. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Параметри запиту:

blobName (string): Назва об'єкта Blob.

- **POST /api/RegisterCartoon/registercartoon** - Зареєструвати новий мультфільм. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Поля тіла запиту (multipart/form-data):

Poster (file): Постер мультфільму.  
PosterPartOne (file)?: Перша частина розділеного постеру мультфільму (необов'язково).  
PosterPartTwo (file)?: Друга частина розділеного постеру мультфільму (необов'язково).  
PosterPartThree (file)?: Третя частина розділеного постеру мультфільму (необов'язково).  
Title (string): Назва мультфільму.  
Description (string): Опис мультфільму.  
CountSeasons (string)?: Кількість сезонів мультфільму (необов'язково).  
CountParts (string)?: Кількість серій мультфільму (необов'язково).  
Quality (int): Якість мультфільму.  
Duration (string): Тривалість мультфільму.  
Country (string): Країна мультфільму.  
AgeRestriction (int): Вікове обмеження.  
DateOfPublish (int): Дата публікації мультфільму.  
Director (string): Режисер мультфільму.  
TrailerUri (string): Посилання на трейлер мультфільму.  
Studios (string): Студії мультфільму (через кому).  
Genres (string): Жанри мультфільму (через кому).  
Categories (string): Категорії мультфільму (через кому).  
Selections (string)?: Додати мультфільм в існуючі вибірки (через кому) (необов'язково).  

- **POST /api/RegisterCartoon/registercartoonpart** - Зареєструвати серію мультфільму. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (application/json):

CartoonId (int): Ідентифікатор мультфільму.  
SeasonNumber (int): Номер сезону.  
PartNumber (int): Номер серії.  
Duration (string): Тривалість серії. 

- **POST /api/RegisterCartoon/uploadedanimepart** - Додати в базу данних лінк на файл в сховищі. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (application/json):

Id (int): Ідентифікатор серії мультфільму.  
FileName (string): Назва файлу.  
FileUri (string): URI файлу.  

- **POST /api/RegisterCartoon/uploadedcartoon** - Додати в базу данних лінк на файл в сховищі. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (application/json):

Id (int): Ідентифікатор мультфільму.  
FileName (string): Назва файлу.  
FileUri (string): URI файлу.  

- **POST /api/RegisterCartoon/createselection** - Створити нову вибірку. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  
  
Поля тіла запиту (multipart/form-data):
 
Name (string): Назва вибірки.  
Image (file): Картинка вибірки.  

##CartoonsController
- **POST /api/Cartoons/countpagesbyfiltersandsorting** - Отримати кількість сторінок за довільною кількість фільтрів та відсортувати. Результати: Ok(результат), NotFound  

Параметри запиту:  

pageSize (int): Розмір сторінки.  
sortByDate (string)?: Сортування за датою (необов'язкого). Варіанти: "asc", "desc"  
sortByPopularity (string)?: Сортування за популярністю (необов'язкого). Варіанти: "rating", "discussing"  

Поля тіла запиту (application/json):  

Довільна кількість фільтрів  
До прикладу:  

{  
    "Genres": ["Action", "Drama"],  
    "Tags": ["Classic", "Must Watch"],  
    "Studios": ["Warner Bros", "Paramount"]  
    "Selections": ["New year", "AI"]  
}  

- **GET /api/Cartoons/byid** - Отримати мультфільм за ідентифікатором. Результати: Ok(результат), NotFound
  
Параметри запиту:

id (int): Ідентифікатор серіалу.

- **POST /api/Cartoons/byids** - Отримати масив мультфільмів за ідентифікаторами: Результати: Ok(результат), NotFound
  
Поля тіла запиту (application/json):

ids (масив int): Масив ідентифікаторів серіалів.

- **GET /api/Cartoons/bytitle** - Отримати мультфільм за назвою. Результати: Ok(результат), NotFound

Параметри запиту:

title (string): Назва мультфільму.

- **POST /api/Cartoons/byfiltersandsorting** - Отримати мультфільми за довільною кількість фільтрів та відсортувати. Результати: Ok(результат), NotFound
  
Параметри запиту:  

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  
sortByDate (string)?: Сортування за датою (необов'язкого). Варіанти: "asc", "desc"  
sortByPopularity (string)?: Сортування за популярністю (необов'язкого). Варіанти: "rating", "discussing"  

Поля тіла запиту (application/json):  

Довільна кількість фільтрів  
До прикладу:  

{  
    "Genres": ["Action", "Drama"],  
    "Tags": ["Classic", "Must Watch"],  
    "Studios": ["Warner Bros", "Paramount"]  
    "Selections": ["New year", "AI"]  
}  

- **GET /api/Cartoons/getsas** - Отримати SaS токен для завантажування файлу зі сховища. Результати: Ok, BadRequest(текст помилки)
  
Параметри запиту:  

blobName (string): Назва файлу в сховищі.  

- **GET /api/Cartoons/genres** - Отримати всі жанри. Результати: Ok(результат), NotFound  

- **GET /api/Cartoons/categories** - Отримати всі теги. Результати: Ok(результат), NotFound  

- **GET /api/Cartoons/studios** - Отримати всі студії. Результати: Ok(результат), NotFound  

- **GET /api/Cartoons/selections** - Отримати всі вибірки. Результати: Ok(результат), NotFound

##CartoonPartController
- **GET /api/CartoonPart/getbycartoonid** - Отримати серії мультфільму за ідентифікатором. Результати: Ok(результат), NotFound
  
Параметри запиту:  

cartoonId (int): Ідентифікатор мультфільму.  

- **GET /api/CartoonPart/getbyid** - Отримати серію мультфільму за ідентифікатором. Результати: Ok(результат), NotFound

Параметри запиту:  

Id (string): Ідентифікатор серії мультфільму.  

- **GET /api/CartoonPart/getbyseason** - Отримати серії аніме за сезоном. Результати: Ok(результат), NotFound
  
Параметри запиту:  

cartoonId (int): Ідентифікатор мультфільму.  
season (int): Номер сезону.  

# UserService
## AuthController
- **POST /api/Auth/register** - Створити нового користувача. Результати: Ok, BadRequest(текст помилки)  

Поля тіла запиту (application/json):  

Email (string): Пошта користувача.  
Password (string): Пароль користувача.  

- **POST /api/Auth/authenticate** - Аутентифікувати користувача. Результати: Ok, BadRequest(текст помилки), NotFound, Forbidden  

Поля тіла запиту (application/json):  

Email (string): Пошта користувача.  
Password (string): Пароль користувача.  

- **PUT /api/Auth/refreshjwt** - Оновити JWT токен. Результати: Ok, BadRequest(текст помилки)  

- **DELETE /api/Auth/logout** - Вихід користувача з системи. Результат: Ok, BadRequest(текст помилки), Unauthorized  

- **POST /api/Auth/google** - Аутентифікувати користувача через Google. Результати: Ok, BadRequest(текст помилки)  

Поля тіла запиту (multipart/form-data):  

token (string): Токен доступу виданий Google.  

- **POST /api/Auth/migrateuser** - Додати користувачу Google аутентифікацію. Результати: Ok, BadRequest(текст помилки), NotFound, Conflict  

Параметри запиту:  

token (string): Токен користувача.  

- **POST /api/Auth/emailconfirm** - Підтвердити пошту користувача. Результати: Ok, BadRequest(текст помилки), Conflict  

Параметри запиту:  

token (string): Токен користувача.  

## UsersController
- **GET /api/Users/byid** - Отримати користувача за ідентифікатором. Результати: Ok(результат), NotFound  

Параметри запиту:

id (int)?: Ідентифікатор користувача (необов'язково).

- **POST /api/Users/byids** - Отримати користувачів за ідентифікаторами: Результати: Ok(результат), NotFound  

Поля тіла запиту (application/json):

ids (масив int): Масив Ідентифікаторів користувача.

- **PUT /api/Users/avatar** - Змінити аватар користувача. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Поля тіла запиту (multipart/form-data):

avatar (file): Аватар користувача.  

- **POST /api/Users/sendemailchangepassword** - Надіслати лист користувачу на пошту для зміни пароля. Результати: Ok, BadRequest(текст помилки), NotFound  

Поля тіла запиту (multipart/form-data):  

email (string): Поштовий адрес користувача.  

- **PUT /api/Users/changepassword** - Змінити пароль користувача. Результати: Ok, BadRequest(текст помилки), NotFound  

Параметри запиту:  

token (string): Токен виданий сервісом для зміни пароля.  

Поля тіла запиту (multipart/form-data):  

password (string): Новий пароль.  

- **POST /api/Users/sendemailchangeemailaddress** - Надіслати лист користувачу на пошту для зміни пошти. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Поля тіла запиту (application/json):  

password (string): Пароль користувача.  
email (string): Новий поштовий адрес користувача.  

- **PUT /api/Users/changeemail** - Змінити пошту користувача. Результати: Ok, BadRequest(текст помилки)  

Параметри запиту:  

token (string): Токен виданий сервісом для зміни пошти.  

- **PUT /api/Users/changenusername** - Змінити ім'я користувача. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Параметри запиту:  

username (string): Нове ім'я користувача.

## HistoryController
- **GET /api/History** - Отримати історії користувача. Результати: Ok(результат), NotFound  

- **POST /api/History** - Додати історію користувачу. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Поля тіла запиту (application/json):  

MediaWithType (object){  
MediaId (int): Ідентифікатор медія.  
MediaTypeId (int): Ідентифікатор типу медіа.  
}  
PartNumber (int)?: Номер частини.  
SeasonNumber (int)?: Номер сезона.  

## BookMarksController
- **GET /api/BookMarks** - Отримати закладки користувача. Результати: Ok(результат), NotFound, Unauthorized  

- **POST /api/BookMarks** - Додати закладку користувачу. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Поля тіла запиту (application/json):  

MediaId (int): Ідентифікатор медіа.  
MediaTypeId (int): Ідентифікатор типу медіа.  

# TransactionService
## FundraisingController  
- **GET /api/Fundraising** - Отримати відпагіновані збори. Результати: Ok(результат), NotFound  

Параметри запиту:  

pageNumber (int): Номер сторінки.  
pageSize (int): Розмір сторінки.  

- **POST /api/Fundraising** - Додати збір. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Поля тіла запиту (multipart/form-data):  

Name (string): Назва збору.  
Description (string): Опис збору.  
Image (file): Картинка збору.  
FundraisingUrl (string): URL на збір.  

- **PUT /api/Fundraising** - Змінити статус збору. Результати: Ok, BadRequest(текст помилки), Forbidden, Unauthorized  

Параметри запиту:  

fundraisingId (int): Ідентифікатор збору.  

## SubscriptionController
- **GET /api/Subscription/subscriptions** - Отримати підписки користувача. Результати: Ok(результат), NotFound, Unauthorized  

- **POST /api/Subscription/subscribe** - Додати нову підписку користувачу. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Поля тіла запиту (application/json):  

OrderId (string): Ідентифікатор замовлення. 
SubscriptionId (string): Ідентифікатор підписки.  

- **PUT /api/Subscription/changestatus** - Змінити статус підписки користувача. Результати: Ok, BadRequest(текст помилки), Unauthorized  

Параметри запиту:  

reason (string): Причина зміни статусу.  
