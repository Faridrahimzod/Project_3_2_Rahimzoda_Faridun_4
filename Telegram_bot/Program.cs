using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram_bot;

// Добавим токен тг бота @Project_3_2_4var_bot
var botToken = "7610536088:AAFiZdKgiD0GBzh5GEzjghYvS6v7M11YH2o";
var botClient = new TelegramBotClient(botToken);

// Настройки для обработки обновлений
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // Разрешаем обработку всех типов обновлений
};

// Запуск бота с обработчиками обновлений и ошибок
botClient.StartReceiving(
    updateHandler: new Telegram.Bot.Polling.DefaultUpdateHandler(
        HandleUpdateAsync, // Обработчик входящих сообщений
        HandleErrorAsync), // Обработчик ошибок
    receiverOptions: receiverOptions);

Console.WriteLine("Бот запущен. Нажмите любую клавишу для выхода...");
Console.ReadKey();

/// <summary>
/// Обрабатывает входящие обновления (сообщения) от пользователей.
/// </summary>
/// <param name="botClient">Клиент Telegram Bot API.</param>
/// <param name="update">Обновление, содержащее сообщение.</param>
/// <param name="ct">Токен отмены для асинхронной операции.</param>
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
{
    // Проверка, что обновление содержит сообщение
    if (update.Message is not { } message)
        return;

    // Получение ID пользователя
    var userId = message.From?.Id ?? 0;

    // Создание сервиса для обработки сообщений
    var botService = new BotService(botToken, userId);

    try
    {
        // Обработка сообщения
        await botService.HandleMessageAsync(message);
    }
    catch (Exception ex)
    {
        // Логирование ошибок
        Console.WriteLine($"Ошибка: {ex}");
    }
}

/// <summary>
/// Обрабатывает ошибки, возникающие при работе бота.
/// </summary>
/// <param name="botClient">Клиент Telegram Bot API.</param>
/// <param name="exception">Исключение, которое произошло.</param>
/// <param name="ct">Токен отмены для асинхронной операции.</param>
/// <returns>Задача, представляющая асинхронную операцию.</returns>
Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
{
    // Логирование ошибки
    Console.WriteLine($"Ошибка: {exception}");
    return Task.CompletedTask;
}