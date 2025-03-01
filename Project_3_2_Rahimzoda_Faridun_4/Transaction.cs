using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс, представляющий транзакцию (доход или расход).
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Дата транзакции. Может быть null, если дата не указана.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Сумма транзакции. Положительное значение указывает на доход, отрицательное — на расход.
        /// Может быть null, если сумма не указана.
        /// </summary>
        public int? Amount { get; set; }

        /// <summary>
        /// Категория транзакции (например, "Продукты", "Транспорт").
        /// Может быть null, если категория не указана.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Описание транзакции. Может быть null, если описание не указано.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Конструктор по умолчанию. Инициализирует новый экземпляр класса Transaction.
        /// </summary>
        public Transaction()
        {
        }

        /// <summary>
        /// Конструктор для инициализации транзакции с указанными параметрами.
        /// </summary>
        /// <param name="date">Дата транзакции.</param>
        /// <param name="amount">Сумма транзакции.</param>
        /// <param name="category">Категория транзакции.</param>
        /// <param name="description">Описание транзакции.</param>
        public Transaction(DateTime date, int amount, string category, string description)
        {
            Date = date;
            Amount = amount;
            Category = category;
            Description = description;
        }
    }
}