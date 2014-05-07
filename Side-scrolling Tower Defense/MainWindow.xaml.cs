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
        int _timeInterval = 20;
        Player player ;
        AI ai ;

 
        public MainWindow()
        {
            InitializeComponent();
            player = new Player(lbMoney, lbMyTower_hp);
            ai = new AI(lbEnemyTower_hp);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(_timeInterval);
            timer.Tick += timer_Tick;
            timer.Start();

          //  reset();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            player.MoneyGain();
            lbMoney.Content ="$ "+ player.MONEY.ToString();

            player.MaintainSolidersPosition(ai.soldier, ai.aiTower);//移動Player的兵
            ai.Intelligence(player.soldier, grid1, lbEenemyTower, player.myTower);//AI智慧操作
            
            ai.aiTower.Attack(player.soldier);//塔要隨時判斷是否有攻擊對象，
            player.myTower.Attack(ai.soldier);//有就攻擊

            if (player.myTower.CRASHED)
            {
                MessageBox.Show("YOU LOSE !!!!!!");
                timer.Stop();
            }
            if (ai.aiTower.CRASHED)
            {
                MessageBox.Show("YOU WIN !!!!!!");
                timer.Stop();
            }

        } 
        
        private void btnUpgradeTower_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > player.UPGRADEPRICE)
            {
                player.UpgradeTower(lbMyTower);
            }
        }

        #region 產兵的buttonClick function
        private void btnSoldier1_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > 50)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 1, 50);
                player.MONEY -= 50;//第一種兵 $50
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
        }
        private void btnSoldier2_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > 100)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 2, 100);
                player.MONEY -= 100;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
        }

        private void btnSoldier3_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > 300)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 3, 300);
                player.MONEY -= 300;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
        }

        private void btnSoldier4_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > 500)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 4, 500);
                player.MONEY -= 500;
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

        private void btnSpeedUp_Click(object sender, RoutedEventArgs e)
        {
            if (btnSpeedUp.Content.ToString() == ">")
            {
             //   timer.Stop(); 
                _timeInterval = 1;
                btnSpeedUp.Content = ">>";
                timer.Start();
            }
            else if(btnSpeedUp.Content.ToString() == ">>")
            {
                timer.Stop();
                btnSpeedUp.Content = "||";
            }
            else
            {
                _timeInterval = 20;
                btnSpeedUp.Content = ">";
                timer.Start();
            }
            timer.Interval = TimeSpan.FromMilliseconds(_timeInterval);
        }
    }
}
