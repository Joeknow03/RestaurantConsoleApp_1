namespace RestaurantConsoleApp_1;

public class ChickenOrder : Order
{
    public ChickenOrder(int quantity) : base(quantity)
    {
    }

    public void CutUp()
    {
        // Симуляция разделки курицы
    }

    public override void Cook()  // Переопределяем метод
    {
        // Симуляция приготовления курицы
    }
    
}