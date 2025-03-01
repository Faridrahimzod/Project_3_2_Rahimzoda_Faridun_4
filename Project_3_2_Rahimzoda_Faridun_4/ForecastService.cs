using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс для прогнозирования расходов на основе исторических данных.
    /// </summary>
    public class ForecastService
    {
        private readonly List<Transaction> _transactions;

        /// <summary>
        /// Инициализирует новый экземпляр класса ForecastService.
        /// </summary>
        /// <param name="transactions">Список транзакций для анализа.</param>
        public ForecastService(List<Transaction> transactions)
        {
            _transactions = transactions;
        }

        /// <summary>
        /// Возвращает прогноз расходов на следующий месяц на основе данных за последние 3 месяца.
        /// </summary>
        /// <returns>Словарь, где ключ — категория, а значение — средний расход за последние 3 месяца.</returns>
        public Dictionary<string, decimal> GetNextMonthForecast()
        {
            var forecast = new Dictionary<string, decimal>();

            // Определение даты, начиная с которой учитываются транзакции (3 месяца назад)
            var threeMonthsAgo = DateTime.Now.AddMonths(-3);

            // Фильтрация и группировка расходов за последние 3 месяца
            var expenses = _transactions
                .Where(t => t.Amount < 0 && t.Date >= threeMonthsAgo) // Только расходы за последние 3 месяца
                .GroupBy(t => t.Category ?? "Другое") // Группировка по категориям
                .ToDictionary(
                    g => g.Key, // Категория
                    g => g.Average(t => Math.Abs(t.Amount ?? 0m)) // Средний расход по категории
                );

            // Заполнение прогноза
            foreach (var category in expenses)
            {
                forecast[category.Key] = category.Value;
            }

            return forecast;
        }
    }
}