namespace RestaurantConsoleApp_1;

public abstract class CookedFood : MenuItem
{
    protected int _quantity;

    public CookedFood(int quantity)
    {
        _quantity = quantity;
    }

    // Property вместо GetQuantity()
    public int Quantity
    {
        get { return _quantity; }
        protected set { _quantity = value; }
    }

    public void SubtractQuantity(int amount)
    {
        _quantity -= amount;
    }

    public abstract void Cook();
}