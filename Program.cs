using System;
using System.Collections.Generic;

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
                        Worker.AddWorker();
                        break;
                    case ConsoleKey.D2:
                        // Вывод всего списка
                        Worker.PrintWorkers();
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
    struct Worker
    {
        public enum GenderEnum
        {
            MALE,
            FEMALE
        }
        private static List<Worker> workers = new List<Worker>();

        /// Полное имя работника
        string Name;
        /// Должность работника
        string Position;
        /// Пол работника
        GenderEnum Gender;
        /// Дата найма работника
        DateTime HireDate;

        public static void AddWorker()
        {
            Worker worker = new Worker();
            Console.Write("Введите полное имя работника: ");
            worker.Name = ReadString();
            Console.Write("Введите должность работника: ");
            worker.Position = ReadString();
            Console.Write("Введите пол работника (м/ж): ");
            worker.Gender = ReadGender();
            Console.Write("Введите дату найма работника: ");
            worker.HireDate = ReadDate();
            workers.Add(worker);
        }

        public static void PrintWorkers()
        {
            int counter = 1;
            foreach (var worker in workers)
            {
                Console.WriteLine($"        Работник №{ counter++ }:");
                worker.PrintSingleWorker();
            }
        }

        public static string ReadString()
        {
            string result;
            while ((result = Console.ReadLine()) == string.Empty)
            {
                Console.WriteLine("Нельзя ввести пустую строку");
            };
            return result;
        }

        public static GenderEnum ReadGender()
        {
            while (true)
            {
                switch (ReadString().ToLower()[0])
                {
                    case 'м':
                        return GenderEnum.MALE;
                    case 'ж':
                        return GenderEnum.FEMALE;
                    default:
                        Console.WriteLine("Пол указан неверно. Просто введите «м» или «ж».");
                        continue;
                }
            }
        }

        public static DateTime ReadDate()
        {
            while (true)
            {
                DateTime result;
                if (DateTime.TryParse(ReadString(), out result))
                    return result;
                Console.WriteLine("Неправильный формат даты. Введите дату вида «день.месяц.год час:минута».");
            }
        }

        void PrintSingleWorker()
        {
            string genderString;
            switch (Gender)
            {
                case GenderEnum.MALE:
                    genderString = "мужской";
                    break;
                case GenderEnum.FEMALE:
                    genderString = "женский";
                    break;
                default:
                    genderString = "ошибочка в программе";
                    break;
            }

            Console.WriteLine($"Имя: { Name }");
            Console.WriteLine($"Должность: { Position }");
            Console.WriteLine($"Пол: { genderString }");
            Console.WriteLine($"Дата найма: { HireDate }");
        }
    }

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
