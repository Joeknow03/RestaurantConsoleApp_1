using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace RestaurantConsoleApp_1;

public partial class MainWindow : Window
{
    private Employee _employee;
    private object? _currentOrder;

    public MainWindow()
    {
        InitializeComponent();
        _employee = new Employee();
    }

    private void SubmitNewRequest_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                ResultsTextBox.Text = "Error: Please enter a valid quantity!";
                return;
            }

            string menuItem = ChickenRadio.IsChecked == true ? "Chicken" : "Egg";
            
            _currentOrder = _employee.NewRequest(quantity, menuItem);
            
            string inspectionResult = _employee.Inspect(_currentOrder);
            EggQualityText.Text = inspectionResult;
            
            string orderType = _currentOrder is ChickenOrder ? "Chicken" : "Egg";
            ResultsTextBox.Text = $"Employee obtained: {quantity} {orderType}\n{inspectionResult}";
        }
        catch (Exception ex)
        {
            ResultsTextBox.Text = $"Error: {ex.Message}";
        }
    }

    private void CopyPreviousRequest_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _currentOrder = _employee.CopyRequest();
            
            string inspectionResult = _employee.Inspect(_currentOrder);
            EggQualityText.Text = inspectionResult;
            
            string orderType = _currentOrder is ChickenOrder ? "Chicken" : "Egg";
            int quantity = _currentOrder is ChickenOrder chicken 
                ? chicken.GetQuantity() 
                : ((EggOrder)_currentOrder).GetQuantity();
            
            ResultsTextBox.Text = $"Employee copied previous order: {quantity} {orderType}\n{inspectionResult}";
        }
        catch (Exception ex)
        {
            ResultsTextBox.Text = $"Error: {ex.Message}";
        }
    }

    private void PrepareFood_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_currentOrder == null)
            {
                ResultsTextBox.Text = "Error: No order to prepare! Submit a request first.";
                return;
            }

            string result = _employee.PrepareFood(_currentOrder);
            ResultsTextBox.Text = result;
        }
        catch (Exception ex)
        {
            ResultsTextBox.Text = $"Error: {ex.Message}";
        }
    }
}