using System.Collections.Generic;
using Avalonia.Controls;

namespace Logof_Client;

public partial class ResultWindow : Window
{
    public ResultWindow(List<(int, List<AddressCheck.ErrorTypes>)> result)
    {
        InitializeComponent();
        GenerateView(result);
    }

    private void GenerateView(List<(int, List<AddressCheck.ErrorTypes>)> result)
    {
        var errors = new List<KasPersonError>();
        foreach (var single_result in result) errors.Add(new KasPersonError(single_result));
        DgResult.ItemsSource = errors;
    }
}