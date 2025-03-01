using ScottPlot;
using ScottPlot.TickGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using Project_3_2_Rahimzoda_Faridun_4;

namespace Telegram_bot
{
    /// <summary>
    /// Класс для генерации графиков с использованием библиотеки ScottPlot.
    /// </summary>
    public class PlotService
    {
        /// <summary>
        /// Генерирует график динамики доходов и расходов по месяцам.
        /// </summary>
        /// <param name="transactions">Список транзакций для анализа.</param>
        /// <returns>Массив байтов, представляющий изображение графика в формате PNG.</returns>
        public byte[] GenerateMonthlyTrendsPlot(List<Transaction> transactions)
        {
            var plot = new Plot();

            // Группировка транзакций по месяцам
            var monthlyData = transactions
                .Where(t => t.Date.HasValue) // Фильтрация транзакций с указанной датой
                .GroupBy(t => new
                {
                    Year = t.Date.Value.Year,  // Год транзакции
                    Month = t.Date.Value.Month // Месяц транзакции
                })
                .Select(g => new
                {
                    Label = $"{g.Key.Year}-{g.Key.Month:D2}", // Метка для оси X (ГГГГ-ММ)
                    Income = g.Where(t => t.Amount > 0).Sum(t => t.Amount), // Сумма доходов
                    Expense = g.Where(t => t.Amount < 0).Sum(t => t.Amount) // Сумма расходов
                })
                .OrderBy(d => d.Label) // Сортировка по метке
                .ToList();

            // Проверка наличия данных
            if (!monthlyData.Any())
                return Array.Empty<byte>();

            // Генерация позиций и значений для графика
            double[] positions = Enumerable.Range(0, monthlyData.Count).Select(i => (double)i).ToArray();
            double[] incomeValues = monthlyData.Select(m => (double)m.Income).ToArray();
            double[] expenseValues = monthlyData.Select(m => (double)m.Expense).ToArray();

            // Добавление столбцов для доходов
            var barIncome = plot.Add.Bars(positions, incomeValues);
            barIncome.Color = Colors.Green.WithAlpha(0.5); // Зелёный цвет для доходов

            // Добавление столбцов для расходов
            var barExpense = plot.Add.Bars(positions, expenseValues);
            barExpense.Color = Colors.Red.WithAlpha(0.5); // Красный цвет для расходов

            // Настройка оси X
            plot.Axes.Bottom.Label.Text = "Месяц"; // Подпись оси X
            plot.Axes.Bottom.TickGenerator = new NumericAutomatic(); // Автоматическая генерация меток

            // Настройка оси Y
            plot.Axes.Left.Label.Text = "Сумма (руб)"; // Подпись оси Y

            // Возвращение графика в виде массива байтов (PNG)
            return plot.GetImageBytes(800, 600);
        }
    }
}