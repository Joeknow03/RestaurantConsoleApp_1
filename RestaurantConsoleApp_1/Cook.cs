using System;
using System.Threading;

namespace RestaurantConsoleApp_1;

public class Cook
{
    public int Id { get; }

    public Cook() : this(1) { }

    public Cook(int id)
    {
        Id = id;
    }

    public string Process(TableRequests requests)
    {
        string result   = "";
        int rottenEggs  = 0;

        try
        {
            // Курица
            IMenuItem[] chickens = requests[new Chicken(1)];
            if (chickens.Length > 0)
            {
                foreach (var item in chickens)
                {
                    if (item is Chicken chicken)
                    {
                        chicken.Obtain();
                        chicken.CutUp();
                    }
                }

                Thread.Sleep(600 * chickens.Length); // симуляция готовки

                if (chickens[0] is Chicken firstChicken)
                    firstChicken.Cook();

                result += $"[Cook {Id}] Prepared {chickens.Length} chicken(s)\n";
            }

            // Яйца
            IMenuItem[] eggs = requests[new Egg(1)];
            if (eggs.Length > 0)
            {
                foreach (var item in eggs)
                {
                    if (item is Egg egg)
                    {
                        egg.Obtain();
                        try   { egg.Crack(); }
                        catch { rottenEggs++; }
                        finally { egg.Dispose(); }
                    }
                }

                Thread.Sleep(400 * eggs.Length); // симуляция готовки

                if (eggs[0] is Egg firstEgg)
                    firstEgg.Cook();

                result += $"[Cook {Id}] Prepared {eggs.Length} egg(s)";
                if (rottenEggs > 0) result += $" (found {rottenEggs} rotten)";
                result += "\n";
            }

            if (chickens.Length == 0 && eggs.Length == 0)
                result = $"[Cook {Id}] No food to prepare!";
        }
        catch (Exception ex)
        {
            result = $"[Cook {Id}] Error: {ex.Message}";
        }

        return result;
    }
}