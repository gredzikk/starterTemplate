using CommunityToolkit.Mvvm.ComponentModel;

namespace starterTemplate.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    protected readonly ILogger _logger;

    protected BaseViewModel(ILogger logger)
    {
        _logger = logger;
    }
}