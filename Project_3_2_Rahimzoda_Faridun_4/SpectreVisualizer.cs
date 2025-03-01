// SpectreVisualizer.cs
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс для визуализации данных с использованием библиотеки Spectre.Console.
    /// </summary>
    public static class SpectreVisualizer
    {
        /// <summary>
        /// Отображает интерактивную таблицу транзакций с возможностью фильтрации и сортировки.
        /// </summary>
        /// <param name="transactions">Список транзакций для отображения.</param>
        public static void ShowInteractiveTable(List<Transaction> transactions)
        {
            // Фильтрация транзакций по выбранной категории
            var filteredTransactions = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Фильтр категорий:")
                    .PageSize(10)
                    .AddChoices(GetCategoriesWithAllOption(transactions))) switch
            {
                "Все" => transactions,
                var category => transactions.Where(t => t.Category == category).ToList()
            };

            // Сортировка транзакций по выбранному критерию
            var sortedTransactions = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Сортировка:")
                    .AddChoices(new[] { "Дата ↑", "Дата ↓", "Сумма ↑", "Сумма ↓" })) switch
            {
                "Дата ↑" => filteredTransactions.OrderBy(t => t.Date).ToList(),
                "Дата ↓" => filteredTransactions.OrderByDescending(t => t.Date).ToList(),
                "Сумма ↑" => filteredTransactions.OrderBy(t => t.Amount).ToList(),
                "Сумма ↓" => filteredTransactions.OrderByDescending(t => t.Amount).ToList(),
                _ => filteredTransactions
            };

            // Создание и настройка таблицы
            var table = new Table();
            table.Border = TableBorder.Rounded;
            table.AddColumn("[bold]Дата[/]");
            table.AddColumn("[bold]Сумма[/]");
            table.AddColumn(new TableColumn("[bold]Категория[/]").Centered());
            table.AddColumn("[bold]Описание[/]");

            // Добавление строк в таблицу
            foreach (var t in sortedTransactions)
            {
                var amountColor = t.Amount > 0 ? "green" : "red";
                table.AddRow(
                    t.Date?.ToString("yyyy-MM-dd") ?? "",
                    $"[{amountColor}]{t.Amount}[/]",
                    $"[bold]{t.Category}[/]",
                    t.Description ?? ""
                );
            }

            // Отображение таблицы
            AnsiConsole.Write(table);
            AnsiConsole.Console.Input.ReadKey(true);
        }

        /// <summary>
        /// Отображает диаграмму распределения расходов по категориям.
        /// </summary>
        /// <param name="transactions">Список транзакций для анализа.</param>
        public static void ShowExpenseChart(List<Transaction> transactions)
        {
            // Получение расходов по категориям
            var expenses = transactions
                .Where(t => t.Amount < 0)
                .GroupBy(t => t.Category ?? "Другое") // Обработка null-категорий
                .Select(g => new
                {
                    Category = g.Key,
                    Total = Math.Abs(g.Sum(t => t.Amount ?? 0))
                })
                .OrderByDescending(g => g.Total)
                .ToList();

            // Проверка наличия данных
            if (!expenses.Any())
            {
                AnsiConsole.MarkupLine("[red]Нет данных о расходах.[/]");
                AnsiConsole.Console.Input.ReadKey(true);
                return;
            }

            // Создание и настройка диаграммы
            var chart = new BarChart()
                .Width(60)
                .Label("[bold]Распределение расходов[/]")
                .CenterLabel();

            // Добавление данных в диаграмму
            foreach (var item in expenses)
            {
                chart.AddItem(
                    item.Category,
                    item.Total,
                    Spectre.Console.Color.Red
                );
            }

            // Отображение диаграммы
            AnsiConsole.Write(chart);
            AnsiConsole.Console.Input.ReadKey(true);
        }

        /// <summary>
        /// Возвращает список категорий с опцией "Все".
        /// </summary>
        /// <param name="transactions">Список транзакций для извлечения категорий.</param>
        /// <returns>Список категорий.</returns>
        private static List<string> GetCategoriesWithAllOption(List<Transaction> transactions)
        {
            var categories = transactions
                .Select(t => t.Category ?? "Другое")
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            categories.Insert(0, "Все");
            return categories;
        }

        /// <summary>
        /// Отображает отчёт о бюджетах и фактических расходах.
        /// </summary>
        /// <param name="transactions">Список транзакций для анализа.</param>
        /// <param name="budgetManager">Менеджер бюджетов.</param>
        public static void ShowBudgetReport(
            List<Transaction> transactions,
            BudgetManager budgetManager)
        {
            // Получение расходов за текущий месяц
            var expenses = transactions
                .Where(t => t.Amount < 0 && t.Date?.Month == DateTime.Now.Month && t.Date?.Year == DateTime.Now.Year)
                .GroupBy(t => t.Category ?? "Другое")
                .ToDictionary(g => g.Key, g => Math.Abs(g.Sum(t => t.Amount ?? 0)));

            var budgets = budgetManager.GetBudgets();

            // Создание и настройка таблицы
            var table = new Table();
            table.Border = TableBorder.Rounded;
            table.AddColumns(
                new TableColumn("[bold]Категория[/]"),
                new TableColumn("[bold]Бюджет[/]").RightAligned(),
                new TableColumn("[bold]Факт[/]").RightAligned(),
                new TableColumn("[bold]% выполнения[/]").RightAligned()
            );

            // Добавление данных в таблицу
            foreach (var category in budgets.Keys.Union(expenses.Keys))
            {
                budgets.TryGetValue(category, out var budget);
                expenses.TryGetValue(category, out var expense);

                var percentage = budget > 0
                    ? Math.Round(expense / budget * 100, 1)
                    : 0;

                var statusColor = percentage > 100 ? "red" : "green";
                var budgetColor = budget == 0 ? "yellow" : "default";

                table.AddRow(
                    $"{category}",
                    $"[{budgetColor}]{budget:N2}[/]",
                    $"[{(expense > budget ? "red" : "green")}]{expense:N2}[/]",
                    $"[{statusColor}]{percentage}%[/]"
                );
            }

            // Отображение таблицы
            AnsiConsole.Write(table);
            AnsiConsole.Console.Input.ReadKey(true);
        }

        /// <summary>
        /// Отображает прогноз расходов на следующий месяц.
        /// </summary>
        /// <param name="forecastService">Сервис для прогнозирования расходов.</param>
        public static void ShowForecast(ForecastService forecastService)
        {
            var forecast = forecastService.GetNextMonthForecast();

            // Проверка наличия данных
            if (!forecast.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Недостаточно данных для прогноза[/]");
                return;
            }

            // Создание и настройка таблицы
            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("Прогноз расходов на следующий месяц")
                .AddColumn("[bold]Категория[/]")
                .AddColumn("[bold]Средний расход[/]");

            // Добавление данных в таблицу
            foreach (var item in forecast.OrderByDescending(x => x.Value))
            {
                table.AddRow(
                    item.Key,
                    $"[red]{item.Value:N2}[/]"
                );
            }

            // Отображение таблицы
            AnsiConsole.Write(table);
            AnsiConsole.Console.Input.ReadKey(true);
        }
    }
}