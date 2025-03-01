using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс, представляющий бюджет для определённой категории расходов.
    /// </summary>
    public class Budget
    {
        /// <summary>
        /// Получает или задает категорию, для которой установлен бюджет.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Получает или задает месячный лимит расходов для данной категории.
        /// </summary>
        public decimal MonthlyLimit { get; set; }
    }
}