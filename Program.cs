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
    /// Cтруктура для представления отдельного работника.
    /// </summary>
    struct Worker
    {
        /// <summary>
        /// Перечисление для представления пола работника.
        /// </summary>
        public enum GenderEnum
        {
            MALE,
            FEMALE
        }

        /// <summary>
        /// Список, в котором хранятся все добавленные работники.
        /// </summary>
        private static List<Worker> workers = new List<Worker>();

        /// <summary>
        /// Полное имя работника.
        /// </summary>
        string Name;
        /// <summary>
        /// Должность работника.
        /// </summary>
        string Position;
        /// <summary>
        /// Пол работника.
        /// </summary>
        GenderEnum Gender;
        /// <summary>
        /// Дата найма работника.
        /// </summary>
        DateTime HireDate;

        /// <summary>
        /// Метод для добавления новых работников путём ввода их параметров
        /// через консоль.
        /// </summary>
        public static void AddWorker()
        {
            // Создание работника
            Worker worker = new Worker();

            // Заполнение структуры работника
            Console.Write("Введите полное имя работника: ");
            worker.Name = ReadString();
            Console.Write("Введите должность работника: ");
            worker.Position = ReadString();
            Console.Write("Введите пол работника (м/ж): ");
            worker.Gender = ReadGender();
            Console.Write("Введите дату найма работника: ");
            worker.HireDate = ReadDate();

            // Сохранение работника в список
            workers.Add(worker);
        }

        /// <summary>
        /// Метод для вывода всех работников на консоль с их подсчётом.
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
        /// Метод для ввода строки. Зацикливается до ввода непустой строки.
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
        /// Метод для ввода пола работника. Зацикливается до ввода правильного
        /// значения для пола работника.
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
        /// Метод для ввода даты и времени. Зацикливается до ввода правильного
        /// значения для даты и времени.
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
        /// Метод для вывода информации об одном работнике на консоль.
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
        /// Метод для вывода отфильтрованного списка работников на консоль.
        /// <param name = "filter"> Ссылка на используемый фильтр </param>
        /// </summary>
        public static void PrintFilteredWorkers(ref Filter filter)
        {
            foreach (Worker worker in workers)
            {
                // Если значение фильтра по имени задано и не совпадает с именем работника, этот работник пропускается
                if (filter.name != null && worker.Name != filter.name)
                    continue;

                // Если значение фильтра по должности задано и не совпадает с должностью работника, этот работник пропускается
                if (filter.post != null && worker.Position != filter.post)
                    continue;

                // Если значение фильтра по полу задано и не совпадает с полом работника, этот работник пропускается
                if (filter.gender != '\0')
                {
                    if (worker.Gender == Worker.GenderEnum.MALE && filter.gender == 'ж')
                        continue;

                    if (worker.Gender == Worker.GenderEnum.FEMALE && filter.gender == 'м')
                        continue;
                }

                // Если фильтры по дате не заданы, информация о работнике выводится на консоль
                if (filter.recruitmentDateStart == new DateTime(0) && filter.recruitmentDateEnd == new DateTime(0))
                {
                    worker.PrintSingleWorker();
                    continue;
                }

                // Если нижняя граница фильтра не задана, но верхняя задана, она сравнивается с датой приема на работу работника
                if (filter.recruitmentDateStart != new DateTime(0) && filter.recruitmentDateEnd == new DateTime(0))
                {
                    if (worker.HireDate < filter.recruitmentDateEnd)
                        worker.PrintSingleWorker();
                    continue;
                }

                // Если оба значения фильтра по дате заданы, проверяется соответствие с датой приема на работу работника
                if (filter.recruitmentDateStart != new DateTime(0) && filter.recruitmentDateEnd != new DateTime(0))
                {
                    if (worker.HireDate >= filter.recruitmentDateStart && worker.HireDate <= filter.recruitmentDateEnd)
                        worker.PrintSingleWorker();
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// Структура для представления фильтра
    /// </summary>
    struct Filter
    {
        /// <summary>
        /// Полное имя работника
        /// </summary>
        public string name;
        /// <summary>
        /// Должность работника
        /// </summary>
        public string post;
        /// <summary>
        /// Пол работника
        /// </summary>
        public char gender;
        /// <summary>
        /// Начало временного периода для фильтрации
        /// </summary>
        public DateTime recruitmentDateStart;
        /// <summary>
        /// Конец временного периода для фильтрации
        /// </summary>
        public DateTime recruitmentDateEnd;

        /// <summary>
        /// Метод для установки полей фильтра
        /// </summary>
        public void setFilter()
        {
            // Если введена пустая строка, то фильтрация по имени не задана
            Console.Write("Введите полное имя работника: ");
            string str = Console.ReadLine();
            if (str == string.Empty)
                this.name = null;
            else
                this.name = str;

            // Если введена пустая строка, то фильтрация по должности не задана
            Console.Write("Введите должность работника: ");
            str = Console.ReadLine();
            if (str == string.Empty)
                this.post = null;
            else
                this.post = str;

            // Если введена пустая строка, то фильтрация по полу не задана
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

            // Если введена пустая строка, то нижняя граница интервала не задана
            Console.Write("Введите дату начала временного интервала: ");
            str = Console.ReadLine();
            if (!DateTime.TryParse(str, out recruitmentDateStart))
                recruitmentDateStart = new DateTime(0);

            // Если введена пустая строка, то верхняя граница интервала не задана
            Console.Write("Введите дату конца временного интервала: ");
            str = Console.ReadLine();
            if (!DateTime.TryParse(str, out recruitmentDateEnd))
                recruitmentDateEnd = new DateTime(0);

            // Если заданы оба значения филтра по дате, и дата конца меньше, чем дата начала, то они меняются местами
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
