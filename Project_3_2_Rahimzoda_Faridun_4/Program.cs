using Project_3_2_Rahimzoda_Faridun_4;


class Program
{
    private static List<Transaction> transactions = new List<Transaction>();
    internal static readonly string[] categories = { "Продукты", "Транспорт", "Развлечение", "Коммунальные платежи", "Зарплата", "Другое" };

    static void Main()
    {
        Console.WriteLine("Введите путь к файлу транзакций:");
        string? filePath = Console.ReadLine();

        if (!File.Exists(filePath) && !String.IsNullOrEmpty(filePath))
        {
            Console.WriteLine("Файл не найден. Создан новый файл.");
            File.Create(filePath).Close();
        }
        do
        {
            try
            {
                transactions = LoadTransactions.Loading(filePath);
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        } while (true);

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Просмотр всех транзакций");
            Console.WriteLine("2. Добавить транзакцию");
            Console.WriteLine("3. Удалить транзакцию");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите действие: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowTransactions.Showing(transactions);
                    break;
                case "2":
                    transactions.Add(AddTransaction.Adding());
                    break;
                case "3":
                    DeleteTransaction.Deleting(transactions);
                    break;
                case "4":
                    SaveTransactions.Saving(filePath, transactions);
                    return;
                default:
                    Console.WriteLine("Неверный ввод!");
                    break;
            }
        }
    }
}