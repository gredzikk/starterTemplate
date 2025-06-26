using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace starterTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger _logger;
        public string Version { get; }

        public MainWindow(ILogger logger)
        {
            InitializeComponent();
            _logger = logger;
            _logger.LogInfo("MainWindow initialized.");
            Version = GetVersionString();
            DataContext = this;
        }

        public static string GetVersionString()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}