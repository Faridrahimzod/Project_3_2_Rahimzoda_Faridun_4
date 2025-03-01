using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс, отвечающий за сохранение транзакций в файл.
    /// </summary>
    public class SaveTransactions
    {
        /// <summary>
        /// Метод для сохранения списка транзакций в указанный файл.
        /// </summary>
        /// <param name="path">Путь к файлу, в который будут сохранены транзакции.</param>
        /// <param name="transactions">Список транзакций для сохранения.</param>
        internal static void Saving(string path, List<Transaction> transactions)
        {
            try
            {
                // Преобразование каждой транзакции в строку формата: "ГГГГ-ММ-ДД Сумма Категория Описание"
                var lines = transactions.Select(t =>
                    $"{t.Date:yyyy-MM-dd} {t.Amount} {t.Category} {t.Description}");

                // Запись всех строк в файл
                File.WriteAllLines(path, lines);
            }
            catch (Exception ex)
            {
                // Обработка ошибок при сохранении файла
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }
    }
}