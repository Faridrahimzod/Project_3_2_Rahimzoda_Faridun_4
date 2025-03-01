///Рахимзода Фаридун Тоджиддин
///БПИ244-1
///Вариант 4
///Выбран B-side в качаестве продолжения

using Project_3_2_Rahimzoda_Faridun_4;
using Spectre.Console;

/// <summary>
/// Главный класс программы, отвечающий за управление личными финансами.
/// </summary>
class Program
{
    private static List<Transaction> transactions = new List<Transaction>();
    internal static readonly string[] categories = { "Продукты", "Транспорт", "Развлечение", "Коммунальные платежи", "Зарплата", "Другое" };
    private static BudgetManager _budgetManager = new BudgetManager();
    private static ForecastService _forecastService;

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    static void Main()
    {
        Console.WriteLine("Введите путь к файлу транзакций:");
        string? filePath = Console.ReadLine();

        // Проверка существования файла и создание нового, если он отсутствует
        if (!File.Exists(filePath) && !String.IsNullOrEmpty(filePath))
        {
            Console.WriteLine("Файл не найден. Создан новый файл.");
            File.Create(filePath).Close();
        }

        // Загрузка транзакций и бюджетов
        do
        {
            try
            {
                transactions = LoadTransactions.Loading(filePath);
                _budgetManager.LoadBudgets("budgets.txt");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        } while (true);

        _forecastService = new ForecastService(transactions);

        // Основной цикл меню
        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Просмотр всех транзакций");
            Console.WriteLine("2. Добавить транзакцию");
            Console.WriteLine("3. Удалить транзакцию");
            Console.WriteLine("4. Интерактивная таблица");
            Console.WriteLine("5. Диаграмма расходов");
            Console.WriteLine("6. Управление бюджетами");
            Console.WriteLine("7. Прогноз расходов");
            Console.WriteLine("8. Анализ трендов");
            Console.WriteLine("9. Выход");
            Console.Write("Выберите действие: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowTransactions.Showing(transactions);
                    break;
                case "2":
                    transactions.Add(AddTransaction.Adding());
                    break;
                case "3":
                    DeleteTransaction.Deleting(transactions);
                    break;
                case "4":
                    SpectreVisualizer.ShowInteractiveTable(transactions);
                    break;
                case "5":
                    SpectreVisualizer.ShowExpenseChart(transactions);
                    break;
                case "6":
                    ManageBudgets();
                    break;
                case "7":
                    SpectreVisualizer.ShowForecast(_forecastService);
                    break;
                case "8":
                    ShowTrendsMenu();
                    break;
                case "9":
                    SaveTransactions.Saving(filePath, transactions);
                    using (FileStream fs = new FileStream("budgets.txt", FileMode.Truncate))
                    {
                        // Используется для удаления данных в budgets.txt
                    }
                    return;

                default:
                    Console.WriteLine("Неверный ввод!");
                    break;
            }
        }
    }

    /// <summary>
    /// Управление бюджетами: добавление и просмотр бюджетов.
    /// </summary>
    static void ManageBudgets()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Управление бюджетами")
                .AddChoices("Добавить бюджет", "Просмотр бюджетов", "Назад"));

        switch (choice)
        {
            case "Добавить бюджет":
                var category = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Категория")
                        .AddChoices(Program.categories));

                var limit = AnsiConsole.Ask<decimal>("Лимит бюджета:");
                _budgetManager.AddBudget(new Budget { Category = category, MonthlyLimit = limit });
                _budgetManager.SaveBudgets("budgets.txt");
                break;

            case "Просмотр бюджетов":
                Console.WriteLine("Статистика по бюджетированию за этот месяц");
                SpectreVisualizer.ShowBudgetReport(transactions, _budgetManager);
                break;
        }
    }

    /// <summary>
    /// Меню для анализа трендов: динамика за месяц и гистограмма категорий.
    /// </summary>
    static void ShowTrendsMenu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Выберите тип анализа:")
                .AddChoices(
                    "Динамика за месяц",
                    "Гистограмма категорий",
                    "Назад"));

        switch (choice)
        {
            case "Динамика за месяц":
                ScottPlotVisualizer.PlotMonthlyTrends(transactions);
                break;

            case "Гистограмма категорий":
                ScottPlotVisualizer.PlotCategoryHistogram(transactions);
                break;
        }
    }
}