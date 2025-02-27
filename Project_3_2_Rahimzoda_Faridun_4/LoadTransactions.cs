using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Transactions;
using System.Security.Cryptography.X509Certificates;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    public static class LoadTransactions
    {
        public static List<Transaction> Loading(string path)
        {
            List<Transaction> res = new List<Transaction>();
            try
            {
                foreach (var line in File.ReadLines(path))
                {
                    var parts = line.Split(new[] { ' ' }, 4);
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
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            }
            return res;
        }
    }
}
