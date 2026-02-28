using System;

namespace RestaurantConsoleApp_1;

public class Cook
{
    public string Process(TableRequests requests)
    {
        string result = "";
        int rottenEggs = 0;

        try
        {
            // Обрабатываем курицу
            IMenuItem[] chickens = requests[new Chicken(1)]; // используем индексатор
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

                // Готовим всю курицу сразу
                if (chickens.Length > 0 && chickens[0] is Chicken firstChicken)
                {
                    firstChicken.Cook();
                }

                result += $"Prepared {chickens.Length} chicken(s)\n";
            }

            // Обрабатываем яйца
            IMenuItem[] eggs = requests[new Egg(1)]; // используем индексатор
            if (eggs.Length > 0)
            {
                foreach (var item in eggs)
                {
                    if (item is Egg egg)
                    {
                        egg.Obtain();
                        
                        try
                        {
                            egg.Crack();
                        }
                        catch (Exception)
                        {
                            rottenEggs++;
                        }
                        finally
                        {
                            // IDisposable - выбрасываем скорлупу
                            egg.Dispose();
                        }
                    }
                }

                // Готовим все яйца сразу
                if (eggs.Length > 0 && eggs[0] is Egg firstEgg)
                {
                    firstEgg.Cook();
                }

                result += $"Prepared {eggs.Length} egg(s)";
                if (rottenEggs > 0)
                    result += $" (found {rottenEggs} rotten)";
                result += "\n";
            }

            if (chickens.Length == 0 && eggs.Length == 0)
            {
                result = "No food to prepare!";
            }
        }
        catch (Exception ex)
        {
            result = $"Cook error: {ex.Message}";
        }

        return result;
    }
}