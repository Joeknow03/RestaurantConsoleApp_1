namespace RestaurantConsoleApp_1;

public class Chicken : CookedFood
{
    public Chicken(int quantity) : base(quantity)
    {
    }

    public override void Obtain()
    {
        _isObtained = true;
    }

    public void CutUp()
    {
        // Симуляция разделки
    }

    public override void Cook()
    {
        // Симуляция готовки курицы
    }

    public override void Serve()
    {
        _isServed = true;
    }

    public override string GetName()
    {
        return "Chicken";
    }
}