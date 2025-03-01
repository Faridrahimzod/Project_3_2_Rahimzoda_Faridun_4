using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс, отвечающий за добавление новой транзакции.
    /// </summary>
    public class AddTransaction
    {
        /// <summary>
        /// Метод для добавления новой транзакции. Запрашивает у пользователя данные о транзакции.
        /// </summary>
        /// <returns>Возвращает объект Transaction с данными, введёнными пользователем.</returns>
        public static Transaction Adding()
        {
            var transaction = new Transaction();

            // Запрос даты транзакции
            Console.Write("Дата (ГГГГ-ММ-ДД): ");
            transaction.Date = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", null);

            // Запрос суммы транзакции
            Console.Write("Сумма (+/-): ");
            transaction.Amount = Int32.Parse(Console.ReadLine());

            // Выбор категории транзакции
            Console.WriteLine("Категории: " + string.Join(", ", Program.categories));
            Console.Write("Категория: ");
            string category = Console.ReadLine();

            // Проверка, существует ли введённая категория. Если нет, используется категория "Другое".
            transaction.Category = (Program.categories).Contains(category) ? category : "Другое";

            // Запрос описания транзакции
            Console.Write("Описание: ");
            transaction.Description = Console.ReadLine();

            return transaction;
        }
    }
}
