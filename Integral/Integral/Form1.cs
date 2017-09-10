using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Integral
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Добавление checked в checkedListBox1 (Выбора метода)
            checkedListBox1.Items.Add("Метод прямоугольников");
            checkedListBox1.Items.Add("Метод трапеций");
            checkedListBox1.Items.Add("Метод Симпсона");
        }

        //Режим оидания программы (true - вкл, false - выкл): 
        //Смена курсора на песочные часы и запрещение кликать куда-либо (при permission = true) и наоборот (при permission = false)
        public void wait(bool permission)
        {
            this.UseWaitCursor = permission;
            textBox1.Enabled = !permission;
            textBox2.Enabled = !permission;
            textBox3.Enabled = !permission;
            textBox4.Enabled = !permission;
            checkBox1.Enabled = !permission;
            checkedListBox1.Enabled = !permission;
            button1.Enabled = !permission;
        }

        //Действия при нажатии кнопки
        private void button1_Click(object sender, EventArgs e)
        {
            //Границы и погрешность
            double a = 0, b = 0, eps = 0;
            //Функция
            string function = "";
            //Создаём булевый массив, который будет показывать, какие из методов были выбраны. На данный момент состоит из элементов "false"
            bool[] indexes_are_checked = new bool[checkedListBox1.Items.Count];
            //Массив, в котором будут содержаться результаты вычислений
            double[] results = new double[checkedListBox1.CheckedItems.Count];

            //Прячем сообщение об ошибке "Введены не все необходимые входные данные", если оно есть
            label15.Hide();
            //Удаляем выведенный результат предыдущих вычислений, если он есть
            label12.Text = "";
            label13.Text = "";
            label14.Text = "";
            //Включаем режим ожидания отклика программы
            wait(true);

            //#1)
            //Работаем с первой частью формы (ввод функции, границ и погрешности). Если функция введена
            if (!String.IsNullOrEmpty(textBox3.Text))
            {
                //Получаем границы и погрешность в виде double
                try
                {
                    a = Convert.ToDouble(textBox2.Text);
                    b = Convert.ToDouble(textBox1.Text);
                    eps = Convert.ToDouble(textBox4.Text);
                    //Если a>b, выводим сообщение об ошибке
                    if (a > b)
                    {
                        label15.Show();
                    }
                }
                //Исключение в случае, если a,b или eps не могут быть конвертированы из string в double:
                catch (System.FormatException ex)
                {
                    label15.Show();
                }
                //Получаем функцию в виде строки
                function = textBox3.Text;
            }
            //Если функция не введена, выводим сообщение об ошибке
            else
                label15.Show();

            //#2)
            //Теперь работаем со второй частью формы. Если сообщение об ошибке не было выведено
            if (label15.Visible == false)
            {
                //Если флаг "Все" не был выбран
                if (checkBox1.Checked == false)
                {
                    //Если были выбраны какие-то методы
                    if (checkedListBox1.CheckedItems.Count > 0)
                    {
                        //Перебираем индексы всех возможных методов
                        for (int i = 0; i < checkedListBox1.Items.Count; i++)
                        {
                            //Если флаг i-ого метода был выбран, присваеваем соответствующему ему элементу массива значение "true"
                            if (checkedListBox1.CheckedIndices.Contains(i))
                            {
                                indexes_are_checked[i] = true;
                            }
                        }
                    }
                    //Если ничего не быбрано - выдаём сообщение об ошибке
                    else
                        label15.Show();
                }
                //Если флаг "Все" был выбран, заполняем весь массив элементами True
                else
                {
                    for (int i = 0; i < indexes_are_checked.Length; i++)
                    {
                        indexes_are_checked[i] = true;
                    }
                }
                //Передаём функцию, границы, массив элементов и bool-значение (выбран ли флаг "Все") клиенту. Он вернёт результаты вычислений
                results = Program.Client(checkBox1.Checked, indexes_are_checked, a, b, eps, function);
            }

            //#3)
            //Если сообщение об ошибке не было выведено, результаты получены. Выводим их
            if(label15.Visible == false)
            {
                //Индикатор индекса в массиве results
                int j = 0;
                //Для всех методов
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    //Если метод был выбран и мы ожидаем результат вычисления этим методом
                    if (indexes_are_checked[i] == true)
                    {
                        //Если этот метод - первый
                        if (i == 0)
                        {
                            label12.Text = results[j].ToString();
                        }
                        //Если этот метод - второй
                        else if (i == 1)
                        {
                            label13.Text = results[j].ToString();
                        }
                        //Если этот метод - третий
                        else if (i == 2)
                        {
                            label14.Text = results[j].ToString();
                        }
                        //Если метод был выбран, мы уже вывели результат его выполнения. Переходим к следующему элементу массива результатов
                        j += 1;
                    }
                }
            }
            //Выключаем режим ожидания отклика программы
            wait(false);
        }

        //Действия при клике на "Все"
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Если поставили отметку "Все", скрываем панель выбора метода
            if (checkBox1.Checked == true)
            {
                checkedListBox1.Hide();
            }
            //Если сняли отметку, снова показываем
            else
            {
                checkedListBox1.Show();
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
