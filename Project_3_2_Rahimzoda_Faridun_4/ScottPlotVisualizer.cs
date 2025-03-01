using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.TickGenerators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс для визуализации данных с использованием библиотеки ScottPlot.
    /// </summary>
    public static class ScottPlotVisualizer
    {
        /// <summary>
        /// Строит график динамики доходов и расходов по месяцам.
        /// </summary>
        /// <param name="transactions">Список транзакций для анализа.</param>
        public static void PlotMonthlyTrends(List<Transaction> transactions)
        {
            var plot = new Plot();

            // Подготовка данных: группировка транзакций по месяцам
            var monthlyData = transactions
                .Where(t => t.Date.HasValue)
                .GroupBy(t => new DateTime(t.Date.Value.Year, t.Date.Value.Month, 1))
                .Select(g => new {
                    Month = g.Key,
                    Income = g.Where(t => t.Amount > 0).Sum(t => t.Amount) ?? 0, // Сумма доходов
                    Expense = Math.Abs(g.Where(t => t.Amount < 0).Sum(t => t.Amount) ?? 0) // Сумма расходов
                })
                .OrderBy(d => d.Month)
                .ToList();

            // Конвертация данных для построения графика
            double[] positions = Enumerable.Range(0, monthlyData.Count).Select(x => (double)x).ToArray();
            double[] income = monthlyData.Select(x => (double)x.Income).ToArray();
            double[] expense = monthlyData.Select(x => (double)x.Expense).ToArray();
            string[] labels = monthlyData.Select(x => x.Month.ToString("MMM yyyy")).ToArray();

            // Добавление столбцов для доходов
            var bars = plot.Add.Bars(positions, income);
            bars.Color = Colors.Green.WithAlpha(0.5); // Зелёный цвет для доходов
            bars.Label = "Доходы";

            // Добавление столбцов для расходов
            var bars2 = plot.Add.Bars(positions, expense);
            bars2.Color = Colors.Red.WithAlpha(0.5); // Красный цвет для расходов
            bars2.Label = "Расходы";

            // Настройка осей
            plot.Axes.Left.Label.Text = "Сумма"; // Подпись оси Y
            plot.Axes.Bottom.Label.Text = "Месяц"; // Подпись оси X
            plot.Axes.Bottom.TickGenerator = new NumericAutomatic(); // Автоматическая генерация меток

            // Настройка заголовка и легенды
            plot.Title("Динамика доходов и расходов");
            plot.ShowLegend();

            // Сохранение и отображение графика
            SaveAndShowPlot(plot, "monthly_trends.png");
        }

        /// <summary>
        /// Строит гистограмму распределения расходов по категориям.
        /// </summary>
        /// <param name="transactions">Список транзакций для анализа.</param>
        public static void PlotCategoryHistogram(List<Transaction> transactions)
        {
            var plot = new Plot();

            // Группировка расходов по категориям
            var categories = transactions
                .Where(t => t.Amount < 0) // Только расходы
                .GroupBy(t => t.Category ?? "Другое") // Группировка по категориям
                .ToDictionary(g => g.Key, g => Math.Abs(g.Sum(t => t.Amount ?? 0))); // Сумма расходов по категориям

            // Конвертация данных для построения гистограммы
            double[] positions = Enumerable.Range(0, categories.Count).Select(x => (double)x).ToArray();
            double[] values = categories.Values.Select(v => (double)v).ToArray();
            string[] labels = categories.Keys.ToArray();

            // Добавление столбцов для гистограммы
            var bars = plot.Add.Bars(positions, values);
            bars.Color = Colors.Blue.WithAlpha(0.5); // Синий цвет для гистограммы

            // Настройка осей
            plot.Axes.Left.Label.Text = "Сумма"; // Подпись оси Y
            plot.Axes.Bottom.Label.Text = "Категории"; // Подпись оси X

            // Настройка заголовка
            plot.Title("Распределение расходов по категориям");

            // Сохранение и отображение графика
            SaveAndShowPlot(plot, "category_histogram.png");
        }

        /// <summary>
        /// Сохраняет график в файл и открывает его.
        /// </summary>
        /// <param name="plot">Объект графика для сохранения.</param>
        /// <param name="filename">Имя файла для сохранения.</param>
        private static void SaveAndShowPlot(Plot plot, string filename)
        {
            // Сохранение графика в PNG-файл
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            plot.SavePng(path, 800, 400);
            Console.WriteLine($"График сохранён: {path}");

            // Открытие файла с графиком
            System.Diagnostics.Process.Start("explorer", path);
        }
    }
}