using System.Data.Entity;

namespace ClassLib.Model
{
    /// <summary>
    /// Основной класс, отвечающий за взаимодействие БД с сущностями
    /// </summary>
    public class PASContext : DbContext
    {
        public PASContext() : base("PASConnection") { }//Конструктор класса наследует конструктор DbContext, и в качестве параметра принимает строку подключения к БД

        //Создание объектов сущностей, с которыми непосредственно и взаимодействует БД 
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vocation> Vocations { get; set; }

    }
}
