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

namespace Proxy_Pattern
{
    public partial class MainWindow : Window
    {
        private Proxy _proxy;

        public MainWindow()
        {
            InitializeComponent();
            _proxy = new Proxy(TimeSpan.FromSeconds(30)); // Кэш на 30 секунд
        }

        private void OnSendRequestClick(object sender, RoutedEventArgs e)
        {
            string request = RequestTextBox.Text;
            string result = _proxy.Request(request);
            ResultTextBlock.Text = result;
        }
    }
}