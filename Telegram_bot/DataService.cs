using Newtonsoft.Json;
using Project_3_2_Rahimzoda_Faridun_4;
using System.Collections.Generic;
using System.IO;

namespace Telegram_bot
{
    /// <summary>
    /// Класс для управления данными пользователя (транзакции и бюджеты).
    /// </summary>
    public class DataService
    {
        private readonly string _basePath;

        /// <summary>
        /// Инициализирует новый экземпляр класса DataService.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя для создания уникальной папки с данными.</param>
        public DataService(long userId)
        {
            // Создание уникальной папки для хранения данных пользователя
            _basePath = $"data_{userId}";
            Directory.CreateDirectory(_basePath);
        }

        /// <summary>
        /// Загружает список транзакций пользователя из файла.
        /// </summary>
        /// <returns>Список транзакций. Если файл не существует, возвращает пустой список.</returns>
        public List<Transaction> LoadTransactions()
        {
            var path = Path.Combine(_basePath, "transactions.json");
            return File.Exists(path)
                ? JsonConvert.DeserializeObject<List<Transaction>>(File.ReadAllText(path))
                : new List<Transaction>();
        }

        /// <summary>
        /// Сохраняет список транзакций пользователя в файл.
        /// </summary>
        /// <param name="transactions">Список транзакций для сохранения.</param>
        public void SaveTransactions(List<Transaction> transactions)
        {
            var path = Path.Combine(_basePath, "transactions.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(transactions));
        }

        /// <summary>
        /// Загружает список бюджетов пользователя из файла.
        /// </summary>
        /// <returns>Список бюджетов. Если файл не существует, возвращает пустой список.</returns>
        public List<Budget> LoadBudgets()
        {
            var path = Path.Combine(_basePath, "budgets.json");
            return File.Exists(path)
                ? JsonConvert.DeserializeObject<List<Budget>>(File.ReadAllText(path))
                : new List<Budget>();
        }

        /// <summary>
        /// Сохраняет список бюджетов пользователя в файл.
        /// </summary>
        /// <param name="budgets">Список бюджетов для сохранения.</param>
        public void SaveBudgets(List<Budget> budgets)
        {
            var path = Path.Combine(_basePath, "budgets.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(budgets));
        }
    }
}