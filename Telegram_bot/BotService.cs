using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram_bot;
using Project_3_2_Rahimzoda_Faridun_4;

namespace Telegram_bot
{
    /// <summary>
    /// Класс для обработки сообщений и управления взаимодействием с пользователем в Telegram-боте.
    /// </summary>
    public class BotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly DataService _dataService;
        private readonly PlotService _plotService;

        /// <summary>
        /// Инициализирует новый экземпляр класса BotService.
        /// </summary>
        /// <param name="token">Токен Telegram-бота.</param>
        /// <param name="userId">Идентификатор пользователя для управления данными.</param>
        public BotService(string token, long userId)
        {
            _botClient = new TelegramBotClient(token);
            _dataService = new DataService(userId);
            _plotService = new PlotService();
        }

        /// <summary>
        /// Обрабатывает входящие сообщения от пользователя.
        /// </summary>
        /// <param name="message">Сообщение от пользователя.</param>
        public async Task HandleMessageAsync(Message message)
        {
            switch (message.Text)
            {
                case "/start":
                    await ShowMainMenu(message.Chat.Id);
                    break;
                case "➕ Добавить транзакцию":
                    await RequestTransactionInput(message.Chat.Id);
                    break;
                case "📋 Список транзакций":
                    await ShowTransactions(message.Chat.Id);
                    break;
                case "📊 Статистика":
                    await ShowStatistics(message.Chat.Id);
                    break;
                default:
                    if (message.Text.Contains('-'))
                        await ProcessTransactionInput(message);
                    break;
            }
        }

        /// <summary>
        /// Отображает главное меню с доступными действиями.
        /// </summary>
        /// <param name="chatId">Идентификатор чата для отправки меню.</param>
        private async Task ShowMainMenu(long chatId)
        {
            var menu = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("➕ Добавить транзакцию") },
                new[] { new KeyboardButton("📋 Список транзакций"), new KeyboardButton("📊 Статистика") }
            })
            { ResizeKeyboard = true };

            await _botClient.SendTextMessageAsync(chatId, "Выберите действие:", replyMarkup: menu);
        }

        /// <summary>
        /// Запрашивает у пользователя данные для добавления новой транзакции.
        /// </summary>
        /// <param name="chatId">Идентификатор чата для отправки запроса.</param>
        private async Task RequestTransactionInput(long chatId)
        {
            await _botClient.SendTextMessageAsync(chatId,
                "Введите транзакцию в формате:\n" +
                "ГГГГ-ММ-ДД Сумма Категория Описание\n" +
                "Пример: 2024-05-20 -1500 Продукты Покупка еды");
        }

        /// <summary>
        /// Обрабатывает ввод пользователя для добавления новой транзакции.
        /// </summary>
        /// <param name="message">Сообщение с данными транзакции.</param>
        private async Task ProcessTransactionInput(Message message)
        {
            try
            {
                // Разделение введённых данных на части
                var parts = message.Text.Split(' ', 4);
                var transaction = new Transaction
                {
                    Date = DateTime.Parse(parts[0]), // Дата транзакции
                    Amount = Int32.Parse(parts[1]),  // Сумма транзакции
                    Category = parts[2],             // Категория транзакции
                    Description = parts[3]           // Описание транзакции
                };

                // Загрузка текущих транзакций, добавление новой и сохранение
                var transactions = _dataService.LoadTransactions();
                transactions.Add(transaction);
                _dataService.SaveTransactions(transactions);

                await _botClient.SendTextMessageAsync(message.Chat.Id, "✅ Транзакция добавлена!");
            }
            catch
            {
                // Обработка ошибок при неверном формате данных
                await _botClient.SendTextMessageAsync(message.Chat.Id, "❌ Ошибка формата данных");
            }
        }

        /// <summary>
        /// Отображает последние транзакции пользователя.
        /// </summary>
        /// <param name="chatId">Идентификатор чата для отправки списка транзакций.</param>
        private async Task ShowTransactions(long chatId)
        {
            var transactions = _dataService.LoadTransactions();
            var response = "📋 Последние транзакции:\n\n" +
                           string.Join("\n", transactions
                               .OrderByDescending(t => t.Date) // Сортировка по дате
                               .Take(10)                      // Ограничение до 10 транзакций
                               .Select(t => $"{t.Date:dd.MM.yy} {t.Amount}₽ ({t.Category}) {t.Description}"));

            await _botClient.SendTextMessageAsync(chatId, response);
        }

        /// <summary>
        /// Отображает статистику доходов и расходов в виде графика.
        /// </summary>
        /// <param name="chatId">Идентификатор чата для отправки графика.</param>
        private async Task ShowStatistics(long chatId)
        {
            var transactions = _dataService.LoadTransactions();
            var plotBytes = _plotService.GenerateMonthlyTrendsPlot(transactions);

            // Отправка графика как изображения
            await using var stream = new MemoryStream(plotBytes);
            await _botClient.SendPhotoAsync(
                chatId: chatId,
                photo: InputFile.FromStream(stream, "chart.png"),
                caption: "📈 Статистика доходов/расходов");
        }
    }
}