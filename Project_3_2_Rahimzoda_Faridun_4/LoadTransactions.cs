using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс, отвечающий за загрузку транзакций из файла.
    /// </summary>
    public static class LoadTransactions
    {
        /// <summary>
        /// Метод для загрузки транзакций из указанного файла.
        /// </summary>
        /// <param name="path">Путь к файлу, содержащему транзакции.</param>
        /// <returns>Список загруженных транзакций.</returns>
        public static List<Transaction> Loading(string path)
        {
            List<Transaction> res = new List<Transaction>();
            try
            {
                // Чтение всех строк из файла
                foreach (var line in File.ReadLines(path))
                {
                    // Разделение строки на части: дата, сумма, категория, описание
                    var parts = line.Split(new[] { ' ' }, 4);

                    // Создание новой транзакции и добавление её в список
                    res.Add(new Transaction
                    {
                        Date = DateTime.ParseExact(parts[0], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Amount = Int32.Parse(parts[1]),
                        Category = parts[2],
                        Description = parts[3]
                    });
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок при загрузке файла
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            }

            // Возвращение списка транзакций
            return res;
        }
    }
}