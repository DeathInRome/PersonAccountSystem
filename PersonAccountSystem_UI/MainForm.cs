using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Windows.Forms;
using ClassLib.Model;

namespace PersonAccountSystem_UI
{
    public partial class MainForm : Form
    {
        public PASContext db; ////Подключения к БД, которое осуществляется через контекстный класс 

        List<Employee> employees;

        public MainForm() //Метод главной формы
        {
            InitializeComponent();
            db = new PASContext(); //Создание нового подключения к БД
            RefreshUpdate();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "pASConnectionDataSet.Employees". При необходимости она может быть перемещена или удалена.
            this.employeesTableAdapter.Fill(this.pASConnectionDataSet.Employees);

        }

        /// <summary>
        /// Метод для обновления данных в сетке, который перезаписывает каждый раз связанные данные 
        /// </summary>
        private void RefreshUpdate()
        {
            db.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
            //// Загрузить всех пользователей и связанные с ними отпуска
            employees = db.Employees
           .Include(v => v.Vocations)
           .ToList();
            dataGridView1.DataSource = employees;
        }


        //Добавить нового сотрудника
        private void button1_Click(object sender, EventArgs e)
        {
            EmployeeForm ef = new EmployeeForm(); //Создаем новую форму для заполнения данных сотрудника
            DialogResult result = ef.ShowDialog(this); //Результатом диалога при нажатии клавиши (button1_Click) возвращается событие в виде отображения формы "ef"
            if (result == DialogResult.Cancel)//Если будет нажата отмена, то форма будет закрыта 
            { return; }

            Employee empl = new Employee(); //Создаем нового сотрудника. ID в таком случае будет добавлено автоматически 
            empl.First_Name = ef.textBox1.Text;  //Имя        //В класс сотрудника пихается значение текстбокса из экземляра формы ef
            empl.Second_Name = ef.textBox2.Text; //Фамилия
            empl.Education = ef.comboBox1.SelectedItem.ToString();//Образование
            empl.Position = ef.comboBox2.SelectedItem.ToString(); //Должность
            empl.Date_Of_Birth = (int)ef.numericUpDown1.Value; //Возраст


            db.Employees.Add(empl); //Передать через узел в БД
            db.SaveChanges(); //Сохранить изменения
            RefreshUpdate();
            MessageBox.Show("Новый сотрудник добавлен");
          
        }

        //Изменить выбранного сотрудника
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)//Если выбранная строка имеет не нулевое значение, то выполнить
            {
                int index = dataGridView1.SelectedRows[0].Index; //Задаем индексу нулевое значение элемента списка
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id); //Какая-то ахинея
                if (converted == false)
                { return; }
                Employee empl = db.Employees.Find(id); //Выбрать сотрудника по заданному id при выборе строки


                EmployeeForm ef = new EmployeeForm(); //Создаем новую форму для заполнения данных сотрудника
                ef.textBox1.Text = empl.First_Name; //Перегоняем данные поля карточки сотрудника из БД на текущую форму
                ef.textBox2.Text = empl.Second_Name;
                ef.comboBox1.SelectedItem = empl.Education;
                ef.comboBox2.SelectedItem = empl.Position;
                ef.numericUpDown1.Value = empl.Date_Of_Birth;

                DialogResult result = ef.ShowDialog(this);//Создаем новую форму для заполнения данных сотрудника

                if (result == DialogResult.Cancel)//Если будет нажата отмена, то форма будет закрыта 
                { return; }

                empl.First_Name = ef.textBox1.Text;  //Имя        //В класс сотрудника пихается значение текстбокса из экземляра формы ef
                empl.Second_Name = ef.textBox2.Text; //Фамилия
                empl.Education = ef.comboBox1.SelectedItem.ToString();//Образование
                empl.Position = ef.comboBox2.SelectedItem.ToString(); //Должность
                empl.Date_Of_Birth = (int)ef.numericUpDown1.Value; //Возраст

                db.SaveChanges(); //Сохраняем изменения в БД
                dataGridView1.Refresh(); // Обновляем грид
                MessageBox.Show("Сотрудник был изменен обновлен");
                

            }
            else { MessageBox.Show("Неверное входное значение"); }


        }

        //Удалить выбранного сотрудника
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)//Если выбранная строка имеет не нулевое значение, то выполнить
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Employee empl = db.Employees.Find(id);
                empl.Vocations.Clear(); //Очистим предварительно ссылки на отпуска
                db.Employees.Remove(empl);
                db.SaveChanges();
                dataGridView1.Refresh();
                MessageBox.Show("Сотрудник был удален");

            }
        }

        //Добавить отпуск
        private void button4_Click(object sender, EventArgs e)
        {
            VocationForm vf = new VocationForm(); //Создать новую форму
            DialogResult result = vf.ShowDialog(this); //Результатом нажатия, открывается созданная форма
            if (result == DialogResult.Cancel)//Если будет нажата отмена, то форма будет закрыта 
            { return; }

            Vocation v = new Vocation(); //Создается новая запись
            v.Reason = vf.textBox1.Text;
            v.DateBegin = vf.dateTimePicker1.Value;
            v.DateEnd = vf.dateTimePicker2.Value;
            v.EmployeeId = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

            db.Vocations.Add(v);
            db.SaveChanges();
            dataGridView2.Refresh();
            MessageBox.Show("Новый отпуск добавлен");
        }


        //Изменить значения отпуска
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)//Если выбранная строка имеет не нулевое значение, то выполнить
            {
                int index = dataGridView2.SelectedRows[0].Index; //Задаем индексу нулевое значение элемента списка
                int id = 0;
                bool converted = Int32.TryParse(dataGridView2[0, index].Value.ToString(), out id); //Какая-то ахинея
                if (converted == false)
                { return; }
                Vocation v = db.Vocations.Find(id); //Выбрать отпуск по заданному id при выборе строки

                VocationForm vf = new VocationForm();

                vf.textBox1.Text = v.Reason;
                vf.dateTimePicker1.Value = v.DateBegin;
                vf.dateTimePicker2.Value = v.DateEnd;

                DialogResult result = vf.ShowDialog(this);//Создаем новую форму для заполнения данных 

                v.Reason = vf.textBox1.Text;
                v.DateBegin = vf.dateTimePicker1.Value;
                v.DateEnd = vf.dateTimePicker2.Value;

                db.SaveChanges(); //Сохраняем изменения в БД
                dataGridView2.Refresh(); // Обновляем грид
                MessageBox.Show("Отпуск был обновлен");
            }
            else
            {
                MessageBox.Show("Неверное входное значение");
            }
        }

        //Удалить отпуск
        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)//Если выбранная строка имеет не нулевое значение, то выполнить
            {
                int index = dataGridView2.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView2[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Vocation v = db.Vocations.Find(id);
                db.Vocations.Remove(v);
                db.SaveChanges();
                dataGridView2.Refresh();
                MessageBox.Show("Отпуск был удален");

            }
            else
            {
                MessageBox.Show("Неверное входное значение");
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            List<Vocation> vocations = employees.SelectMany(v => v.Vocations) //Выбрать из списка сотрудников их отпуска
               .ToList(); //Добавить в списка

            for (int k = 0; k < vocations.Count; k++)
            {
                if (vocations[k].EmployeeId != index)
                {
                    vocations.RemoveAt(k);
                    k--;
                }
            }

            dataGridView2.DataSource = vocations; //Добавить в грид
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var agemass = from ages in db.Employees //Выводим список возрастов
                          where ages.Date_Of_Birth > 0
                          select ages;

            double midage = agemass.Average(m => m.Date_Of_Birth); //Считаем среднее значение
            string result = "Средний возраст сотрудников " + midage + " лет";

            MessageBox.Show(result);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var idmass = from id in db.Employees //Делаем запрос на вывод списка всех ID сотрудников
                         where id.EmployeeId >= 0
                         select id;
            int numID = idmass.AsQueryable().Count(); //Получаем количество сотрудников

            string result = "Общее количество сотрудников " + numID + "человек";

            MessageBox.Show(result);
        }
    }
}
