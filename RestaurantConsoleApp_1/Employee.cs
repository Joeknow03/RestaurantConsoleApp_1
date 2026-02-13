using System;

namespace RestaurantConsoleApp_1;

public class Employee
{
    private object? _lastOrder;
    private static int _newRequestCount = 0;
    private bool _foodPrepared = false;

    public object NewRequest(int quantity, string menuItem)
    {
        _newRequestCount++;
        _foodPrepared = false;
        
        // every 3rd request employee forgets and brings smth else 
        bool shouldForget = _newRequestCount % 3 == 0;
        
        object order;
        if (menuItem == "Chicken")
        {
            order = shouldForget ? new EggOrder(quantity) : new ChickenOrder(quantity);
        }
        else // Egg
        {
            order = shouldForget ? new ChickenOrder(quantity) : new EggOrder(quantity);
        }
        
        _lastOrder = order;
        return order;
    }

    public object CopyRequest()
    {
        if (_lastOrder == null)
            throw new Exception("Employee is upset: No previous request to copy!");
        
        _foodPrepared = false;
        
        // copying order
        if (_lastOrder is ChickenOrder chicken)
        {
            return new ChickenOrder(chicken.GetQuantity());
        }
        else if (_lastOrder is EggOrder egg)
        {
            return new EggOrder(egg.GetQuantity());
        }
        
        throw new Exception("Unknown order type");
    }

    public string Inspect(object order)
    {
        if (order is ChickenOrder)
        {
            return "Chicken order - no inspection required";
        }
        else if (order is EggOrder eggOrder)
        {
            int? quality = eggOrder.GetQuality();
            if (quality.HasValue)
                return $"Egg Quality: {quality.Value}";
            else
                return "Egg Quality: Employee forgot to inspect!";
        }
        
        return "Unknown order type";
    }

    public string PrepareFood(object order)
    {
        if (_foodPrepared)
            throw new Exception("Food already prepared! Cannot prepare again!");
        
        _foodPrepared = true;
        
        if (order is ChickenOrder chickenOrder)
        {
            int quantity = chickenOrder.GetQuantity();
            for (int i = 0; i < quantity; i++)
            {
                chickenOrder.CutUp();
            }
            chickenOrder.Cook();
            
            return $"Prepared {quantity} chicken(s) successfully!";
        }
        else if (order is EggOrder eggOrder)
        {
            int quantity = eggOrder.GetQuantity();
            int rottenCount = 0;
            
            for (int i = 0; i < quantity; i++)
            {
                try
                {
                    eggOrder.Crack();
                }
                catch (Exception)
                {
                    rottenCount++;
                }
                finally
                {
                    eggOrder.DiscardShell();
                }
            }
            
            eggOrder.Cook();
            
            if (rottenCount > 0)
                return $"Prepared {quantity} egg(s). Found {rottenCount} rotten egg(s)!";
            else
                return $"Prepared {quantity} egg(s) successfully!";
        }
        
        return "Unknown order type";
    }
}