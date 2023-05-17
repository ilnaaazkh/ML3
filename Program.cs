//Хузин Ильназ, 4217, #15 в списке группы
//Унарная операция -x отрицание Поста (вариант 2)
//Бинарная операция x + y сумма по моудлю k (вариант 4)
//Форма представления - вторая стандартная

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextAnalyze;

namespace ML3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выполнил: студент группы 4217 Хузин Ильназ(#15 в списке группы)");
            Console.WriteLine("Справка:");
            Console.WriteLine("1. Вводить только переменные х и у");
            Console.WriteLine("2. Не вводить в формуле значение постоянных больше или равно k");
            Console.WriteLine("3. Использовать только предоставленные унарные операции");
            Console.WriteLine("4. jN(var) - характерестическая функция первого рода числа N");
            int k;
            int n;
            string func;
            while (true)
            {
                #region Получение и проверка k и n от пользователя
                Console.Write("Введите значение k: ");
                while (!(int.TryParse(Console.ReadLine(), out k)) || k < 0)
                {
                    Console.WriteLine("Введенное значение k неккоректно.(k - целое, положительное число)");
                    Console.WriteLine("Повторите попытку");
                }

                Console.Write("Введите значение n(числа существенных переменных (1 или 2)): ");
                while (!(int.TryParse(Console.ReadLine(), out n)) || (n != 1 && n != 2))
                {
                    Console.WriteLine("Введенное значение n неккоректно. (n = 1 или n = 2)");
                    Console.WriteLine("Повторите попытку");
                }
                #endregion

                Console.WriteLine("Введите функцию: ");
                Console.WriteLine("Доступные операции: \nОтрицание Поста: -x\nСумма по модулю k: x + y\n");
                func = Console.ReadLine();

                try
                {
                    Console.WriteLine("Введеная формула: f(x) = " + func);


                    int[,] table = MakeTable(func, n, k);
                    Console.WriteLine("Таблица результатов функции: ");
                    PrintTable(table, n, k);


                    string SecondForm = MakeSecondForm(table, n, k);
                    Console.WriteLine("Вторая стандартная форма представления: ");
                    Console.WriteLine(SecondForm);

                    Console.WriteLine("Введите значения множества E через пробел: ");
                    string set = Console.ReadLine();
                    List<int> list;

                    if(set.All(x => char.IsDigit(x) || x == ' '))
                    {
                        list = set.Trim().Split(' ').Select(x => int.Parse(x)).ToList();
                        if (CheckSet(list, k))
                        {
                            if (IsSaveSet(table, n, k, list))
                            {
                                Console.WriteLine("Функция принадлежит T(E)");
                            }
                            else
                            {
                                Console.WriteLine("Функция не сохраняет E");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Введенные данные некорректны");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Множество введено некорректно");
                    }


                }
                catch
                {
                    Console.WriteLine("Введеная формула не является корректной");
                }

                #region Завершить или продолжить работу?
                Console.WriteLine("Выберите действие: ");
                Console.WriteLine("1 - продолжить работу");
                Console.WriteLine("2 - заврешить работу");
                int answer;
                while(!int.TryParse(Console.ReadLine(), out answer) || (answer != 1 && answer != 2))
                {
                    Console.WriteLine("Ошибка ввода. Попробуйте еще раз");
                }
                if(answer == 2)
                {
                    break;
                }
                #endregion
            }
        }

        public static int[,] MakeTable(string func, int n, int k)
        {
            string copy = func;
            int[,] table;
            if (n == 1)
            {
                table = new int[k, 1];
                for (int x = 0; x < k; x++)
                {
                    copy = func.Replace("x", x.ToString());
                    var lexemes = Analyzer.Analyze(copy);
                    var buffer = new LexemeBuffer(lexemes);
                    var result = (new Parser(n, k)).Expr(buffer);
                    table[x, 0] = result;
                }
            }
            else
            {
                table = new int[k, k];
                for (int x = 0; x < k; x++)
                {
                    for (int y = 0; y < k; y++)
                    {
                        copy = func.Replace("x", x.ToString()).Replace("y", y.ToString());
                        var lexemes = Analyzer.Analyze(copy);
                        var buffer = new LexemeBuffer(lexemes);
                        var result = (new Parser(n, k)).Expr(buffer);
                        table[x, y] = result;
                    }
                }
            }
            return table;
        }

        public static void PrintTable(int[,] results, int n, int k)
        {
            if (n == 1)
            {
                Console.WriteLine(" x | f(x) ");

                for (int x = 0; x < k; x++)
                {
                    Console.WriteLine(" " + x + " | " + results[x, 0]);
                }
            }
            else
            {
                Console.WriteLine(" x | y |f(x) ");

                for (int x = 0; x < k; x++)
                {
                    for (int y = 0; y < k; y++)
                    {
                        Console.WriteLine(" " + x + " | " + y + " | " + results[x, y]);
                    }
                }
            }
        }

        public static string MakeSecondForm(int[,] results, int n, int k)
        {
            StringBuilder sb = new StringBuilder();

            if (n == 1)
            {
                for (int x = 0; x < k; x++)
                {
                    if (results[x, 0] == 0)
                    {
                        continue;
                    }

                    if (sb.Length != 0)
                    {
                        sb.Append(" + ");
                    }
                    sb.Append(results[x, 0].ToString() + "*j" + x.ToString() + "(x)");
                }
            }
            else
            {
                for (int x = 0; x < k; x++)
                {
                    for (int y = 0; y < k; y++)
                    {
                        if (results[x, y] == 0)
                        {
                            continue;
                        }

                        if (sb.Length != 0)
                        {
                            sb.Append(" + ");
                        }
                        sb.Append(results[x, y].ToString() + "*j" + x.ToString() + "(x)" + "*j" + y.ToString() + "(y)");
                    }
                }
            }

            return sb.ToString();
        }

        public static bool IsSaveSet(int[,] results, int n, int k, List<int> set)
        {
            if (n == 1)
            {
                foreach (int x in set)
                {
                    if (!set.Contains(results[x, 0]))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                foreach (int x in set)
                {
                    foreach (int y in set)
                    {
                        if (!set.Contains(results[x, y]))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        public static bool CheckSet(List<int> set, int k)
        {
            return set.All(x => x < k && x >= 0);
        }
    }
}
