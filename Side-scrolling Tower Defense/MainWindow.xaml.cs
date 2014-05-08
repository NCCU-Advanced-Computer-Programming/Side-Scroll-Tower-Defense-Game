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

        List<Label> lbCD = new List<Label>();
        int kSECOND;
        int cdCounter = 0;

        #region 設定參數區
        private const int s1_price = 50;
        private const int s2_price = 100;
        private const int s3_price = 300;
        private const int s4_price = 500;
        private const int unlock_s2_price = 1000;
        private const int unlock_s3_price = 2000;
        private const int unlock_s4_price = 3000;

       
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            player = new Player(lbMoney, lbMyTower_hp);
            ai = new AI(lbEnemyTower_hp);
            btnUpgradeTower.Content = "升級塔\n$" + player.UPGRADEPRICE.ToString();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(_timeInterval);
            kSECOND = 1000 / _timeInterval;
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
            if ((++cdCounter % kSECOND) == 0)
                checkCD();
            else
                checkPrice();
        } 
        private void LabelBlocking(Button btn, int CDtime) //即時產生擋住 btn 的label
        {

            Label tmp = new Label();
            tmp.Height = btn.Height;
            tmp.Width = btn.Width;
            tmp.Margin = new Thickness(btn.Margin.Left, btn.Margin.Top, btn.Margin.Right, btn.Margin.Bottom);
            tmp.Background = System.Windows.Media.Brushes.Black;
            tmp.Foreground = System.Windows.Media.Brushes.White;
            tmp.VerticalAlignment = VerticalAlignment.Top;
            tmp.HorizontalAlignment = HorizontalAlignment.Left;
            tmp.Opacity = 0.7;
            tmp.Visibility = Visibility.Visible;

            if (CDtime != 0)
                tmp.Content = CDtime.ToString();
            else
                tmp.Content = "";

            bool isBlocked = false;     //判斷這個按鈕是否已被擋住(無論是因CD或是因錢不夠)
            foreach (Label l in lbCD)
                if (l.Margin == tmp.Margin)
                    isBlocked = true;
            if (!isBlocked)
            {
                gridControlBar.Children.Add(tmp);
                lbCD.Add(tmp);
            }
        }
        private void checkCD()
        {
            for (int i = 0; i < lbCD.Count; i++ )
            {
                if (lbCD[i] != null && lbCD[i].Content.ToString() != "")
                {
                    int res;
                    Int32.TryParse(lbCD[i].Content.ToString(), out res);
                    if (res > 1)
                    {
                        lbCD[i].Content = (res - 1).ToString();
                    }
                    else
                    {
                        lbCD[i].Visibility = Visibility.Hidden;
                        gridControlBar.Children.Remove(lbCD[i]);
                        lbCD.RemoveAt(i);
                    }
                }
            }
        }
        private void checkPrice()
        {
            for (int i = 0; i < lbCD.Count; i++)  //先把所有因為錢不夠而禁案的鈕解開，重新判斷
            {
                if (lbCD[i].Content.ToString() == "")
                {
                    gridControlBar.Children.Remove(lbCD[i]);
                    lbCD.RemoveAt(i);
                }
            }
            if (player.MONEY < s1_price && btnSoldier1.IsEnabled)
                LabelBlocking(btnSoldier1, 0);
            if (player.MONEY < s2_price && btnSoldier2.IsEnabled)
                LabelBlocking(btnSoldier2, 0);
            if (player.MONEY < s3_price && btnSoldier3.IsEnabled)
                LabelBlocking(btnSoldier3, 0);
            if (player.MONEY < s4_price && btnSoldier4.IsEnabled)
                LabelBlocking(btnSoldier4, 0);
            if (player.MONEY < unlock_s2_price && btnUnlock1.IsEnabled)
                LabelBlocking(btnUnlock1, 0);
            if (player.MONEY < unlock_s3_price && btnUnlock2.IsEnabled)
                LabelBlocking(btnUnlock2, 0);
            if (player.MONEY < unlock_s4_price && btnUnlock3.IsEnabled)
                LabelBlocking(btnUnlock3, 0);
        }

        #region 產兵的buttonClick function
        private void btnSoldier1_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s1_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 1, s1_price);
                player.MONEY -= s1_price;//第一種兵 $50
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier1, 2);
        }
        private void btnSoldier2_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s2_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 2, s2_price);
                player.MONEY -= s2_price;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier2, 5);
        }

        private void btnSoldier3_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s3_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 3, s3_price);
                player.MONEY -= s3_price;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier3, 12);
        }

        private void btnSoldier4_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s4_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 4, s4_price);
                player.MONEY -= s4_price;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier4, 12);
        }
        #endregion

        #region 解鎖士兵的click function
        private void btnUnlock1_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY >= unlock_s2_price)
            {
                player.MONEY -= unlock_s2_price;
                btnSoldier2.IsEnabled = true;
              //  btnUnlock1.Visibility = Visibility.Hidden;
                btnUnlock1.IsEnabled = false;
                gridControlBar.Children.Remove(btnUnlock1);
            }
        }

        private void btnUnlock2_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY >= unlock_s3_price)
            {
                player.MONEY -= unlock_s3_price;
                btnSoldier3.IsEnabled = true;
                btnUnlock2.IsEnabled = false;
                gridControlBar.Children.Remove(btnUnlock2);
            }
        }

        private void btnUnlock3_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY >= unlock_s4_price)
            {
                player.MONEY -= unlock_s4_price;
                btnSoldier4.IsEnabled = true;
                btnUnlock3.IsEnabled = false;
                gridControlBar.Children.Remove(btnUnlock3);
            }
        }
        #endregion

        private void btnUpgradeTower_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > player.UPGRADEPRICE)
            {
                player.UpgradeTower(lbMyTower);
                btnUpgradeTower.Content = "升級塔\n$" + player.UPGRADEPRICE.ToString();
            }
        }
        private void btnSpeedUp_Click(object sender, RoutedEventArgs e)
        {
            if (btnSpeedUp.Content.ToString() == ">")
            {
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
