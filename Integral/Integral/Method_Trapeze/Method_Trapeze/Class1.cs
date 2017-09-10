using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TrapezeMethod
{
    public class Tr : Component
    {
        public double Calculate(double[] Y, double a, double b, int n)
        {
            //Считаем интеграл
            double sum = 0.0;
            for (int i = 1; i < Y.Length - 1; i++)
            {
                sum += (Y[i]);
            }
            sum += (Y[0] + Y[Y.Length - 1]) / 2.0;
            return sum * (b - a) / (double)n;
        }

        //Вычисление количества секций для метода трапеций
        public int SectionsTrapeze(double[] Y, double a, double b, double eps)
        {
            //Находим максимальное и минимальное значение второй производной
            double max = Y.Max(), min = Y.Min();
            //Выбираем максимальный элемент 
            double M_2 = (Math.Abs(min) > max) ? Math.Abs(min) : max;
            //Вычисляем шаг разбиения отрезка [a,b]
            double h = Math.Sqrt((12.0 * eps) / (M_2 * (b - a)));
            //Вычисляем число шагов разбиения (секций)
            int n = (int)((b - a) / (double)h);
            //Если получилось количество шагов, меньшее 2, полагаем оптимальное количество секций равным двум
            if (n < 2)
                return 2;
            //Возвращаем количество секций
            return n;
        }
    }
}