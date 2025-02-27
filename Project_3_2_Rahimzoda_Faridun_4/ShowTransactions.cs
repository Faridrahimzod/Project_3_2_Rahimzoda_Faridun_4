using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    public class ShowTransactions
    {
        public static void Showing(List<Transaction> transactions)
        {
            Console.WriteLine("\n| №  | Дата       | Сумма     | Категория | Описание");
            Console.WriteLine("|---|---|---|---|---|");
            for (int i = 0; i < transactions.Count; i++)
            {
                var t = transactions[i];
                Console.WriteLine($"| {i + 1,-2} | {t.Date:yyyy-MM-dd} | {t.Amount,8} | {t.Category,-10} | {t.Description}");
            }

            int? income = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            int? expense = transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
            Console.WriteLine($"\nИтого доход: {income}");
            Console.WriteLine($"Итого расход: {expense}");
            Console.WriteLine($"Баланс: {income + expense}");
        }
    }
}
