using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp11
{
    class Program
    {

        static int size;

        static Matrix A;
        static Matrix A1;
        static Matrix A2;
        static Matrix B2;
        static Matrix C2;


        static Matrix b;
        static Matrix b1;
        static Matrix c1;

        static Matrix tmp1;
        static Matrix tmp2;
        static Matrix tmp3;
        static Matrix tmp4;
        static Matrix tmp5;
        static Matrix tmp6;
        static Matrix tmp7;

        static Matrix y1;
        static Matrix y2;
        static Matrix y3;

        static Matrix x;

        static double K1;
        static double K2;

        static Random random;

        class Matrix
        {
            double[][] matrix;

            public Matrix()
            {
                matrix = new double[size][];
                for (int i = 0; i < size; ++i)
                    matrix[i] = new double[size];

                for (int i = 0; i < size; ++i)
                    for (int j = 0; j < size; ++j)
                        matrix[i][j] = random.Next(1, 10);
            }

            static public Matrix operator *(Matrix X, double k)
            {
                Matrix Z = new Matrix();
                Z.changeMatrixSize(X.rowsCount, X.colsCount);

                for (int i = 0; i < X.rowsCount; i++)
                {
                    for (int j = 0; j < X.colsCount; j++)
                    {
                        Z[i, j] = X[i, j] * k;
                    }
                }
                return Z;
            }

            public double this[int i,int j]
            {
                get
                {
                    return matrix[i][j];
                }
                set
                {
                    matrix[i][j] = value;
                }
            }

            static public Matrix operator +(Matrix A, Matrix B)
            {
                Matrix Z = new Matrix();
                if(A.rowsCount==1&& A.colsCount==1)
                {
                    Z.changeMatrixSize(B.rowsCount, B.colsCount);
                    for (int i = 0; i < B.rowsCount; i++)
                        for (int j = 0; j < B.colsCount; j++)
                            Z[i, j] = A[0, 0] + B[i, j];
                    return Z;
                }
                Z.changeMatrixSize(A.rowsCount, A.colsCount);

                for (int i = 0; i < A.rowsCount; i++)
                    for (int j = 0; j < A.colsCount; j++)
                        Z[i,j] = A[i, j] + B[i, j];

                return Z;
            }

            static public Matrix operator -(Matrix A, Matrix B)
            {
                Matrix Z = new Matrix();
                Z.changeMatrixSize(A.rowsCount, A.colsCount);

                for (int i = 0; i < A.rowsCount; i++)
                    for (int j = 0; j < A.colsCount; j++)
                        Z[i, j] = A[i, j] - B[i, j];

                return Z;
            }

            void changeMatrixSize(int rows,int cols)
            {
                matrix = new double[rows][];
                for (int i = 0; i < rows; ++i)
                    matrix[i] = new double[cols];
            }

            int rowsCount
            {
                get
                {
                    return matrix.Length;
                }
            }

            int colsCount
            {
                get
                {
                    return matrix[0].Length;
                }
            }

            static public Matrix operator *(Matrix A, Matrix B)
            {
                Matrix Z = new Matrix();
                Z.changeMatrixSize(A.rowsCount, B.colsCount);

                for (var i = 0; i < A.rowsCount; i++)
                    for (var j = 0; j < B.colsCount; j++)
                    {
                        Z[i, j] = 0;

                        for (var k = 0; k < A.colsCount; k++)
                            Z[i, j] += A[i, k] * B[k, j];
                    }
                return Z;
            }

            public void printMatrix()
            {
                for (int i = 0; i < matrix.Length; ++i)
                {
                    for (int j = 0; j < matrix[i].Length; ++j)
                        Console.Write("{0:F2}\t", matrix[i][j]);
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            public void initialVectorB()
            {
                matrix = new double[size][];
                for (int i = 0; i < size; ++i)
                    matrix[i] = new double[1];

                for (int i = 0; i < size; ++i)
                    matrix[i][0] = 5 * Math.Pow(i + 1, 3);
            }
            public void initC2()
            {
                matrix = new double[size][];
                for (int i = 0; i < size; ++i)
                    matrix[i] = new double[size];

                for(int i=0;i<size;++i)
                    for(int j=0;j<size;++j)
                        matrix[i][j] = 1f/(Math.Pow(i+1,2)+j+1);
            }

            public void randInitialVector()
            {
                matrix = new double[size][];
                for (int i = 0; i < size; ++i)
                    matrix[i] = new double[1];

                for (int i = 0; i < size; ++i)
                    matrix[i][0] = random.Next(1, 10);
            }

            public Matrix GetTranspMatrix()
            {
                Matrix Z = new Matrix();
                Z.changeMatrixSize(colsCount, rowsCount);

                for (int i = 0; i < colsCount; ++i)
                    for (int j = 0; j < rowsCount; ++j)
                        Z[i, j] = matrix[j][i];

                return Z;
            }
        }


        static void action11()
        {
            tmp1 = new Matrix();
            tmp1 = b1*5 - c1;
        }

        static void action12()
        {
            tmp2 = new Matrix();
            tmp2 = B2 + C2*10;
        }

        static void action21()
        {
            y1 = new Matrix();
            y1 = A*b;
        }

        static void action22()
        {
            y2 = new Matrix();
            y2 = A1 * tmp1;
        }

        static void action23()
        {
            y3 = new Matrix();
            y3 = A2 * tmp2;
        }

        static void action31()
        {
            tmp1 = new Matrix();
            tmp1 = y1 * y2.GetTranspMatrix();
        }

        static void action32()
        {
            tmp2 = new Matrix();
            tmp2 = y1.GetTranspMatrix()*y3;
        }

        static void action33()
        {
            tmp3 = new Matrix();
            tmp3 = y3 * y2;
        }

        static void action34()
        {
            tmp4 = new Matrix();
            tmp4 = y3 * y3;

        }

        static void action41()
        {
            tmp5 = new Matrix();
            tmp5 = tmp2 * y1;

        }

        static void action42()
        {
            tmp6 = new Matrix();
            tmp6 = tmp3+y1;
            
        }

        static void action43()
        {
            tmp7 = new Matrix();
            tmp7 = tmp4 * tmp1;

        }

        static void action51()
        {
            tmp1 = new Matrix();
            tmp1 = tmp5 + y2.GetTranspMatrix();

        }

        static void action52()
        {
            tmp2 = new Matrix();
            tmp2 = tmp7 * y2 * K2;
            tmp2.printMatrix();
        }

        static void action61()
        {
            tmp3 = new Matrix();
            tmp3 = tmp6 + tmp2;
            tmp3.printMatrix();
        }

        static void action71()
        {
            x = new Matrix();
            x = tmp1 * tmp3*K1;
        }

        static void Main(string[] args)
        {
            Console.Write("size=");
            size = int.Parse(Console.ReadLine());

            random = new Random();

            b = new Matrix();
            b.initialVectorB();

            A = new Matrix();
            A1 = new Matrix();
            A2 = new Matrix();
            C2 = new Matrix();

            C2.initC2();

            b1 = new Matrix();
            c1 = new Matrix();
            B2 = new Matrix();

            b1.randInitialVector();
            c1.randInitialVector();
            //B2.randInitialVector();

            K1=random.Next(1,10)*random.NextDouble();
            K2= random.Next(1, 10) * random.NextDouble();

            Console.WriteLine("K1={0:F2}", K1);
            Console.WriteLine("K2={0:F2}\n", K2);

            Console.WriteLine("matrix A:");
            A.printMatrix();

            Console.WriteLine("matrix A1:");
            A1.printMatrix();

            Console.WriteLine("matrix A2:");
            A2.printMatrix();

            Console.WriteLine("matrix C2:");
            C2.printMatrix();

            Console.WriteLine("matrix B2:");
            B2.printMatrix();

            Console.WriteLine("vector b:");
            b.printMatrix();

            Console.WriteLine("vector b1:");
            b1.printMatrix();

            Console.WriteLine("vector c1:");
            c1.printMatrix();

            Thread thread1 = new Thread(action11);
            thread1.Start();

            action12();
            thread1.Join();

            thread1 = new Thread(action21);
            Thread thread2 = new Thread(action22);

            thread1.Start();
            thread2.Start();

            action23();
            thread1.Join();
            thread2.Join();

            Console.WriteLine("vector y1:");
            y1.printMatrix();

            Console.WriteLine("vector y2:");
            y2.printMatrix();

            Console.WriteLine("matrix y3:");
            y3.printMatrix();

            thread1 = new Thread(action31);
            thread2 = new Thread(action32);
            Thread thread3 = new Thread(action33);

            thread1.Start();
            thread2.Start();
            thread3.Start();

            action34();
            thread1.Join();
            thread2.Join();
            thread3.Join();

            thread1 = new Thread(action41);
            thread2 = new Thread(action42);

            thread1.Start();
            thread2.Start();

            action43();
            thread1.Join();
            thread2.Join();

            thread1 = new Thread(action51);
            thread1.Start();

            action52();
            thread1.Join();
            action61();
            action71();

            Console.WriteLine("x=");
            x.printMatrix();

            Console.ReadKey();
        }
    }
}