using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    public class SaveTransactions
    {
        internal static void Saving(string path, List<Transaction> transactions)
        {
            try
            {
                var lines = transactions.Select(t =>
                    $"{t.Date:yyyy-MM-dd} {t.Amount} {t.Category} {t.Description}");
                File.WriteAllLines(path, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }
    }
}
