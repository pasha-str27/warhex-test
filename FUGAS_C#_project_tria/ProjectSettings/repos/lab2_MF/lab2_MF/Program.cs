using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_MF
{
    class Program
    {
        static double T_0_t()
        {
            return 5;
        }

        static double T_l_t()
        {
            return 40* T_0_t();
        }

        static double T_x_0(double x,double l)
        {
            return (T_l_t() - T_0_t()) / (l * l) * x * x + T_0_t();
        }

        static void Main(string[] args)
        {
            int k = 5;
            double p = 7150;
            double lambda = 0.113f;
            double c = 0.384f;
            double h = 0.1f;
            int m = 3;
            double l = 0.5f;
            double a_2 = lambda / (c * p);
            int n = (int)Math.Round(l / h);

            double sigma = 1f / 6f;
            double tao = (sigma * h * h) / a_2;

            double[,] T = new double[m+1, n+1];

            for (int j = 0; j < m + 1; ++j)
                for (int i=0;i<n+1;++i)
                {
                    if (i == 0)
                        T[j, i] = T_0_t();
                    else
                    {
                        if(i==n)
                            T[j, i] = T_l_t();
                        else
                        {
                            if (j == 0)
                                T[j, i] = T_x_0(i * h, l);
                            else
                                T[j, i] = sigma * (T[j - 1, i - 1] + 4 * T[j-1, i] + T[j - 1, i + 1]);
                        }
                    }
                }

            for (int j = 0; j < m + 1; ++j)
            {
                for (int i = 0; i < n + 1; ++i)
                    Console.Write(T[j, i] + "\t");
                Console.WriteLine();
            }
        }
    }
}
