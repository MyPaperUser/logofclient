using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Logof_Client;

public partial class ResultWindow : Window
{
    public List<CheckBox> errortypecheckboxes = new();
    public List<(int, List<AddressCheck.ErrorTypes>, List<AddressCheck.WarningTypes>)> ur_result;
    public List<CheckBox> warningtypecheckboxes = new();

    public ResultWindow(List<(int, List<AddressCheck.ErrorTypes>, List<AddressCheck.WarningTypes>)> result)
    {
        InitializeComponent();
        ur_result = result;
        Load(result);
    }

    private void GenerateView(List<(int, List<AddressCheck.ErrorTypes>, List<AddressCheck.WarningTypes>)> result)
    {
        var errors = new List<KasPersonError>();
        foreach (var single_result in result) errors.Add(new KasPersonError(single_result));
        LblResultCount.Content = $"{errors.Count}/{ur_result.Count} Ergebnisse";
        DgResult.ItemsSource = errors;
    }

    private void Load(List<(int, List<AddressCheck.ErrorTypes>, List<AddressCheck.WarningTypes>)> result)
    {
        var knownErrors = new List<AddressCheck.ErrorTypes>();
        var knownWarnings = new List<AddressCheck.WarningTypes>();

        foreach (var single_result in result)
        {
            foreach (var errtyp in single_result.Item2)
                if (!knownErrors.Contains(errtyp))
                    knownErrors.Add(errtyp);

            foreach (var wartyp in single_result.Item3)
                if (!knownWarnings.Contains(wartyp))
                    knownWarnings.Add(wartyp);
        }


        foreach (var errtype in knownErrors)
        {
            var cb = new CheckBox();
            cb.IsChecked = true;
            cb.Content = errtype.ToString();
            cb.Click += (sender, e) => UpdateFilter();
            errortypecheckboxes.Add(cb);
            StpFilterOptions.Children.Add(cb);
        }

        foreach (var wartype in knownWarnings)
        {
            var cb = new CheckBox();
            cb.IsChecked = true;
            cb.Content = wartype.ToString();
            cb.Click += (sender, e) => UpdateFilter();
            warningtypecheckboxes.Add(cb);
            StpFilterOptions.Children.Add(cb);
        }

        GenerateView(result);
    }

    private void BtnUpdateFilter_OnClick(object? sender, RoutedEventArgs e)
    {
    }

    private void UpdateFilter()
    {
        var temp_result = new List<(int, List<AddressCheck.ErrorTypes>, List<AddressCheck.WarningTypes>)>();
        var checked_types = new List<AddressCheck.ErrorTypes>();
        var checked_types_war = new List<AddressCheck.WarningTypes>();
        foreach (var cb in errortypecheckboxes)
            if (cb.IsChecked == true)
                checked_types.Add(
                    (AddressCheck.ErrorTypes)Enum.Parse(typeof(AddressCheck.ErrorTypes), cb.Content.ToString()));

        foreach (var cb in warningtypecheckboxes)
            if (cb.IsChecked == true)
                checked_types_war.Add(
                    (AddressCheck.WarningTypes)Enum.Parse(typeof(AddressCheck.WarningTypes), cb.Content.ToString()));

        foreach (var sres in ur_result)
        {
            foreach (var err in sres.Item2)
                if (checked_types.Contains(err) && !temp_result.Contains(sres))
                    temp_result.Add(sres);

            foreach (var war in sres.Item3)
                if (checked_types_war.Contains(war) && !temp_result.Contains(sres))
                    temp_result.Add(sres);
        }


        var errors = new List<KasPersonError>();
        foreach (var single_result in temp_result) errors.Add(new KasPersonError(single_result));

        LblResultCount.Content = $"{errors.Count}/{ur_result.Count} Ergebnisse";
        DgResult.ItemsSource = errors;
    }
}