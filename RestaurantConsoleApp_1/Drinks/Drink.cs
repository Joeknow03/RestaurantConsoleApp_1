namespace RestaurantConsoleApp_1;

public abstract class Drink : MenuItem
{
    public override void Obtain()
    {
        _isObtained = true;
    }

    public override void Serve()
    {
        _isServed = true;
    }
}