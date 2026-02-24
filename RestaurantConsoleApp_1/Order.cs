using System;

namespace RestaurantConsoleApp_1;

public class Order
{
    protected int _quantity;

    public Order(int quantity)
    {
        _quantity = quantity;
    }

    public int GetQuantity()
    {
        return _quantity;
    }

    public void SubtractQuantity(int amount)
    {
        _quantity -= amount;
    }

    public virtual void Cook()
    {
        // Базовая реализация готовки
    }
}