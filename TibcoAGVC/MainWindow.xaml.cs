using JxSystem.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TibcoAGVC.ServiceReference1;

namespace TibcoAGVC
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel mainViewModel = new MainViewModel();

            this.DataContext = mainViewModel;

            CoreModulesManager.Initialize(mainViewModel);
        }

        private void OnApplicationShutdown(object sender, CancelEventArgs e)
        {
            if (JxConfirmDialog.ShowDialog("Do You Want To Exit Application ?") == false)
            {
                e.Cancel = true;

                return;
            }

            CoreModulesManager.Shutdown();
        }

        private void BtnGetLoadPortMessage_Click(object sender, RoutedEventArgs e)
        {
            CoreModulesManager.McsLiteTcpClient.SendCommand("QueryLoadPortEvent");
        }

        private void BtnGetOutStockerMessage_Click(object sender, RoutedEventArgs e)
        {
            CoreModulesManager.McsLiteTcpClient.SendCommand("QueryOutStockerEvent");
        }

        private void BtnGetJobPrepareMessage_Click(object sender, RoutedEventArgs e)
        {
            CoreModulesManager.McsLiteTcpClient.SendCommand("QueryJobPrepareEvent");
        }
    }
}
