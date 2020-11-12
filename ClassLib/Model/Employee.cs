using System.Collections.Generic;

namespace ClassLib.Model
{
    /// <summary>
    /// Класс описывающий сотрудника
    /// </summary>
   public class Employee 
    {
        public int EmployeeId { get; set; } //Поле идентификатора сотрудника
        public string First_Name { get; set; } //Поле для имени
        public string Second_Name { get; set; } //Поле для фамилии
        public int Date_Of_Birth { get; set; } //Поле для даты рождения
        public string Education { get; set; } //Поле для образования
        public string Position { get; set; } //Поле для должности
       
        public override string ToString() //Поле для того чтобы в случае обращения вернуть сообщение, содержащее текущее значение полей класса объекта
        {
            return base.ToString();
        }

        //Навигационное свойство, связь сотрудника с отпусками (сотрудник - отпуска/один ко многому) 
        public virtual ICollection<Vocation> Vocations { get; set; }
        public Employee() //Конструктор для создания списка отпусков для сотрудника
        {
            Vocations = new List<Vocation>();
        }

    }
}
