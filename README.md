# Cube
## Что за проект?
Это тренировочный проект реализации собственного мессенджера
# Установка
* Выберете папку в которую скачаете проект и используйте команду git clone [сыллка на репозиторий]
* Далее в папке ../Cube/Cube.Api создайте файл appsettings.personal.json где будут храниться персональные данные для JWT токенов и подключения к базе данных 
Структура данного файла:
{
  "Auth": {
    "Secrete": "*"
  },
  "ConnectionStrings": {
    "DefaultConnection": "*"
  }
}
* Примечание в зависимости от того с какой базой данных вы работаете нужно скачать необходимые инструменты и поменять подключение в файле: ..\Cube\Cube.Api\Configuration\ConfigurationExtensions.cs

замените расширение:

	public static void ConfigureRepository(this WebApplicationBuilder builder)
    	{
        	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
	
        	builder.Services.AddDbContext<CubeDbContext>(
            		options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        	builder.Services.AddScoped<IRepositoryWrapper>(
            		options => new RepositoryWrapper(options.GetRequiredService<CubeDbContext>()));
    	}
	
 В данном примере указан способ подключения к базе данных SQLite
		
* Далее нужно настроить миграции для этого есть 2 способа
* 1:
В Visual Studio View->Other Windows->Package Manager Console
В области Package Manager Console поставте Default Project = Cube.Repository 
Далее установите в качестве запускаемого проекта Cube.Api (right-cklick on Cube.Api in Solution Explorer -> Set as Startup Project)
и в данной консоле выполните команду: update-database
* 2:
Зайдите в папку ../Cube/Cube.Repository вызовите консоль в данной папке и выполните команду: dotnet ef database update

## Какие функции будут доступны?
* Написать/прочитать сообщение
* Добавить/удалить друзей
* Переслать файл
* ...
## Документация
[click here](https://docs.google.com/document/d/1uuU6nUzgCSoHaY29WP4AvgfKLC9vyQFm7X9-kLy-8Ko/edit?usp=sharing)
https://docs.google.com/document/d/1uuU6nUzgCSoHaY29WP4AvgfKLC9vyQFm7X9-kLy-8Ko/edit?usp=sharing
### Взаимодействие с БД





