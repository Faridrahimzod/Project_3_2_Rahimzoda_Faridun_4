using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Project_3_2_Rahimzoda_Faridun_4;

namespace Telegram_bot
{
    /// <summary>
    /// Класс для обработки обновлений и ошибок в Telegram-боте.
    /// </summary>
    public class DefaultUpdateHandler
    {
        private readonly Func<ITelegramBotClient, Update, CancellationToken, Task> _handleUpdate;
        private readonly Func<ITelegramBotClient, Exception, CancellationToken, Task> _handleError;

        /// <summary>
        /// Инициализирует новый экземпляр класса DefaultUpdateHandler.
        /// </summary>
        /// <param name="handleUpdate">Делегат для обработки входящих обновлений.</param>
        /// <param name="handleError">Делегат для обработки ошибок.</param>
        public DefaultUpdateHandler(
            Func<ITelegramBotClient, Update, CancellationToken, Task> handleUpdate,
            Func<ITelegramBotClient, Exception, CancellationToken, Task> handleError)
        {
            _handleUpdate = handleUpdate;
            _handleError = handleError;
        }

        /// <summary>
        /// Обрабатывает входящие обновления (сообщения) от пользователей.
        /// </summary>
        /// <param name="botClient">Клиент Telegram Bot API.</param>
        /// <param name="update">Обновление, содержащее сообщение.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            => _handleUpdate(botClient, update, cancellationToken);

        /// <summary>
        /// Обрабатывает ошибки, возникающие при работе бота.
        /// </summary>
        /// <param name="botClient">Клиент Telegram Bot API.</param>
        /// <param name="exception">Исключение, которое произошло.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            => _handleError(botClient, exception, cancellationToken);
    }
}