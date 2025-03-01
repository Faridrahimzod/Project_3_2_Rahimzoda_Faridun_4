using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;


namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс, отвечающий за удаление транзакций из списка.
    /// </summary>
    public class DeleteTransaction
    {
        /// <summary>
        /// Метод для удаления транзакции по её номеру в списке.
        /// </summary>
        /// <param name="transactions">Список транзакций, из которого нужно удалить транзакцию.</param>
        public static void Deleting(List<Transaction> transactions)
        {
            // Запрос номера транзакции для удаления
            Console.Write("Номер транзакции для удаления: ");

            // Проверка корректности введённого номера
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= transactions.Count)
            {
                // Удаление транзакции по индексу (индекс уменьшается на 1, так как пользователь вводит номер, начиная с 1)
                transactions.RemoveAt(index - 1);
                Console.WriteLine("Транзакция удалена.");
            }
            else
            {
                // Сообщение об ошибке, если номер транзакции некорректен
                Console.WriteLine("Неверный номер!");
            }
        }
    }
}