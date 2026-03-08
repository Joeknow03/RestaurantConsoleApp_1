using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantConsoleApp_1;

public class TableRequests : IEnumerable<(string customerName, IMenuItem item)>
{
    private List<(string customerName, IMenuItem item)> _requests = new();
    private const int MAX_CUSTOMERS = 8;

    // Добавить по имени
    public void Add(string customerName, IMenuItem item)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new Exception("Customer name cannot be empty.");
        _requests.Add((customerName, item));
    }

    // Старый Add через int — оставляем чтобы Cook.cs не сломался
    public void Add(int customer, IMenuItem item) =>
        Add($"Customer {customer}", item);

    // Индексатор по типу (используется в Cook.Process)
    public IMenuItem[] this[IMenuItem itemType]
    {
        get
        {
            string typeName = itemType.GetType().Name;
            return _requests
                .Where(r => r.item.GetType().Name == typeName)
                .Select(r => r.item)
                .ToArray();
        }
    }

    // Индексатор по имени клиента
    public IMenuItem[] this[string customerName] =>
        _requests
            .Where(r => r.customerName == customerName)
            .Select(r => r.item)
            .ToArray();

    // Старый индексатор по int
    public IMenuItem[] this[int customer] => this[$"Customer {customer}"];

    // LINQ: имена клиентов в алфавитном порядке
    public IEnumerable<string> CustomerNamesAlphabetical() =>
        _requests.Select(r => r.customerName).Distinct().OrderBy(n => n);

    // LINQ: сводка по каждому клиенту в алфавитном порядке
    // Пример: "Brent ordered 1 drink, 3 egg and 2 chicken"
    public IEnumerable<string> Summaries() =>
        CustomerNamesAlphabetical().Select(name =>
        {
            var items    = _requests.Where(r => r.customerName == name).Select(r => r.item);
            int chickens = items.Count(i => i is Chicken);
            int eggs     = items.Count(i => i is Egg);
            int drinks   = items.Count(i => i is Drink && i is not NoDrink);

            var parts = new List<string>();
            if (drinks   > 0) parts.Add($"{drinks} drink");
            if (eggs     > 0) parts.Add($"{eggs} egg");
            if (chickens > 0) parts.Add($"{chickens} chicken");

            string ordered = parts.Count > 0 ? string.Join(", ", parts) : "nothing";
            return $"{name} ordered {ordered}";
        });

    public int GetTotalChickenCount() => _requests.Count(r => r.item is Chicken);
    public int GetTotalEggCount()     => _requests.Count(r => r.item is Egg);
    public int GetCustomerCount()     => _requests.Select(r => r.customerName).Distinct().Count();
    public void Clear()               => _requests.Clear();

    public IEnumerator<(string customerName, IMenuItem item)> GetEnumerator() =>
        _requests.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}