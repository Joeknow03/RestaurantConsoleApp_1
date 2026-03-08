using System;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantConsoleApp_1;

/// Один официант — используем lock (не SemaphoreSlim),
/// потому что ресурс один, а не пул.
///
/// Заблокированные операции:
///   1. ReceiveRequest  — приём заказа
///   2. SendToCookAsync — снятие снапшота заказов
///   3. ServeFood       — подача еды (continuation task)

public class Server
{
    private readonly object _lock = new();

    private TableRequests _tableRequests  = new();
    private int           _currentCustomer = 0;
    private const int     MAX_CUSTOMERS    = 8;

    // ── 1. Принять заказ (LOCKED) ─────────────────────────────────────────────
    public string ReceiveRequest(int chickenQty, int eggQty, string drinkType, string customerName)
    {
        lock (_lock)
        {
            if (_currentCustomer >= MAX_CUSTOMERS)
                return "Table is full! Maximum 8 customers.";

            try
            {
                for (int i = 0; i < chickenQty; i++)
                    _tableRequests.Add(customerName, new Chicken(1));

                for (int i = 0; i < eggQty; i++)
                    _tableRequests.Add(customerName, new Egg(1));

                IMenuItem drink = drinkType switch
                {
                    "Tea"      => new Tea(),
                    "CocaCola" => new CocaCola(),
                    "Pepsi"    => new Pepsi(),
                    _          => new NoDrink()
                };

                if (drink is not NoDrink)
                    _tableRequests.Add(customerName, drink);

                string result = $"{customerName}'s order received: " +
                                $"{chickenQty} chicken, {eggQty} egg, {drink.GetName()}";
                _currentCustomer++;
                return result;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }

    // Старый вариант без имени — для совместимости
    public string ReceiveRequest(int chickenQty, int eggQty, string drinkType) =>
        ReceiveRequest(chickenQty, eggQty, drinkType, $"Customer {_currentCustomer}");

    // ── 2. Отправить в кухню: Task 1 (готовка) → Task 2 continuation (подача) ─
    public async Task<string> SendToCookAsync(CookPool pool)
    {
        if (_currentCustomer == 0)
            return "No orders to send!";

        // Снапшот внутри lock, потом отпускаем до await
        TableRequests requests;
        lock (_lock)
        {
            requests         = _tableRequests;
            _tableRequests   = new TableRequests();
            _currentCustomer = 0;
        }

        string cookResult;

        // Task 1: повар готовит (SemaphoreSlim ограничивает параллельность)
        using (var scope = await pool.AcquireAsync())
        {
            cookResult = await Task.Run(() => scope.Cook.Process(requests));
        } // повар возвращается в пул здесь

        // Task 2 (continuation): подача еды — LOCKED внутри ServeFood
        string serveResult = await Task.Run(() => ServeFood(requests));

        return cookResult + "\n" + serveResult;
    }

    // ── 3. Подать еду с LINQ (LOCKED) ─────────────────────────────────────────
    public string ServeFood(TableRequests requests)
    {
        lock (_lock)
        {
            if (requests.GetCustomerCount() == 0)
                return "No customers to serve!";

            Thread.Sleep(300); // симуляция подачи

            string result = "";

            // LINQ: алфавитный порядок + сводка по каждому клиенту
            foreach (var summary in requests.Summaries())
                result += summary + "\n";

            result += "\nPlease enjoy your food!";
            return result;
        }
    }

    // Старый ServeFood — для кнопки "Serve" без предварительной готовки
    public string ServeFood()
    {
        lock (_lock)
        {
            if (_currentCustomer == 0) return "No customers to serve!";

            var requests     = _tableRequests;
            _tableRequests   = new TableRequests();
            _currentCustomer = 0;

            // Вызываем без lock — уже внутри lock
            string result = "";
            foreach (var summary in requests.Summaries())
                result += summary + "\n";
            result += "\nPlease enjoy your food!";
            return result;
        }
    }
}