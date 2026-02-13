namespace RestaurantConsoleApp_1;

public class ChickenOrder
{
    private readonly int _quantity;

    public ChickenOrder(int quantity)
    {
        _quantity = quantity;
    }

    public int GetQuantity()
    {
        return _quantity;
    }

    public void CutUp()
    {
        // chicken cut simulation
    }

    public void Cook()
    {
        // cooking process
    }
}