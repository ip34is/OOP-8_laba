using System;
using System.Collections.Generic;
using System.Globalization;

public struct Record
{
    public string Account { get; set; }
    public string Category { get; set; }
    public double Money { get; set; } 
    public ulong Timestamp { get; set; }

    public Record(string account, string category, double money, ulong timestamp)
    {
        Account = account;
        Category = category;
        Money = money;
        Timestamp = timestamp;
    }
}

public class Category
{
    public string Name { get; set; }

    public Category(string name)
    {
        Name = name;
    }
}

public class Account
{
    public string Name { get; set; }
    public double Balance { get; set; }

    public Account(string name, double balance = 0)
    {
        Name = name;
        Balance = balance;
    }
}

public class FinanceManager
{
    private List<Category> categories = new List<Category>();
    private List<Account> accounts = new List<Account>();
    private List<Record> records = new List<Record>();
    // Вище приклади агрегації

    public void AddCategory(string name)
    {
        categories.Add(new Category(name));
    }

    public void DeleteCategory(string name)
    {
        categories.RemoveAll(c => c.Name == name);
    }

    public void UpdateCategory(string oldName, string newName)
    {
        var category = categories.Find(c => c.Name == oldName);
        if (category != null)
        {
            category.Name = newName;
        }
    }

    public List<Category> GetCategories()
    {
        return new List<Category>(categories);
    }

    public void AddAccount(string name, double balance = 0)
    {
        accounts.Add(new Account(name, balance));
    }

    public void DeleteAccount(string name)
    {
        accounts.RemoveAll(a => a.Name == name);
    }

    public void UpdateAccount(string oldName, string newName, double balance)
    {
        var account = accounts.Find(a => a.Name == oldName);
        if (account != null)
        {
            account.Name = newName;
            account.Balance = balance;
        }
    }

    public List<Account> GetAccounts()
    {
        return new List<Account>(accounts);
    }

    public void AddExpense(string accountName, string categoryName, double amount, ulong timestamp)
    {
        var account = accounts.Find(a => a.Name == accountName);
        if (account != null)
        {
            account.Balance -= amount;
            records.Add(new Record(accountName, categoryName, -amount, timestamp));
        }
    }

    public void AddIncome(string accountName, string categoryName, double amount, ulong timestamp)
    {
        var account = accounts.Find(a => a.Name == accountName);
        if (account != null)
        {
            account.Balance += amount;
            records.Add(new Record(accountName, categoryName, amount, timestamp));
        }
    }

    public List<Record> GetRecords()
    {
        return new List<Record>(records);
    }

    public List<Record> GetRecordsByPeriod(ulong startTimestamp, ulong endTimestamp)
    {
        return records.FindAll(r => r.Timestamp >= startTimestamp && r.Timestamp <= endTimestamp);
    }

    public double GetStatisticsByPeriod(ulong startTimestamp, ulong endTimestamp)
    {
        double total = 0;
        foreach (var record in records)
        {
            if (record.Timestamp >= startTimestamp && record.Timestamp <= endTimestamp)
            {
                total += record.Money;
            }
        }
        return total;
    }
}

public class Program
{
    private static FinanceManager manager = new FinanceManager();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Виберіть дію:");
            Console.WriteLine("1. Додати категорію");
            Console.WriteLine("2. Видалити категорію");
            Console.WriteLine("3. Оновити категорію");
            Console.WriteLine("4. Переглянути категорії");
            Console.WriteLine("5. Додати рахунок");
            Console.WriteLine("6. Видалити рахунок");
            Console.WriteLine("7. Оновити рахунок");
            Console.WriteLine("8. Переглянути рахунки");
            Console.WriteLine("9. Додати витрати");
            Console.WriteLine("10. Додати доходи");
            Console.WriteLine("11. Переглянути записи");
            Console.WriteLine("12. Переглянути записи за період");
            Console.WriteLine("13. Переглянути статистику за період");
            Console.WriteLine("0. Вийти");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddCategory();
                    break;
                case "2":
                    DeleteCategory();
                    break;
                case "3":
                    UpdateCategory();
                    break;
                case "4":
                    ViewCategories();
                    break;
                case "5":
                    AddAccount();
                    break;
                case "6":
                    DeleteAccount();
                    break;
                case "7":
                    UpdateAccount();
                    break;
                case "8":
                    ViewAccounts();
                    break;
                case "9":
                    AddExpense();
                    break;
                case "10":
                    AddIncome();
                    break;
                case "11":
                    ViewRecords();
                    break;
                case "12":
                    ViewRecordsByPeriod();
                    break;
                case "13":
                    ViewStatisticsByPeriod();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    //Нижче приклади залежності Program від Finance Manager
    static void AddCategory()
    {
        Console.Write("Введіть назву категорії: ");
        string name = Console.ReadLine();
        manager.AddCategory(name);
        Console.WriteLine("Категорію додано.\n");
    }

    static void DeleteCategory()
    {
        Console.Write("Введіть назву категорії для видалення: ");
        string name = Console.ReadLine();
        manager.DeleteCategory(name);
        Console.WriteLine("Категорію видалено.\n");
    }

    static void UpdateCategory()
    {
        Console.Write("Введіть стару назву категорії: ");
        string oldName = Console.ReadLine();
        Console.Write("Введіть нову назву категорії: ");
        string newName = Console.ReadLine();
        manager.UpdateCategory(oldName, newName);
        Console.WriteLine("Категорію оновлено.\n");
    }

    static void ViewCategories()
    {
        List<Category> categories = manager.GetCategories();
        Console.WriteLine("Категорії:");
        foreach (var category in categories)
        {
            Console.WriteLine(category.Name);
        }
    }

    static void AddAccount()
    {
        Console.Write("Введіть назву рахунку: ");
        string name = Console.ReadLine();
        Console.Write("Введіть початковий баланс: ");
        double balance = Convert.ToDouble(Console.ReadLine(), CultureInfo.InvariantCulture);
        manager.AddAccount(name, balance);
        Console.WriteLine("Рахунок додано.\n");
    }

    static void DeleteAccount()
    {
        Console.Write("Введіть назву рахунку для видалення: ");
        string name = Console.ReadLine();
        manager.DeleteAccount(name);
        Console.WriteLine("Рахунок видалено.\n");
    }

    static void UpdateAccount()
    {
        Console.Write("Введіть стару назву рахунку: ");
        string oldName = Console.ReadLine();
        Console.Write("Введіть нову назву рахунку: ");
        string newName = Console.ReadLine();
        Console.Write("Введіть новий баланс: ");
        double balance = Convert.ToDouble(Console.ReadLine(), CultureInfo.InvariantCulture);
        manager.UpdateAccount(oldName, newName, balance);
        Console.WriteLine("Рахунок оновлено.\n");
    }

    static void ViewAccounts()
    {
        List<Account> accounts = manager.GetAccounts();
        Console.WriteLine("Рахунки:");
        foreach (var account in accounts)
        {
            Console.WriteLine($"{account.Name} - Баланс: {account.Balance}");
        }
    }

    static void AddExpense()
    {
        Console.Write("Введіть назву рахунку: ");
        string accountName = Console.ReadLine();
        Console.Write("Введіть назву категорії: ");
        string categoryName = Console.ReadLine();
        Console.Write("Введіть суму витрат: ");
        double amount = Convert.ToDouble(Console.ReadLine(), CultureInfo.InvariantCulture);
        ulong timestamp = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds();
        manager.AddExpense(accountName, categoryName, amount, timestamp);
        Console.WriteLine("Витрати додано.\n");
    }

    static void AddIncome()
    {
        Console.Write("Введіть назву рахунку: ");
        string accountName = Console.ReadLine();
        Console.Write("Введіть назву категорії: ");
        string categoryName = Console.ReadLine();
        Console.Write("Введіть суму доходів: ");
        double amount = Convert.ToDouble(Console.ReadLine(), CultureInfo.InvariantCulture);
        ulong timestamp = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds();
        manager.AddIncome(accountName, categoryName, amount, timestamp);
        Console.WriteLine("Доходи додано.\n");
    }

    static void ViewRecords()
    {
        List<Record> records = manager.GetRecords();
        Console.WriteLine("Записи:");
        foreach (var record in records)
        {
            Console.WriteLine($"Рахунок: {record.Account}, Категорія: {record.Category}, Сума: {record.Money}, Час: {record.Timestamp}");
        }
    }

    static void ViewRecordsByPeriod()
    {
        Console.Write("Введіть початкову дату (timestamp): ");
        ulong startTimestamp = Convert.ToUInt64(Console.ReadLine());
        Console.Write("Введіть кінцеву дату (timestamp): ");
        ulong endTimestamp = Convert.ToUInt64(Console.ReadLine());
        List<Record> records = manager.GetRecordsByPeriod(startTimestamp, endTimestamp);
        Console.WriteLine("Записи за період:");
        foreach (var record in records)
        {
            Console.WriteLine($"Рахунок: {record.Account}, Категорія: {record.Category}, Сума: {record.Money}, Час: {record.Timestamp}");
        }
    }

    static void ViewStatisticsByPeriod()
    {
        Console.Write("Введіть початкову дату (timestamp): ");
        ulong startTimestamp = Convert.ToUInt64(Console.ReadLine());
        Console.Write("Введіть кінцеву дату (timestamp): ");
        ulong endTimestamp = Convert.ToUInt64(Console.ReadLine());
        double total = manager.GetStatisticsByPeriod(startTimestamp, endTimestamp);
        Console.WriteLine($"Загальна сума за період: {total}");
    }
}