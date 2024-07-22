using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticTask1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Словарь сотрудников и их отпусков
            var vacationDictionary = new Dictionary<string, List<DateTime>>()
            {
                ["Иванов Иван Иванович"] = new List<DateTime>(),
                ["Петров Петр Петрович"] = new List<DateTime>(),
                ["Юлина Юлия Юлиановна"] = new List<DateTime>(),
                ["Сидоров Сидор Сидорович"] = new List<DateTime>(),
                ["Павлов Павел Павлович"] = new List<DateTime>(),
                ["Георгиев Георг Георгиевич"] = new List<DateTime>()
            };

            // Список рабочих дней недели
            var availableWorkingDays = new List<DayOfWeek>()
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };

            // Список праздничных дней
            var holidays = new List<DateTime>()
            {
                new DateTime(DateTime.Now.Year, 1, 1),  // Новый год
                new DateTime(DateTime.Now.Year, 1, 7),  // Рождество
                new DateTime(DateTime.Now.Year, 2, 23), // День защитника Отечества
                new DateTime(DateTime.Now.Year, 3, 8),  // Международный женский день
                new DateTime(DateTime.Now.Year, 5, 1),  // Праздник Весны и Труда
                new DateTime(DateTime.Now.Year, 5, 9),  // День Победы
                new DateTime(DateTime.Now.Year, 6, 12), // День России
                new DateTime(DateTime.Now.Year, 11, 4)  // День народного единства
            };

            // Общий список отпусков
            var vacations = new List<DateTime>();
            var random = new Random();

            foreach (var employee in vacationDictionary.Keys.ToList())
            {
                int remainingVacationDays = 28; // Общее количество отпускных дней для каждого сотрудника

                while (remainingVacationDays > 0)
                {
                    // Генерация случайной даты начала отпуска
                    DateTime startDate = GenerateRandomDate(random);

                    // Проверка, что отпуск начинается в будний день и не попадает на праздничный день
                    if (availableWorkingDays.Contains(startDate.DayOfWeek) && !holidays.Contains(startDate))
                    {
                        // Выбор случайной продолжительности отпуска (7 или 14 дней)
                        int vacationDuration = random.Next(2) == 0 ? 7 : 14;
                        if (remainingVacationDays < vacationDuration)
                        {
                            vacationDuration = remainingVacationDays;
                        }
                        DateTime endDate = startDate.AddDays(vacationDuration);

                        // Проверка пересечения с другими отпусками и праздничными днями
                        bool hasOverlap = vacations.Any(date => date >= startDate && date < endDate);
                        bool hasHoliday = holidays.Any(date => date >= startDate && date < endDate);
                        if (!hasOverlap && !hasHoliday)
                        {
                            // Проверка минимального интервала между отпусками одного сотрудника
                            bool hasMinimumInterval = vacationDictionary[employee].Any(date =>
                                (date >= startDate.AddMonths(-1) && date <= endDate.AddMonths(1)) ||
                                (date >= endDate.AddMonths(-1) && date <= startDate.AddMonths(1)));

                            if (!hasMinimumInterval)
                            {
                                // Добавление отпуска в списки
                                for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
                                {
                                    if (!holidays.Contains(date)) // Исключаем праздничные дни
                                    {
                                        vacations.Add(date);
                                        vacationDictionary[employee].Add(date);
                                    }
                                }
                                remainingVacationDays -= vacationDuration;
                            }
                        }
                    }
                }
            }

            // Вывод результатов
            foreach (var vacationList in vacationDictionary)
            {
                Console.WriteLine("Дни отпуска " + vacationList.Key + " : ");
                foreach (var date in vacationList.Value)
                {
                    Console.WriteLine(date.ToShortDateString());
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        // Метод для генерации случайной даты в текущем году
        static DateTime GenerateRandomDate(Random random)
        {
            DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime end = new DateTime(DateTime.Now.Year, 12, 31);
            int range = (end - start).Days;
            return start.AddDays(random.Next(range));
        }
    }
}
