using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Side_scrolling_Tower_Defense
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
 //       int _timeCount = 0;
        Player player = new Player();
        AI ai = new AI();
 
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;
            timer.Start();

          //  reset();
            ai.GenerateSolider(grid1);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            player.MONEY += 1;
            lbMoney.Content ="$ "+ player.MONEY.ToString();

            //移動Player的兵
            player.MaintainSolidersPosition();
            ai.MaintainSolidersPosition();
            //lbMyTower.Margin = new Thickness(lbMyTower.Margin.Left - 0.3, lbMyTower.Margin.Top, lbMyTower.Margin.Right + 0.3, lbMyTower.Margin.Bottom);

        } 
        
        private void btnUpgradeTower_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > player.UPGRADEPRICE)
            {
                player.UpgradeTower(lbTowerProperty);
            }
        }
        #region 產兵的buttonClick function
        private void btnSoldier1_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > 50)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1);
                player.MONEY -= 50;//第一種兵 $50
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
        }
        #endregion

        #region 解鎖士兵的click function
        private void btnUnlock1_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY >= 1000)
            {
                player.MONEY -= 1000;
                btnSoldier2.IsEnabled = true;
                btnUnlock1.Visibility = Visibility.Hidden;
                
            }
        }

        private void btnUnlock2_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY >= 2000)
            {
                player.MONEY -= 2000;
                btnSoldier3.IsEnabled = true;
                btnUnlock2.Visibility = Visibility.Hidden;
            }
        }

        private void btnUnlock3_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY >= 3000)
            {
                player.MONEY -= 3000;
                btnSoldier4.IsEnabled = true;
                btnUnlock3.Visibility = Visibility.Hidden;
            }
        }
        #endregion
    }
}
