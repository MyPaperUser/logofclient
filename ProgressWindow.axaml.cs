using Avalonia.Controls;

namespace Logof_Client;

public partial class ProgressWindow : Window
{
    public ProgressWindow()
    {
        InitializeComponent();
        PbProgress.Minimum = 0;
        PbProgress.Maximum = 100;
    }

    public void ChangePercentage(double percentage)
    {
        PbProgress.Value = percentage;
    }

    public void AddToLog(string message)
    {
        TbLog.Text = message;
        //ScvLog.ScrollToEnd();
    }
}