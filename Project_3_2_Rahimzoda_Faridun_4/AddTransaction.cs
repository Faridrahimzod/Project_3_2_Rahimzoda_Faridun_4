using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    public class AddTransaction
    {
        public static Transaction Adding()
        {
            var transaction = new Transaction();

            Console.Write("Дата (ГГГГ-ММ-ДД): ");
            transaction.Date = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", null);

            Console.Write("Сумма (+/-): ");
            transaction.Amount = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Категории: " + string.Join(", ", Program.categories));
            Console.Write("Категория: ");
            string category = Console.ReadLine();
            transaction.Category = (Program.categories).Contains(category) ? category : "Другое";

            Console.Write("Описание: ");
            transaction.Description = Console.ReadLine();

            return transaction;
        }
    }
}
