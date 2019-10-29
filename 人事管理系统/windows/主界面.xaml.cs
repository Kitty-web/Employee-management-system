using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace 人事管理系统.windows
{
    /// <summary>
    /// 主界面.xaml 的交互逻辑
    /// </summary>
    public partial class 主界面 : Window
    {
        public 主界面()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void addEmp_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri(@"AddStaff.xaml", UriKind.Relative);
        }

        private void addDept_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri(@"AddDept.xaml", UriKind.Relative);
        }

        private void checkOn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri(@"CheckOn.xaml", UriKind.Relative);
        }

        private void askLeave_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri(@"AskLeave.xaml", UriKind.Relative);
        }

        private void rewardOrPunish_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri(@"RewardOrPunish.xaml", UriKind.Relative);
        }

        private void salary_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri(@"Salary.xaml", UriKind.Relative);
        }

        private void workOutside_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri(@"WorkOutside.xaml", UriKind.Relative);
        }

        private void assess_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri(@"Assess.xaml", UriKind.Relative);
        }

        private void personnelTransfer_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri(@"PersonnelTransfer.xaml", UriKind.Relative);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定退出系统？", "退出系统", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                App.Current.Shutdown();
            }
        }

        private void btnExit2_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定退出系统？", "退出系统", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                App.Current.Shutdown();
            }
        }
    }
}
