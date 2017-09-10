using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Func_calc
{
    public class F_calculation : Component
    {
        //Вычисление занчения распарсенной функции при указанном значении переменной
        public double Calc(List<string> postfixExpression, double x)
        {
            //Список, который будем использовать для вычисления результата. В итоге result[0] будет содержать результат вычисления
            List<double> result = new List<double>();
            //Для каждой операции в списке операций
            for (int i = 0; i < postfixExpression.Count; i++)
            {
                //Сперва идёт получение значений
                //Если число
                if (postfixExpression[i][0] >= '0' && postfixExpression[i][0] <= '9')
                {
                    //Добавляем к списку, предварительно переведя строковое представление числа в double
                    result.Add(Double.Parse(postfixExpression[i]));
                    continue;
                }
                //Если переменная, добавляем к списку
                if (postfixExpression[i] == "x")
                {
                    result.Add(x);
                    continue;
                }


                //Далее идут функции от одного аргумента
                //Если ни число, ни переменная не были получены - выводим ошибку
                if (result.Count < 1)
                {
                    throw new Exception("Не хватает операндов для операции " + postfixExpression[i]);
                }
                //Если функция, то будем вычислять эту функцию от полученного значения. Полученный результат присвоим тому же элементу списка, в котором содержалось значение
                if (postfixExpression[i] == "sin")
                {
                    result[result.Count - 1] = Math.Sin(result[result.Count - 1]);
                    continue;
                }
                if (postfixExpression[i] == "cos")
                {
                    result[result.Count - 1] = Math.Cos(result[result.Count - 1]);
                    continue;
                }
                if (postfixExpression[i] == "log")
                {
                    result[result.Count - 1] = Math.Log(result[result.Count - 1]);
                    continue;
                }
                if (postfixExpression[i] == "arccos")
                {
                    result[result.Count - 1] = Math.Acos(result[result.Count - 1]);
                    continue;
                }

                if (postfixExpression[i] == "arcsin")
                {
                    result[result.Count - 1] = Math.Asin(result[result.Count - 1]);
                    continue;
                }
                if (postfixExpression[i] == "exp")
                {
                    result[result.Count - 1] = Math.Exp(result[result.Count - 1]);
                    continue;
                }
                if (postfixExpression[i] == "tg")
                {
                    result[result.Count - 1] = Math.Tan(result[result.Count - 1]);
                    continue;
                }
                if (postfixExpression[i] == "ctg")
                {
                    result[result.Count - 1] = 1.0 / Math.Tan(result[result.Count - 1]);
                    continue;
                }
                if (postfixExpression[i] == "arctg")
                {
                    result[result.Count - 1] = Math.Atan(result[result.Count - 1]);
                    continue;
                }
                if (postfixExpression[i] == "arcctg")
                {
                    result[result.Count - 1] = Math.PI / 2.0 - Math.Atan(result[result.Count - 1]);
                    continue;
                }
                if (postfixExpression[i] == "neg")
                {
                    result[result.Count - 1] *= -1.0;
                    continue;
                }

                //Далее идут функции от двух аргументов
                //Если недостаточно значений, выводим ошибку
                if (result.Count < 2)
                {
                    throw new Exception("Не хватает операндов для операции " + postfixExpression[i]);
                }
                //Если функция от двух аргументов, то будем вычислять эту функцию от полученных значений. Полученный результат присвоим предпоследнему элементу списка. Последний элемент удалим.
                if (postfixExpression[i] == "^")
                {
                    result[result.Count - 2] = Math.Pow(result[result.Count - 2], result[result.Count - 1]);
                    result.RemoveAt(result.Count - 1);
                    continue;
                }
                if (postfixExpression[i] == "*")
                {
                    result[result.Count - 2] = result[result.Count - 2] * result[result.Count - 1];
                    result.RemoveAt(result.Count - 1);
                    continue;
                }
                if (postfixExpression[i] == "/")
                {
                    result[result.Count - 2] = result[result.Count - 2] / result[result.Count - 1];
                    result.RemoveAt(result.Count - 1);
                    continue;
                }
                if (postfixExpression[i] == "+")
                {
                    result[result.Count - 2] = result[result.Count - 2] + result[result.Count - 1];
                    result.RemoveAt(result.Count - 1);
                    continue;
                }
                if (postfixExpression[i] == "-")
                {
                    result[result.Count - 2] = result[result.Count - 2] - result[result.Count - 1];
                    result.RemoveAt(result.Count - 1);
                    continue;
                }

                //Если получили скобку - значит, неправильно распарсена (т.к. неправильно введена) функция. Выводим ошибку.
                if (postfixExpression[i] == "(" || postfixExpression[i] == ")")
                {
                    throw new Exception("Проверьте корректность рассталвенных скобок");
                }

            }

            //Если дошли до этого момента с двумя и более элементами списка, значит, данные введены неверно. Выводим ошибку
            if (result.Count > 1)
            {
                throw new Exception("Не хватает оператора для операции над числами");
            }

            //Возвращаем результат
            return result[0];

        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Получение точек по числу секций.
        public void GetPoints(List<string> postfixExpression, double a, double b, int numberSections, out double[] X, out double[] Y)
        {
            double x;
            //Вычисляем шаг
            double step = (b - a) / (double)(numberSections);
            //Число точек
            int numberPoints = numberSections + 1;

            //Создаём массивы для точек и значений функции в них
            X = new double[numberPoints];
            Y = new double[numberPoints];

            //Начинаем с точки a
            x = a;
            X[0] = x;
            Y[0] = Calc(postfixExpression, x);

            //Добавляем остальные точки
            for (int i = 1; i < numberPoints; i++)
            {
                x += step;
                X[i] = x;
                Y[i] = Calc(postfixExpression, x);
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Получение точек по шагу
        public void GetPoints(List<string> postfixExpression, double a, double b, double step, out double[] X, out double[] Y)
        {
            double x;
            //Число секций
            double numberSections = (b - a) / (double)step;
            //Тернарный оператор для вычисления числа точек (округляем число секций)
            int numberPoints = ((numberSections - (int)numberSections) > 0) ? (int)numberSections + 2 : (int)numberSections + 1;

            //Создаём массивы для точек и значений функции в них
            X = new double[numberPoints];
            Y = new double[numberPoints];

            //Начинаем с точки a
            x = a;
            X[0] = x;
            Y[0] = Calc(postfixExpression, x);

            //Добавляем остальные точки
            for (int i = 1; i < numberPoints; i++)
            {
                x += step;
                X[i] = x;
                Y[i] = Calc(postfixExpression, x);
            }
        }
    }
}