using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Logof_Client;

public partial class ResultWindow : Window
{
    public List<CheckBox> errortypecheckboxes = new();
    public List<(int, List<AddressCheck.ErrorTypes>)> ur_result;

    public ResultWindow(List<(int, List<AddressCheck.ErrorTypes>)> result)
    {
        InitializeComponent();
        ur_result = result;
        Load(result);
    }

    private void GenerateView(List<(int, List<AddressCheck.ErrorTypes>)> result)
    {
        var errors = new List<KasPersonError>();
        foreach (var single_result in result) errors.Add(new KasPersonError(single_result));
        LblResultCount.Content = $"{errors.Count}/{ur_result.Count} Ergebnisse";
        DgResult.ItemsSource = errors;
    }

    private void Load(List<(int, List<AddressCheck.ErrorTypes>)> result)
    {
        var knownErrors = new List<AddressCheck.ErrorTypes>();

        foreach (var single_result in result)
        foreach (var errtyp in single_result.Item2)
            if (!knownErrors.Contains(errtyp))
                knownErrors.Add(errtyp);

        foreach (var errtype in knownErrors)
        {
            var cb = new CheckBox();
            cb.IsChecked = true;
            cb.Content = errtype.ToString();
            cb.Click += (sender, e) => UpdateFilter();
            errortypecheckboxes.Add(cb);
            StpFilterOptions.Children.Add(cb);
        }

        GenerateView(result);
    }

    private void BtnUpdateFilter_OnClick(object? sender, RoutedEventArgs e)
    {
    }

    private void UpdateFilter()
    {
        var temp_result = new List<(int, List<AddressCheck.ErrorTypes>)>();
        var checked_types = new List<AddressCheck.ErrorTypes>();
        foreach (var cb in errortypecheckboxes)
            if (cb.IsChecked == true)
                checked_types.Add(
                    (AddressCheck.ErrorTypes)Enum.Parse(typeof(AddressCheck.ErrorTypes), cb.Content.ToString()));

        foreach (var sres in ur_result)
        foreach (var err in sres.Item2)
            if (checked_types.Contains(err) && !temp_result.Contains(sres))
                temp_result.Add(sres);

        var errors = new List<KasPersonError>();
        foreach (var single_result in temp_result) errors.Add(new KasPersonError(single_result));

        LblResultCount.Content = $"{errors.Count}/{ur_result.Count} Ergebnisse";
        DgResult.ItemsSource = errors;
    }
}