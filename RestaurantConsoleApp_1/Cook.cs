using System;

namespace RestaurantConsoleApp_1;

public class Cook
{
    private bool _foodPrepared = false;

    public ChickenOrder SubmitChickenOrder(int quantity)
    {
        _foodPrepared = false;
        return new ChickenOrder(quantity);
    }

    public EggOrder SubmitEggOrder(int quantity)
    {
        _foodPrepared = false;
        return new EggOrder(quantity);
    }

    public string PrepareChicken(ChickenOrder order)
    {
        if (_foodPrepared)
            throw new Exception("Food already prepared!");

        int quantity = order.GetQuantity();
        for (int i = 0; i < quantity; i++)
        {
            order.CutUp();
        }
        order.Cook();

        _foodPrepared = true;
        return $"Prepared {quantity} chicken(s)";
    }

    public string PrepareEggs(EggOrder order)
    {
        if (_foodPrepared)
            throw new Exception("Food already prepared!");

        int quantity = order.GetQuantity();
        int rottenCount = 0;

        for (int i = 0; i < quantity; i++)
        {
            try
            {
                order.Crack();
            }
            catch (Exception)
            {
                rottenCount++;
            }
            finally
            {
                order.DiscardShell();
            }
        }

        order.Cook();
        _foodPrepared = true;

        if (rottenCount > 0)
            return $"Prepared {quantity} egg(s). Found {rottenCount} rotten!";
        else
            return $"Prepared {quantity} egg(s)";
    }
}