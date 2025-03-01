using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_3_2_Rahimzoda_Faridun_4
{
    /// <summary>
    /// Класс для управления бюджетами по категориям расходов.
    /// </summary>
    public class BudgetManager
    {
        private Dictionary<string, decimal> _budgets = new Dictionary<string, decimal>();

        /// <summary>
        /// Добавляет или обновляет бюджет для указанной категории.
        /// </summary>
        /// <param name="budget">Бюджет, содержащий категорию и месячный лимит.</param>
        public void AddBudget(Budget budget)
        {
            _budgets[budget.Category] = budget.MonthlyLimit;
        }

        /// <summary>
        /// Возвращает текущие бюджеты в виде словаря, где ключ — категория, а значение — лимит.
        /// </summary>
        /// <returns>Словарь с бюджетами.</returns>
        public Dictionary<string, decimal> GetBudgets() => _budgets;

        /// <summary>
        /// Сохраняет бюджеты в файл.
        /// </summary>
        /// <param name="path">Путь к файлу для сохранения.</param>
        public void SaveBudgets(string path)
        {
            // Преобразование словаря бюджетов в строки формата "Категория|Лимит"
            var lines = _budgets.Select(b => $"{b.Key}|{b.Value}");

            // Запись строк в файл
            File.WriteAllLines(path, lines);
        }

        /// <summary>
        /// Загружает бюджеты из файла.
        /// </summary>
        /// <param name="path">Путь к файлу для загрузки.</param>
        public void LoadBudgets(string path)
        {
            _budgets.Clear();

            // Проверка существования файла
            if (!File.Exists(path)) return;

            // Чтение файла построчно
            foreach (var line in File.ReadLines(path))
            {
                var parts = line.Split('|');

                // Проверка корректности формата строки
                if (parts.Length == 2 && decimal.TryParse((parts[1]).Trim(), out var limit))
                {
                    _budgets[parts[0]] = limit;
                }
            }
        }
    }
}