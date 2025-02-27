using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    public class Transaction
    {
        public DateTime? Date { get; set; }
        public int? Amount { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public Transaction()
        {

        }
        public Transaction(DateTime date, int amount, string category, string description)
        {
            Date = date;
            Amount = amount;
            Category = category;
            Description = description;
        }
    }
}
