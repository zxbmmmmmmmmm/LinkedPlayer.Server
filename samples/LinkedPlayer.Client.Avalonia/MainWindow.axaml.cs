using Avalonia.Controls;
using LinkedPlayer.Client.Avalonia.ViewModels;

namespace LinkedPlayer.Client.Avalonia
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new SyncViewModel();
        }
    }
}