using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.ComponentModel;
using ParserMathExpressions;
using RectangleMethod;
using TrapezeMethod;
using SimpsonMethod;
using Deriv;
using Func_calc;

namespace Integral
{
    static class Program
    {
        //Основной метод клиента
        //Метод статичный, т.е. может вызываться даже без создания экземпляра класса
        public static double[] Client(bool All, bool[] indexes_are_checked, double a, double b, double eps, string function)
        {
            //Массив результатов
            double[] results;

            //Парсер
            Parser parser = new Parser();
            //Парсим функцию
            List<string> parsed_func = parser.Parse(function);

            //Если был выбран флаг "Все"
            if (All)
            {
                //Размерность - число всех методов (т.к. выбраны все)
                results = new double[indexes_are_checked.Length];
                //Считаем всеми методами с использованием контейнера
                Container_Using(parsed_func, a, b, eps, ref results);
            }
            //Если флаг "Все" не был выбран
            else
            {
                //Размерность - число выбранных методов
                results = new double[indexes_are_checked.Count(x => x == true)];
                //Индикатор индекса в массиве results
                int i = 0;
                //Если выбран метод прямоугольников
                if (indexes_are_checked[0] == true)
                {
                    //Считаем интеграл методом прямоугольников
                    results[i] = Rectangle(parsed_func, a, b, eps);
                    i++;
                }
                //Если выбран метод трапеций
                if (indexes_are_checked[1] == true)
                {
                    //Считаем интеграл методом трапеций
                    results[i] = Trapeze(parsed_func, a, b, eps);
                    i++;
                }
                //Если выбран метод Симпсона
                if (indexes_are_checked[2] == true)
                {
                    //Считаем интеграл методом Симпсона
                    results[i] = Simpson(parsed_func, a, b, eps);
                    i++;
                }
            }
            //Возвращаем результаты вычислений форме для отображения
            return results;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Вычисление с помощью контейнера
        static private void Container_Using(List<string> parsed_func, double a, double b, double eps, ref double[] results)
        {
            //Массивы для точек
            double[] X, Y, X1, Y1;
            int n;

            //Контейнер
            Container container = new Container();
            //Дифференциатор
            Der derivative = new Der();
            //Вычислитель значений функции
            F_calculation F = new F_calculation();
            //Исполнители методов
            Rect method_rect = new Rect();
            Tr method_tr = new Tr();
            Simp method_simp = new Simp();
            //Добавляем компоненты в контейнер
            container.Add(F, "F_calculation");
            container.Add(derivative, "derivative");
            container.Add(method_rect, "Rectangle");
            container.Add(method_tr, "Trapeze");
            container.Add(method_simp, "Simpson");

            //Получаем табличное представление функции (нужно для численного дифферецирования при подсчёте шага)
            F.GetPoints(parsed_func, a, b, 500, out X1, out Y1);
                
                //Метод прямоугольников
            //Считаем вторую производную
            Y = derivative.Derivative_2(X1, Y1);
            //Считаем количество разбиений, необходимое для достижения требуемой точности
            n = method_rect.SectionsRectangle(Y, a, b, eps);
            //Получаем требуемое табличное представление функции
            F.GetPoints(parsed_func, a, b, n, out X, out Y);
            //Возвращаем посчитанный интеграл методом прямоугольников
            results[0] = method_rect.Calculate(Y, a, b, n);

                //Метод трапеций
            //Считаем вторую производную
            Y = derivative.Derivative_2(X1, Y1);
            //Считаем количество разбиений, необходимое для достижения требуемой точности
            n = method_tr.SectionsTrapeze(Y, a, b, eps);
            //Получаем требуемое табличное представление функции
            F.GetPoints(parsed_func, a, b, n, out X, out Y);
            //Возвращаем посчитанный интеграл методом трапеций
            results[1] = method_tr.Calculate(Y, a, b, n);

                //Метод Симпсона
            //Считаем четвёртую производную
            Y = derivative.Derivative_4(X1, Y1);
            //Считаем количество разбиений, необходимое для достижения требуемой точности
            n = method_simp.SectionsSimson(Y, a, b, eps);
            //Получаем требуемое табличное представление функции
            F.GetPoints(parsed_func, a, b, n, out X, out Y);
            //Возвращаем посчитанный интеграл методом Симпсона
            results[2] = method_simp.Calculate(Y, a, b, n);

            //Считаем интегралы всеми методами
            //results[0] = Rectangle(parser, parsed_func, a, b, eps);
            //results[1] = Trapeze(parser, parsed_func, a, b, eps);
            //results[2] = Simpson(parser, parsed_func, a, b, eps);

            //Освобождаем ресурсы
            container.Dispose();
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Вычисление методом прямоугольника
        static private double Rectangle(List<string> parsed_func, double a, double b, double eps)
        {
            //Дифференциатор
            Der derivative = new Der();
            //Вычислитель значений функции
            F_calculation F = new F_calculation();
            //Исполнитель метода
            Rect method = new Rect();
            //Массивы для точек
            double[] X, Y, Y1;

            //Получаем табличное представление функции (нужно для численного дифферецирования при подсчёте шага)
            F.GetPoints(parsed_func, a, b, 500, out X, out Y1);
            //Считаем вторую производную
            Y = derivative.Derivative_2(X, Y1);
            //Считаем количество разбиений, необходимое для достижения требуемой точности
            int n = method.SectionsRectangle(Y, a, b, eps);
            //Получаем требуемое табличное представление функции
            F.GetPoints(parsed_func, a, b, n, out X, out Y);
            //Возвращаем посчитанный интеграл методом прямоугольников
            return method.Calculate(Y, a, b, n);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Вычисление методом трапеций
        static private double Trapeze(List<string> parsed_func, double a, double b, double eps)
        {
            //Дифференциатор
            Der derivative = new Der();
            //Вычислитель значений функции
            F_calculation F = new F_calculation();
            //Исполнитель метода
            Tr method = new Tr();
            //Массивы для точек
            double[] X, Y, Y1;
            //Получаем табличное представление функции (нужно для численного дифферецирования при подсчёте шага)
            F.GetPoints(parsed_func, a, b, 500, out X, out Y1);
            //Считаем вторую производную
            Y = derivative.Derivative_2(X, Y1);
            //Считаем количество разбиений, необходимое для достижения требуемой точности
            int n = method.SectionsTrapeze(Y, a, b, eps);
            //Получаем требуемое табличное представление функции
            F.GetPoints(parsed_func, a, b, n, out X, out Y);
            //Возвращаем посчитанный интеграл методом трапеций
            return method.Calculate(Y, a, b, n);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Вычисление методом Симпсона
        static private double Simpson(List<string> parsed_func, double a, double b, double eps)
        {
            //Дифференциатор
            Der derivative = new Der();
            //Вычислитель значений функции
            F_calculation F = new F_calculation();
            //Исполнитель метода
            Simp method = new Simp();
            //Массивы для точек
            double[] X, Y, Y1;
            //Получаем табличное представление функции (нужно для численного дифферецирования при подсчёте шага)
            F.GetPoints(parsed_func, a, b, 500, out X, out Y1);
            //Считаем четвёртую производную
            Y = derivative.Derivative_4(X, Y1);
            //Считаем количество разбиений, необходимое для достижения требуемой точности
            int n = method.SectionsSimson(Y, a, b, eps);
            //Получаем требуемое табличное представление функции
            F.GetPoints(parsed_func, a, b, n, out X, out Y);
            //Возвращаем посчитанный интеграл методом Симпсона
            return method.Calculate(Y, a, b, n);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
