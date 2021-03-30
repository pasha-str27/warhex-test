using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace relaction_method_CHMMF
{
    class Program
    {
        static double T_y(double y)
        {
            return 35.5 * y + 8;
        }

        static bool checkPrecision(double[][] T_k, double[][] T_kPrev)
        {
            bool result = true;
            for (int i = 1; i < T_k.Length - 1; ++i)
                for (int j = 1; j < T_k[i].Length-1; ++j)
                    if (Math.Abs(T_k[i][j] - T_kPrev[i][j]) > 0.1f)
                        return true;

            return false;
        }

        static void Main(string[] args)
        {
            double p, w;
            float a=3, b=4;
            float h = 1;
            int n=(int)(a/h);
            int m = (int)(b / h);
            p = 0.5 * (Math.Cos(Math.PI/n)+ Math.Cos(Math.PI / m));
            w = 2f / (1 + Math.Sqrt(1 - Math.Pow(p, 2)));
            Console.WriteLine("p="+p);
            Console.WriteLine("w="+w);
            double T1 = 8,T2=150;

            double[][] T_k = new double[n + 1][];
            double[][] T_kPrev = new double[n + 1][];

            for(int i=0;i<n+1;++i)
            {
                T_k[i] = new double[m + 1];
                T_kPrev[i] = new double[m + 1];
            }

            for (int i = 0; i < n+1; ++i)
            {
                T_k[i][0] = T1;
                T_k[i][m] = T2;
            }

            for (int i = 1; i < m; ++i)
            {
                T_k[0][i] = T_y(i);
                T_k[n][i] = T_k[0][i];
            }

            for (int i = 1; i < n ; ++i)
                for (int j = 1; j < m ; ++j)
                    T_k[i][j] = (T_k[i][0]+ T_k[i][m]+ T_k[0][j]+ T_k[n][j]) /4f;

            int k = 0;
            Console.WriteLine("\nk=" + k);
            for (int i = m; i >= 0; --i)
            {
                for (int j = 0; j < n + 1; ++j)
                    Console.Write("{0:f3}\t", T_k[j][i]);
                Console.WriteLine();
            }

            do
            {
                for (int i = 1; i < n; ++i)
                    for (int j = 1; j < m; ++j)
                        T_kPrev[i][j] = T_k[i][j];

                for (int i = 1; i < n; ++i)
                    for (int j = 1; j < m; ++j)
                        T_k[i][j] = (w/4f)*(T_k[i-1][j] + T_k[i+1][j] + T_k[i][j -1] + T_k[i][j +1]) +(1-w)*T_k[i][j];
                ++k;
                Console.WriteLine("\nk=" + k);
                for (int i = m; i >= 0; --i)
                {
                    for (int j = 0; j < n + 1; ++j)
                        Console.Write("{0:f3}\t" , T_k[j][i]);
                    Console.WriteLine();
                }
            } while (checkPrecision(T_k,T_kPrev));

        }
    }
}
