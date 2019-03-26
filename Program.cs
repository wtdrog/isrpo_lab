using System;
using System.Collections.Generic;

namespace isrpo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Цикл главного меню
            Filter filter = new Filter();
            while (true)
            {
                // Вывод справки
                WriteHelp();

                // Выбор пользователем пункта главного меню
                Console.Write("Выберите действие: ");
                string action = Console.ReadLine();

                // Обработка выбора пункта главного меню
                Console.WriteLine();
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

                    default:    // Неверный ввод
                        Console.WriteLine("Неверный ввод!");
                        break;
                }
                Console.WriteLine();
            }
        }

        // Справка
        public static void WriteHelp()
        {
            Console.WriteLine("1 - Ввод нового элемента в список.");
            Console.WriteLine("2 - Вывод всего списка.");
            Console.WriteLine("3 - Вывод отфильтрованного списка.");
            Console.WriteLine("4 - Ввести значение фильтра.");
            Console.WriteLine("0 - Выход из программы.");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Работник
    /// </summary>
    struct Worker
    {
        /// <summary>
        /// Пол
        /// </summary>
        public enum GenderEnum
        {
            MALE,
            FEMALE
        }

        /// <summary>
        /// Список работников
        /// </summary>
        private static List<Worker> workers = new List<Worker>();

        /// <summary>
        /// Полное имя работника
        /// </summary>
        string Name;

        /// <summary>
        /// Должность работника
        /// </summary>
        string Position;

        /// <summary>
        /// Пол работника
        /// </summary>
        GenderEnum Gender;

        /// <summary>
        /// Дата найма работника
        /// </summary>
        DateTime HireDate;

        /// <summary>
        /// Добавление нового работника из консоли
        /// </summary>
        public static void AddWorker()
        {
            // Создание структуры
            Worker worker = new Worker();

            // Ввод имени
            Console.Write("Введите полное имя работника: ");
            worker.Name = ReadString();

            // Ввод должности
            Console.Write("Введите должность работника: ");
            worker.Position = ReadString();

            // Ввод пола
            Console.Write("Введите пол работника (м/ж): ");
            worker.Gender = ReadGender();

            // Ввод даты найма
            Console.Write("Введите дату найма работника: ");
            worker.HireDate = ReadDate();

            // Сохранение работника в список
            workers.Add(worker);
        }

        /// <summary>
        /// Вывод всех работников на консоль с их подсчётом
        /// </summary>
        public static void PrintWorkers()
        {
            int counter = 1;
            foreach (var worker in workers)
            {
                Console.WriteLine($"        Работник №{ counter++ }:");
                worker.PrintSingleWorker();
            }
        }

        /// <summary>
        /// Ввод непустой строки
        /// </summary>
        public static string ReadString()
        {
            string result;
            while ((result = Console.ReadLine()) == string.Empty)
            {
                Console.WriteLine("Нельзя ввести пустую строку");
            };
            return result;
        }

        /// <summary>
        /// Ввод пола работника
        /// </summary>
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

        /// <summary>
        /// Ввод даты и времени
        /// </summary>
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

        /// <summary>
        /// Вывод информации об одном работнике на консоль
        /// </summary>
        void PrintSingleWorker()
        {
            // Выбор строкового представления пола работника
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

            // Вывод информации о работнике
            Console.WriteLine($"Имя: { Name }");
            Console.WriteLine($"Должность: { Position }");
            Console.WriteLine($"Пол: { genderString }");
            Console.WriteLine($"Дата найма: { HireDate }");
        }

        /// <summary>
        /// Вывод отфильтрованного списка работников на консоль.
        /// <param name = "filter"> Ссылка на используемый фильтр </param>
        /// </summary>
        public static void PrintFilteredWorkers(ref Filter filter)
        {
            foreach (Worker worker in workers)
            {
                // Проверка соответствия работника фильтру по имени
                if (filter.name != null && !worker.Name.Contains(filter.name))
                    continue;

                // Проверка соответствия работника фильтру по должности
                if (filter.post != null && worker.Position != filter.post)
                    continue;

                // Проверка соответствия работника фильтру по полу
                switch (filter.gender)
                {
                    case 'м':
                        if (worker.Gender == Worker.GenderEnum.FEMALE)
                            continue;
                        break;

                    case 'ж':
                        if (worker.Gender == Worker.GenderEnum.MALE)
                            continue;
                        break;
                }

                // Проверка соответствия работника фильтру по дате приема на работу
                if (filter.recruitmentDateStart == new DateTime(0))
                {
                    if (filter.recruitmentDateEnd == new DateTime(0))
                    {
                        worker.PrintSingleWorker();
                    }
                    else
                    {
                        if (worker.HireDate <= filter.recruitmentDateEnd)
                        {
                            worker.PrintSingleWorker();
                        }
                    }
                }
                else
                {
                    if (filter.recruitmentDateEnd == new DateTime(0))
                    {
                        if (worker.HireDate >= filter.recruitmentDateStart)
                        {
                            worker.PrintSingleWorker();
                        }
                    }
                    else
                    {
                        if (worker.HireDate >= filter.recruitmentDateStart && worker.HireDate <= filter.recruitmentDateEnd)
                        {
                            worker.PrintSingleWorker();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Фильтр
    /// </summary>
    struct Filter
    {
        /// <summary>
        /// Фильтр по имени
        /// </summary>
        public string name;
        /// <summary>
        /// Фильтр по должности
        /// </summary>
        public string post;
        /// <summary>
        /// Фильтр по полу
        /// </summary>
        public char gender;
        /// <summary>
        /// Фильтр по дате приема на работу - нижняя граница
        /// </summary>
        public DateTime recruitmentDateStart;
        /// <summary>
        /// Фильтр по дате приема на работу - верхняя граница
        /// </summary>
        public DateTime recruitmentDateEnd;

        /// <summary>
        /// Установка полей фильтра
        /// </summary>
        public void setFilter()
        {
            // Ввод фильтра по имени
            Console.Write("Введите имя работника: ");
            string str = Console.ReadLine();
            if (str == string.Empty)
                this.name = null;
            else
                this.name = str;

            // Ввод фильтра по должности
            Console.Write("Введите должность работника: ");
            str = Console.ReadLine();
            if (str == string.Empty)
                this.post = null;
            else
                this.post = str;

            // Ввод фильтра по полу
            Console.Write("Введите пол работника (м/ж): ");
            while (true)
            {
                str = Console.ReadLine();
                switch (str.ToLower())
                {
                    case "м":
                        this.gender = 'м';
                        break;

                    case "ж":
                        this.gender = 'ж';
                        break;

                    case "":
                        gender = '\0';
                        break;

                    default:
                        Console.Write("Введите пол работника (м/ж): ");
                        continue;
                }
                break;
            }

            // Ввод фильтра по дате приема на работу - нижняя граница
            Console.Write("Введите дату начала временного интервала: ");
            str = Console.ReadLine();
            if (!DateTime.TryParse(str, out recruitmentDateStart))
                recruitmentDateStart = new DateTime(0);

            // Ввод фильтра по дате приема на работу - верхняя граница
            Console.Write("Введите дату конца временного интервала: ");
            str = Console.ReadLine();
            if (!DateTime.TryParse(str, out recruitmentDateEnd))
                recruitmentDateEnd = new DateTime(0);

            // Проверка на корректность фильтров по дате приема на работу
            if (this.recruitmentDateStart != new DateTime(0) && this.recruitmentDateEnd != new DateTime(0) && this.recruitmentDateStart > this.recruitmentDateEnd)
            {
                DateTime temp = new DateTime();
                temp = this.recruitmentDateStart;
                this.recruitmentDateStart = this.recruitmentDateEnd;
                this.recruitmentDateEnd = temp;
            }

            // Вывод информации о полях фильтра на консоль
            if (this.name != null)
                Console.WriteLine($"Фильтрация по имени: { this.name }");
            else
                Console.WriteLine("Фильтрация по имени не задана");
            if (this.post != null)
                Console.WriteLine($"Фильтрация по должности: { this.post }");
            else
                Console.WriteLine("Фильтрация по должности не задана");
            switch (this.gender)
            {
                case 'м':
                    Console.WriteLine("Фильтрация по полу: мужской");
                    break;

                case 'ж':
                    Console.WriteLine("Фильтрация по полу: женский");
                    break;

                case '\0':
                    Console.WriteLine("Фильтрация по полу не задана");
                    break;
            }
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
