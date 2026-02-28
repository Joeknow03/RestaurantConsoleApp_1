using System;
using System.Collections.Generic;

namespace RestaurantConsoleApp_1;

public class TableRequests
{
    private List<(int customer, IMenuItem item)> _requests;
    private const int MAX_CUSTOMERS = 8;

    public TableRequests()
    {
        _requests = new List<(int, IMenuItem)>();
    }

    // Добавить заказ
    public void Add(int customer, IMenuItem item)
    {
        if (customer < 0 || customer >= MAX_CUSTOMERS)
            throw new Exception($"Invalid customer number. Must be 0-{MAX_CUSTOMERS - 1}");

        _requests.Add((customer, item));
    }

    // Индексатор [IMenuItem] - возвращает все элементы данного типа
    public IMenuItem[] this[IMenuItem itemType]
    {
        get
        {
            List<IMenuItem> result = new List<IMenuItem>();
            string typeName = itemType.GetType().Name;

            foreach (var request in _requests)
            {
                if (request.item.GetType().Name == typeName)
                {
                    result.Add(request.item);
                }
            }

            return result.ToArray();
        }
    }

    // Индексатор [int] - возвращает все заказы клиента
    public IMenuItem[] this[int customer]
    {
        get
        {
            List<IMenuItem> result = new List<IMenuItem>();

            foreach (var request in _requests)
            {
                if (request.customer == customer)
                {
                    result.Add(request.item);
                }
            }

            return result.ToArray();
        }
    }

    // Получить общее количество Chicken
    public int GetTotalChickenCount()
    {
        int count = 0;
        foreach (var request in _requests)
        {
            if (request.item is Chicken)
                count++;
        }
        return count;
    }

    // Получить общее количество Egg
    public int GetTotalEggCount()
    {
        int count = 0;
        foreach (var request in _requests)
        {
            if (request.item is Egg)
                count++;
        }
        return count;
    }

    // Получить количество клиентов
    public int GetCustomerCount()
    {
        HashSet<int> customers = new HashSet<int>();
        foreach (var request in _requests)
        {
            customers.Add(request.customer);
        }
        return customers.Count;
    }

    // Очистить все заказы
    public void Clear()
    {
        _requests.Clear();
    }
}