using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace Logof_Client;

public partial class ResultWindow : Window
{
    public List<CheckBox> errortypecheckboxes = new();
    public KasAddressList ur_addresses = new();
    public List<(int, List<AddressCheck.ErrorTypes>, List<AddressCheck.WarningTypes>)> ur_result;
    public List<CheckBox> warningtypecheckboxes = new();

    public ResultWindow(List<(int, List<AddressCheck.ErrorTypes>, List<AddressCheck.WarningTypes>)> result,
        KasAddressList ur_addresses)
    {
        InitializeComponent();
        ur_result = result;
        this.ur_addresses = ur_addresses;
        Load(result);
        //ViewSingle(200552426);
    }

    private void GenerateView(List<(int, List<AddressCheck.ErrorTypes>, List<AddressCheck.WarningTypes>)> result)
    {
        var errors = new List<KasPersonError>();
        foreach (var single_result in result) errors.Add(new KasPersonError(single_result));
        LblResultCount.Content = $"{errors.Count}/{ur_result.Count} Ergebnisse";
        DgResult.ItemsSource = errors;
    }

    private void ViewSingle(int refsid)
    {
        foreach (var result in ur_addresses.KasPersons)
            if (result.refsid == refsid)
            {
                var wind = new Window();
                var stp = new StackPanel();
                stp.Orientation = Orientation.Horizontal;
                stp.Margin = new Thickness(10);
                var tb = new TextBlock();
                var tb2 = new TextBlock();
                tb.Text =
                    "refsid:\nanrede:\ntitel:\nvorname:\nadel:\nname:\nnamezus:\nanredzus:\nstrasse:\nstrasse2:\nplz:\nort:\nland:\npplz:\npostfach:\nname1:\nname2:\nname3:\nname4:\nname5:\nfunktion:\nfunktion2:\nabteilung:\nfunktionad:";
                tb2.Text = result.refsid + "\n" + result.anrede + "\n" + result.titel + "\n" + result.vorname + "\n" +
                           result.adel + "\n" + result.name + "\n" + result.namezus + "\n" + result.anredzus + "\n" +
                           result.strasse + "\n" + result.strasse2 + "\n" + result.plz + "\n" + result.ort + "\n" +
                           result.land + "\n" + result.pplz + "\n" + result.postfach + "\n" + result.name1 + "\n" +
                           result.name2 + "\n" + result.name3 + "\n" + result.name4 + "\n" + result.name5 + "\n" +
                           result.funktion + "\n" + result.funktion2 + "\n" + result.abteilung + "\n" +
                           result.funktionad;
                stp.Children.Add(tb);
                stp.Children.Add(tb2);
                wind.Content = stp;
                wind.ShowInTaskbar = false;
                wind.SizeToContent = SizeToContent.WidthAndHeight;
                wind.Show();
                return;
            }
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

    private void BtnShwoSelected_OnClick(object? sender, RoutedEventArgs e)
    {
        foreach (var selected in DgResult.SelectedItems)
            try
            {
                var _asKas = (KasPersonError)selected;
                ViewSingle(_asKas.refsid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
    }
}