using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс, отвечающий за отображение списка транзакций и статистики.
    /// </summary>
    public class ShowTransactions
    {
        /// <summary>
        /// Метод для отображения всех транзакций в виде таблицы и вывода итоговой статистики.
        /// </summary>
        /// <param name="transactions">Список транзакций для отображения.</param>
        public static void Showing(List<Transaction> transactions)
        {
            // Заголовок таблицы
            Console.WriteLine("\n| №  | Дата       | Сумма    |  Категория | Описание");
            Console.WriteLine("--------------------------------------------------");

            // Вывод каждой транзакции в виде строки таблицы
            for (int i = 0; i < transactions.Count; i++)
            {
                var t = transactions[i];
                Console.WriteLine($"| {i + 1,-2} | {t.Date:yyyy-MM-dd} | {t.Amount,8} | {t.Category,-10} | {t.Description}");
            }

            // Разделитель таблицы
            Console.WriteLine("--------------------------------------------------");

            // Расчёт и вывод итоговой статистики
            int? income = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            int? expense = transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
            Console.WriteLine($"\nИтого доход: {income}");
            Console.WriteLine($"Итого расход: {expense}");
            Console.WriteLine($"Баланс: {income + expense}");
        }
    }
}