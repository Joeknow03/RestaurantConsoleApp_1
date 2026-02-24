using System;
using System.Collections.Generic;

namespace RestaurantConsoleApp_1;

public class Server
{
    private MenuItem[][] _tableRequests;  // Jagged array
    private int _currentCustomer = 0;
    private const int MAX_CUSTOMERS = 8;

    public Server()
    {
        _tableRequests = new MenuItem[MAX_CUSTOMERS][];
    }

    // Принять заказ от одного клиента
    public string ReceiveRequest(int chickenQty, int eggQty, MenuItem drink)
    {
        if (_currentCustomer >= MAX_CUSTOMERS)
            return "Table is full! Maximum 8 customers.";

        // Создаем массив для этого клиента
        List<MenuItem> items = new List<MenuItem>();

        // Добавляем курицу
        for (int i = 0; i < chickenQty; i++)
            items.Add(MenuItem.Chicken);

        // Добавляем яйца
        for (int i = 0; i < eggQty; i++)
            items.Add(MenuItem.Egg);

        // Добавляем напиток (если есть)
        if (drink != MenuItem.NoDrink)
            items.Add(drink);

        _tableRequests[_currentCustomer] = items.ToArray();
        _currentCustomer++;

        return $"Customer {_currentCustomer - 1} order received: {chickenQty} chicken, {eggQty} egg, {drink}";
    }

    // Отправить все заказы повару
    public string SendAllRequestsToCook(Cook cook)
    {
        if (_currentCustomer == 0)
            return "No orders to send!";

        // Подсчитываем общее количество
        int totalChicken = 0;
        int totalEgg = 0;

        for (int i = 0; i < _currentCustomer; i++)
        {
            if (_tableRequests[i] != null)
            {
                foreach (var item in _tableRequests[i])
                {
                    if (item == MenuItem.Chicken) totalChicken++;
                    if (item == MenuItem.Egg) totalEgg++;
                }
            }
        }

        string result = "";

        try
        {
            // Готовим курицу
            if (totalChicken > 0)
            {
                ChickenOrder chickenOrder = cook.SubmitChickenOrder(totalChicken);
                result += cook.PrepareChicken(chickenOrder) + "\n";
            }

            // Готовим яйца
            if (totalEgg > 0)
            {
                EggOrder eggOrder = cook.SubmitEggOrder(totalEgg);
                result += cook.PrepareEggs(eggOrder) + "\n";
            }

            result += $"\nTotal: {totalChicken} chicken, {totalEgg} egg prepared";
        }
        catch (Exception ex)
        {
            result = $"Error: {ex.Message}";
        }

        return result;
    }

    // Подать еду клиентам
    public string ServeFood()
    {
        if (_currentCustomer == 0)
            return "No customers to serve!";

        string result = "";

        for (int i = 0; i < _currentCustomer; i++)
        {
            int chickenCount = 0;
            int eggCount = 0;
            string drink = "no drink";

            if (_tableRequests[i] != null)
            {
                foreach (var item in _tableRequests[i])
                {
                    if (item == MenuItem.Chicken) chickenCount++;
                    else if (item == MenuItem.Egg) eggCount++;
                    else if (item != MenuItem.NoDrink) drink = item.ToString();
                }
            }

            result += $"Customer {i} is served {chickenCount} chicken, {eggCount} egg, {drink}\n";
        }

        result += "\nPlease enjoy your food!";

        // Очищаем стол для новых клиентов
        _tableRequests = new MenuItem[MAX_CUSTOMERS][];
        _currentCustomer = 0;

        return result;
    }
}