using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class MainViewModel : ObservableObject
{
    private readonly DatabaseService db = new DatabaseService();

    public ObservableCollection<CalculationRecord> History { get; } = new();

    [ObservableProperty]
    private string input;

    [ObservableProperty]
    private string errorMessage;

    public MainViewModel(){
        LoadHistory();
    }

    [RelayCommand]
    private void Calculate(){
        try{
            if (string.IsNullOrWhiteSpace(Input))
                throw new Exception("Порожній вираз");

            var result = new System.Data.DataTable().Compute(Input, null).ToString();
            db.SaveResult(Input, result);

            History.Add(new CalculationRecord { Expression = Input, Result = result });

            ErrorMessage = "";
        }
        catch (Exception ex){
            ErrorMessage = "Помилка обчислення або збереження даних.";
            Console.WriteLine(ex);
        }
    }

    private void LoadHistory(){
        db.GetHistory().ToList().ForEach(item => History.Add(item));
    }
}