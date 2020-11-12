/// <summary>
/// Класс миграции создается путем использования команды enable-migrations в консоли диспетчера пакетов. Нужен для автоматической генерации базы данных
/// на основе созданных сущностей. В общем случае, миграция - процесс сапостовления объектной модели БД и и той БД, что хранится непосредственно на сервере
/// и создание БД в автоматическом режиме. Команда update-database создает БД, если ее нет
/// </summary>
namespace ClassLib.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ClassLib.Model.PASContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true; //Данная команда позволяет создавать БД в автоматическом режиме, пока стоит включен режим true (в режиме отладки).
        }

        protected override void Seed(ClassLib.Model.PASContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
