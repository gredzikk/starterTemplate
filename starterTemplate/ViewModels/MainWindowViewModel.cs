using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Reflection;

namespace starterTemplate.ViewModels;

public partial class MainWindowViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _version = string.Empty;

    [ObservableProperty]
    private string _title = "Main Window";

    public MainWindowViewModel(ILogger logger) : base(logger)
    {
        Version = GetVersionString();
        Title = $"Main {Version}";
        _logger.LogInfo("MainWindowViewModel initialized.");
    }

    [RelayCommand]
    private void ShowAbout()
    {
        _logger.LogInfo("About command executed.");
    }

    private static string GetVersionString()
    {
        Version version = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(1, 0, 0, 0);
        return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}