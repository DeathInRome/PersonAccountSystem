using System;


namespace ClassLib.Model
{
    /// <summary>
    /// Класс по учету отпусков сотрудника
    /// </summary>
    public class Vocation 
    {
        public int VocationId { get; set; } //Идентификатор отпуска сотрудника
        public string Reason { get; set; } //Причина отпуска, комментарий
        public DateTime DateBegin { get; set; } //Дата начала
        public DateTime DateEnd { get; set; } //Дата окончания 
   
        public int? EmployeeId { get; set; } //В качестве внешнего ключа 

       
        public virtual Employee Employee { get; set; } //Виртуальное свойство (ссылка),  для связывания с сотрудником. (Один ко многому). Через него можно путем перебора списка объектов фактически обращаться к классу, с которым установлена связь, а также "подсасывает" весь объект класса сотрудник при создании отпуска 
    }
}
