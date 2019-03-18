using System;

namespace isrpo
{
    class Program
    {
        // Реализация меню будет находиться здесь
        public static void WriteHelp()
        {
            Console.WriteLine("1 - Ввод нового элемента в список.");
            Console.WriteLine("2 - Вывод всего списка.");
            Console.WriteLine("3 - Вывод отфильтрованного списка.");
            Console.WriteLine("4 - Ввести значение фильтра.");
            Console.WriteLine("q - Выход из программы.");
            Console.WriteLine("F9 - Вывод этой справки.");
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            WriteHelp();
            while (true)
            {               
                Console.Write("Выберите действие: ");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                Console.WriteLine();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        // Ввод нового элемента в список
                        break;
                    case ConsoleKey.D2:
                        // Вывод всего списка
                        break;
                    case ConsoleKey.D3:
                        // Вывод отфильтрованного списка
                        break;
                    case ConsoleKey.D4:
                        // Ввести значение фильтра
                        break;
                    case ConsoleKey.Q:
                        // Выход из программы
                        Console.WriteLine("Работа программы завершена.");
                        return;
                    case ConsoleKey.F9:
                        // Вывод справки
                        WriteHelp();
                        break;
                    default:
                        Console.WriteLine("Неверный ввод!");
                        break;
                }
            }         
        }
    }

    // Реализация работника будет здесь

    // Реализация фильтра будет здесь
    struct Filter
    {
        string name;
        string post;
        char gender;
        DateTime recruitmentDate;

        void setFilter(string name, string post, char gender, DateTime recruitmentDate)
        {
            this.name = name;
            this.post = post;
            this.gender = gender;
            this.recruitmentDate = recruitmentDate;
        }
    }
}
