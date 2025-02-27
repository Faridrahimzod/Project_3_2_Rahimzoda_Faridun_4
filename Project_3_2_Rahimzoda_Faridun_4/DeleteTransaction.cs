using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    public class DeleteTransaction
    {
        public static void Deleting(List<Transaction> transactions)
        {
            Console.Write("Номер транзакции для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= transactions.Count)
            {
                transactions.RemoveAt(index - 1);
                Console.WriteLine("Транзакция удалена.");
            }
            else
            {
                Console.WriteLine("Неверный номер!");
            }
        }
    }
}
