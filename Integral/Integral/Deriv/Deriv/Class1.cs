using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Deriv
{
    public class Der : Component
    {
        //Численное вычисление производной по точкам. nodes_n - количество узлов, которые будут использоваться
        private double[] Derivative(double[] X, double[] Y, int nodes_n)
        {
            double[] result = new double[0];
            int step = nodes_n - 2, start = 0, end = nodes_n;
            int steps_n = ((X.Length - nodes_n) % step != 0) ? (X.Length - nodes_n) / step : ((X.Length - nodes_n) / step - 1);

            result = result.Concat(DerivateNodes(X, Y, start, end, 0, 1)).ToArray();
            for (int i = 0; i < steps_n; i++)
            {
                start += step;
                end += step;
                result = result.Concat(DerivateNodes(X, Y, start, end, 1, 1)).ToArray();

            }
            result = result.Concat(DerivateNodes(X, Y, X.Length - nodes_n, X.Length, X.Length - end, 0)).ToArray();
            return result;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private double[] DerivateNodes(double[] X, double[] Y, int start, int end, int startDeflection, int endDeflection)
        {
            double[] coefficients = getCoefficients(X, Y, start, end);
            double[] result = new double[(end - endDeflection) - (start + startDeflection)];
            for (int i = start + startDeflection; i < end - endDeflection; i++)				//перебирая X[i] для нахождение y'(X[i])
            {
                double Sum = 0.0;
                for (int j = start; j < end; j++)				//суммирую все соотв произведения
                {
                    double multiplication = 1.0;
                    //если i = j, то сумма состоит из X.Length-1 слагаемого  
                    //инчае из 1 слагаемого, так как другие обнуляются
                    if (i != j)
                    {
                        for (int k = start; k < end; k++)
                        {
                            if (k == i || k == j) { continue; }
                            multiplication *= X[i] - X[k];
                        }
                    }
                    else
                    {
                        multiplication = 0.0;
                        for (int r = start; r < end; r++)		//r-ый дифференцируем
                        {
                            double mult = 1.0;
                            if (r == i) { continue; }
                            for (int k = start; k < end; k++)
                            {
                                if (k == r || k == i) { continue; }		// r-ый равен 1, i-го нет
                                mult *= X[i] - X[k];
                            }
                            multiplication += mult;
                        }
                    }
                    Sum += multiplication * coefficients[j - start];
                }
                result[i - (start + startDeflection)] = Sum;
            }
            return result;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private double[] getCoefficients(double[] X, double[] Y, int start, int end)
        {
            double[] coefficients = new double[end - start];

            for (int i = start; i < end; i++)
            {
                double multiplication = 1.0;
                for (int j = start; j < end; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }
                    multiplication *= X[i] - X[j];
                }
                coefficients[i - start] = Y[i] / multiplication;
            }
            return coefficients;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Вычисление второй производной
        public double[] Derivative_2(double[] X, double[] Y)
        {
            //Значения первой производной
            Y = Derivative(X, Y, 5);
            //Значения второй производной
            Y = Derivative(X, Y, 5);
            return Y;
        }

        //Вычисление четвёртой произовдной
        public double[] Derivative_4(double[] X, double[] Y)
        {
            //Вычисляем 4-ую производную
            for (int i = 1; i <= 4; i++)
                Y = Derivative(X, Y, 5);
            return Y;
        }
    }
}