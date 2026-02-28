namespace RestaurantConsoleApp_1;

public abstract class MenuItem : IMenuItem
{
    // Состояния элемента меню
    protected bool _isObtained = false;
    protected bool _isServed = false;

    // Abstract методы - должны быть реализованы в наследниках
    public abstract void Obtain();
    public abstract void Serve();
    public abstract string GetName();
}