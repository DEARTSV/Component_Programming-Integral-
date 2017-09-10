using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SimpsonMethod
{
    public class Simp : Component
    {
        public double Calculate(double[] Y, double a, double b, int n)
        {
            //Считаем интеграл
            double sum1 = 0.0, sum2 = 0.0, sum = 0;

            for (int i = 1; i < Y.Length - 1; i = i + 2)
            {
                sum1 += Y[i];
            }

            for (int i = 2; i < Y.Length - 1; i = i + 2)
            {
                sum2 += Y[i];
            }

            sum += 4*sum1 + 2*sum2 + Y[0] + Y[Y.Length - 1];

            return sum * (b - a) / (3.0 * (double)n);
        }

        //Вычисление количества секций для метода Симпсона
        public int SectionsSimson(double[] Y, double a, double b, double eps)
        {
            //Находим максимальное и минимальное значение второй производной
            double max = Y.Max(), min = Y.Min();
            //Выбираем максимальный элемент 
            double M_4 = (Math.Abs(min) > max) ? Math.Abs(min) : max;
            //Вычисляем шаг разбиения отрезка [a,b]
            double h = Math.Pow((2880.0 * eps) / (M_4 * (b - a)), 0.25);
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
