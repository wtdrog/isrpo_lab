using System;
using System.Collections.Generic;

namespace isrpo
{
    class Program
    {
        // Функция, которая выводит справку
        public static void WriteHelp()
        {
            Console.WriteLine("1 - Ввод нового элемента в список.");
            Console.WriteLine("2 - Вывод всего списка.");
            Console.WriteLine("3 - Вывод отфильтрованного списка.");
            Console.WriteLine("4 - Ввести значение фильтра.");
            Console.WriteLine("0 - Выход из программы.");
            Console.WriteLine("9 - Вывод этой справки.");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Filter filter = new Filter();
            WriteHelp();
            // Цикл главного меню
            while (true)
            {
                Console.Write("Выберите действие: ");
                string action = Console.ReadLine();
                switch (action)
                {
                    case "1":   // Ввод нового элемента в список
                        Worker.AddWorker();
                        break;

                    case "2":   // Вывод всего списка
                        Worker.PrintWorkers();
                        break;

                    case "3":   // Вывод отфильтрованного списка
                        Worker.PrintFilteredWorkers(ref filter);
                        break;

                    case "4":   // Ввести значение фильтра                       
                        filter.setFilter();
                        break;

                    case "0":   // Выход из программы
                        Console.WriteLine("Работа программы завершена.");
                        return;

                    case "9":   // Вывод справки
                        WriteHelp();
                        break;

                    default:    // Неверный ввод
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

        public static void PrintFilteredWorkers(ref Filter filter)
        {
            foreach (Worker worker in workers)
            {
                if (filter.name != null && worker.Name != filter.name)
                    continue;

                if (filter.post != null && worker.Position != filter.post)
                    continue;

                if (filter.gender != '\0')
                {
                    if (worker.Gender == Worker.GenderEnum.MALE && filter.gender == 'ж')
                        continue;

                    if (worker.Gender == Worker.GenderEnum.FEMALE && filter.gender == 'м')
                        continue;
                }

                if (filter.recruitmentDateStart == new DateTime(0) && filter.recruitmentDateEnd == new DateTime(0))
                {
                    worker.PrintSingleWorker();
                    continue;
                }

                if (filter.recruitmentDateStart != new DateTime(0) && filter.recruitmentDateEnd == new DateTime(0))
                {
                    if (worker.HireDate < filter.recruitmentDateEnd)
                        worker.PrintSingleWorker();
                    continue;
                }

                if (filter.recruitmentDateStart != new DateTime(0) && filter.recruitmentDateEnd != new DateTime(0))
                {
                    if (worker.HireDate >= filter.recruitmentDateStart && worker.HireDate <= filter.recruitmentDateEnd)
                        worker.PrintSingleWorker();
                    continue;
                }
            }
        }
    }

    // Реализация фильтра будет здесь
    struct Filter
    {
        public string name;
        public string post;
        public char gender;
        public DateTime recruitmentDateStart;
        public DateTime recruitmentDateEnd;

        public void setFilter()
        {
            Console.Write("Введите полное имя работника: ");
            string str = Console.ReadLine();
            if (str == string.Empty)
                this.name = null;
            else
                this.name = str;

            Console.Write("Введите должность работника: ");
            str = Console.ReadLine();
            if (str == string.Empty)
                this.post = null;
            else
                this.post = str;

            Console.Write("Введите пол работника (м/ж): ");
            str = Console.ReadLine();
            if (str == string.Empty)
                gender = '\0';
            else
            {
                switch (str.ToLower()[0])
                {
                    case 'м':
                        this.gender = 'м';
                        break;
                    case 'ж':
                        this.gender = 'ж';
                        break;
                    default:
                        gender = '\0';
                        break;
                }
            }

            Console.Write("Введите дату начала временного интервала: ");
            str = Console.ReadLine();
            if (!DateTime.TryParse(str, out recruitmentDateStart))
                recruitmentDateStart = new DateTime(0);

            Console.Write("Введите дату конца временного интервала: ");
            str = Console.ReadLine();
            if (!DateTime.TryParse(str, out recruitmentDateEnd))
                recruitmentDateEnd = new DateTime(0);

            if (this.recruitmentDateStart != new DateTime(0) && this.recruitmentDateEnd != new DateTime(0) && this.recruitmentDateStart > this.recruitmentDateEnd)
            {
                DateTime temp = new DateTime();
                temp = this.recruitmentDateStart;
                this.recruitmentDateStart = this.recruitmentDateEnd;
                this.recruitmentDateEnd = temp;
            }

            if (this.name != null)
                Console.WriteLine($"Фильтрация по имени: { this.name }");
            else
                Console.WriteLine("Фильтрация по имени не задана");

            if (this.post != null)
                Console.WriteLine($"Фильтрация по должности: { this.post }");
            else
                Console.WriteLine("Фильтрация по должности не задана");

            if (this.gender != '\0')
            {
                switch (this.gender)
                {
                    case 'м':
                        Console.WriteLine("Фильтрация по полу: мужской");
                        break;
                    case 'ж':
                        Console.WriteLine("Фильтрация по полу: женский");
                        break;
                }
            }
            else
                Console.WriteLine("Фильтрация по полу не задана");

            if (this.recruitmentDateStart != new DateTime(0))
                Console.WriteLine($"Фильтрация по дате приема на работу - с : { this.recruitmentDateStart }");
            else
                Console.WriteLine("Фильтрация по дате приема на работу - нижняя граница интервала не задана");

            if (this.recruitmentDateEnd != new DateTime(0))
                Console.WriteLine($"Фильтрация по дате приема на работу - по : { this.recruitmentDateEnd }");
            else
                Console.WriteLine("Фильтрация по дате приема на работу - верхняя граница интервала не задана");
        }
    }
}