using System;

namespace RestaurantConsoleApp_1;

public class Server
{
    private TableRequests _tableRequests;
    private int _currentCustomer = 0;
    private const int MAX_CUSTOMERS = 8;

    public Server()
    {
        _tableRequests = new TableRequests();
    }

    // Принять заказ от клиента
    public string ReceiveRequest(int chickenQty, int eggQty, string drinkType)
    {
        if (_currentCustomer >= MAX_CUSTOMERS)
            return "Table is full! Maximum 8 customers.";

        try
        {
            // Добавляем курицу
            for (int i = 0; i < chickenQty; i++)
            {
                _tableRequests.Add(_currentCustomer, new Chicken(1));
            }

            // Добавляем яйца
            for (int i = 0; i < eggQty; i++)
            {
                _tableRequests.Add(_currentCustomer, new Egg(1));
            }

            // Добавляем напиток
            IMenuItem drink = drinkType switch
            {
                "Tea" => new Tea(),
                "CocaCola" => new CocaCola(),
                "Pepsi" => new Pepsi(),
                _ => new NoDrink()
            };

            if (drink is not NoDrink)
            {
                _tableRequests.Add(_currentCustomer, drink);
            }

            string result = $"Customer {_currentCustomer} order received: {chickenQty} chicken, {eggQty} egg, {drink.GetName()}";
            _currentCustomer++;
            return result;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    // Отправить заказы повару
    public string SendToCook(Cook cook)
    {
        if (_currentCustomer == 0)
            return "No orders to send!";

        try
        {
            string result = cook.Process(_tableRequests);
            return result;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    // Подать еду клиентам
    public string ServeFood()
    {
        if (_currentCustomer == 0)
            return "No customers to serve!";

        string result = "";

        for (int i = 0; i < _currentCustomer; i++)
        {
            IMenuItem[] customerItems = _tableRequests[i]; // используем индексатор

            int chickenCount = 0;
            int eggCount = 0;
            string drink = "no drink";

            foreach (var item in customerItems)
            {
                item.Serve(); // Подаем еду

                if (item is Chicken) chickenCount++;
                else if (item is Egg) eggCount++;
                else if (item is Drink && item is not NoDrink) drink = item.GetName();
            }

            result += $"Customer {i} is served {chickenCount} chicken, {eggCount} egg, {drink}\n";
        }

        result += "\nPlease enjoy your food!";

        // Очищаем стол
        _tableRequests.Clear();
        _currentCustomer = 0;

        return result;
    }
}