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
using System.Windows.Media.Animation;
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
        const int kSECOND = 50;
        int cdCounter = 0;

        #region 參數設定區
        /*-----------------Price--------------------*/
        private const int s1_price = 50;
        private const int s2_price = 100;
        private const int s3_price = 300;
        private const int s4_price = 111;
        private const int s5_price = 500;
        private const int s6_price = 502;
        private const int s7_price = 530;
        private const int unlock_s2_price = 1000;
        private const int unlock_s3_price = 2000;
        private const int unlock_s4_price = 3000;
        private const int unlock_s5_price = 3001;
        private const int unlock_s6_price = 3002;
        private const int unlock_s7_price = 3003;
        private const int skill1_price = 3000;
        private const int skill2_price = 3000;
        private const int skill3_price = 3000;
        private int overPower = 1;       //技能3--狂戰士倍率
        /*-----------------Price--------------------*/
        /*-----------------Flag--------------------*/
        private bool skill1_isEnable = false;// 敵方暫停 10秒
        private bool skill2_isEnable = false;// 我方塔攻擊距離無限 10秒
        private bool skill3_isEnable = false;// 我方兵 血、攻、移動速度 提升2倍 10秒
        /*-----------------Flag--------------------*/
        /*-----------------Counter--------------------*/
        private int buff1CountDown = 10;
        private int buff2CountDown = 10;
        private int buff3CountDown = 10;
        private int skillCounter1 = 0;
        private int skillCounter2 = 0;
        private int skillCounter3 = 0;
  //      private int skillCounter4 = 0;
        /*-----------------Counter--------------------*/
        private int[] coldDownTime = { 1, 2, 3, 1, 5, 6, 7, 0, 3, 100 };//順序: 兵種1~7 CD, 技能1~3 CD      
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Reset()
        {
            player = new Player(lbMoney, lbMyTower_hp, lbMyTower, grid1);
            ai = new AI(lbEnemyTower_hp, lbEenemyTower, grid1);
            #region Setting Content
            btnUpgradeTower.Content = "升級塔\n$" + player.UPGRADEPRICE.ToString();
            btnSoldier1.Content += "\n$" + s1_price.ToString();
            btnSoldier2.Content += "\n$" + s2_price.ToString();
            btnSoldier3.Content += "\n$" + s3_price.ToString();
            btnSoldier4.Content += "\n$" + s4_price.ToString();
            btnSoldier5.Content += "\n$" + s5_price.ToString();
            btnSoldier6.Content += "\n$" + s6_price.ToString();
            btnSoldier7.Content += "\n$" + s7_price.ToString();
            btnUnlock1.Content += unlock_s2_price.ToString();
            btnUnlock2.Content += unlock_s3_price.ToString();
            btnUnlock3.Content += unlock_s4_price.ToString();
            btnUnlock4.Content += unlock_s5_price.ToString();
            btnUnlock5.Content += unlock_s6_price.ToString();
            btnUnlock6.Content += unlock_s7_price.ToString();
            #endregion

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(_timeInterval);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            player.MoneyGain();
            lbMoney.Content ="$ "+ player.MONEY.ToString();
            
            player.MaintainSolidersPosition(ai.soldier, ai.aiTower);//移動Player的兵
            player.myTower.Attack(ai.soldier);//塔要隨時判斷是否有攻擊對象
            checkSkill();

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
                checkPrice();/**/
        } 
        private void LabelBlocking(Button btn, int CDtime) //即時產生擋住 btn 的 label
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
            if (player.MONEY < player.UPGRADEPRICE)
                LabelBlocking(btnUpgradeTower, 0);

            if (player.MONEY < s1_price && btnSoldier1.IsEnabled)
                LabelBlocking(btnSoldier1, 0);
            if (player.MONEY < s2_price && btnSoldier2.IsEnabled)
                LabelBlocking(btnSoldier2, 0);
            if (player.MONEY < s3_price && btnSoldier3.IsEnabled)
                LabelBlocking(btnSoldier3, 0);
            if (player.MONEY < s4_price && btnSoldier4.IsEnabled)
                LabelBlocking(btnSoldier4, 0);
            if (player.MONEY < s5_price && btnSoldier5.IsEnabled)
                LabelBlocking(btnSoldier5, 0);
            if (player.MONEY < s6_price && btnSoldier6.IsEnabled)
                LabelBlocking(btnSoldier6, 0);
            if (player.MONEY < s7_price && btnSoldier7.IsEnabled)
                LabelBlocking(btnSoldier7, 0);

            if (player.MONEY < unlock_s2_price && btnUnlock1.IsEnabled)
                LabelBlocking(btnUnlock1, 0);
            if (player.MONEY < unlock_s3_price && btnUnlock2.IsEnabled)
                LabelBlocking(btnUnlock2, 0);
            if (player.MONEY < unlock_s4_price && btnUnlock3.IsEnabled)
                LabelBlocking(btnUnlock3, 0);
            if (player.MONEY < unlock_s5_price && btnUnlock4.IsEnabled)
                LabelBlocking(btnUnlock4, 0);
            if (player.MONEY < unlock_s6_price && btnUnlock5.IsEnabled)
                LabelBlocking(btnUnlock5, 0);
            if (player.MONEY < unlock_s7_price && btnUnlock6.IsEnabled)
                LabelBlocking(btnUnlock6, 0);

            if (player.MONEY < skill1_price)
                LabelBlocking(skill1, 0);
            if (player.MONEY < skill2_price)
                LabelBlocking(skill2, 0);
            if (player.MONEY < skill3_price)
                LabelBlocking(skill3, 0);

        }
        private void checkSkill()
        {
            dock1.Children.Clear();
            if (skill1_isEnable) //判斷技能--時間暫停
            {
                AddImage(dock1, "Images/skill1.PNG");
                if ((++skillCounter1 % kSECOND) == 0)
                {
                    buff1CountDown--;
                    skillCounter1 = 0;
                   //tmp.Content = buff1CountDown.ToString();
                    if (buff1CountDown <= 0)
                    {
                        skill1_isEnable = false;
                       // Label name = (Label)this.dock1.FindName("image1");
                       // this.dock1.Children.Remove((Label)this.FindName("image1");
                    }
                }
            }
            else
            {
                ai.Intelligence(player.soldier, grid1, lbEenemyTower, player.myTower);//AI智慧操作
                ai.aiTower.Attack(player.soldier);//塔要隨時判斷是否有攻擊對象
            }

            if (skill2_isEnable) //判斷技能--無限射程
            {
                AddImage(dock1, "Images/skill2.PNG");
                if ((++skillCounter2 % kSECOND) == 0)
                {
                    buff2CountDown--;
                    skillCounter2 = 0;
            //        tmp.Content = buff2CountDown.ToString();
                    if (buff2CountDown <= 0)
                    {
                        skill2_isEnable = false;
                        player.myTower.RANGE -= 1500;
                    }
                }
            }

            if (skill3_isEnable) //判斷技能--狂戰士
            {
                AddImage(dock1, "Images/skill3.PNG");
                if ((++skillCounter3 % kSECOND) == 0)
                {
                    buff3CountDown--;
                    skillCounter3 = 0;
                //    tmp.Content = buff3CountDown.ToString();
                    if (buff3CountDown <= 0)
                    {
                        skill3_isEnable = false;
                        overPower = 1;
                        foreach (Soldier s in player.soldier)
                        {
                            s.ATK /= 2;
                            s.SPEED /= 2;
                            s.HP /= 2;
                        }
                    }
                }
            }
        }

        #region 產兵的 btnClick function
        private void btnSoldier1_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s1_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 1, overPower, s1_price);
                player.MONEY -= s1_price;//第一種兵 $50
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier1, coldDownTime[0]);
        }
        private void btnSoldier2_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s2_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 2, overPower, s2_price);
                player.MONEY -= s2_price;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier2, coldDownTime[1]);
        }

        private void btnSoldier3_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s3_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 3, overPower, s3_price);
                player.MONEY -= s3_price;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier3, coldDownTime[2]);
        }

        private void btnSoldier4_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s4_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 4, overPower, s4_price);
                player.MONEY -= s4_price;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier4, coldDownTime[3]);
        }
        private void btnSoldier5_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s5_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 5, overPower, s5_price);
                player.MONEY -= s5_price;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier5, coldDownTime[4]);
        }

        private void btnSoldier6_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s6_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 6, overPower, s6_price);
                player.MONEY -= s6_price;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier6, coldDownTime[5]);
        }

        private void btnSoldier7_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s7_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 7, overPower, s7_price);
                player.MONEY -= s7_price;
                lbMoney.Content = "$ " + player.MONEY.ToString();
            }
            LabelBlocking(btnSoldier7, coldDownTime[6]);
        }

        #endregion

        #region 解鎖士兵的 btnClick function
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
        private void btnUnlock4_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY >= unlock_s5_price)
            {
                player.MONEY -= unlock_s5_price;
                btnSoldier5.IsEnabled = true;
                btnUnlock4.IsEnabled = false;
                gridControlBar.Children.Remove(btnUnlock4);
            }
        }

        private void btnUnlock5_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY >= unlock_s6_price)
            {
                player.MONEY -= unlock_s6_price;
                btnSoldier6.IsEnabled = true;
                btnUnlock5.IsEnabled = false;
                gridControlBar.Children.Remove(btnUnlock5);
            }
        }

        private void btnUnlock6_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY >= unlock_s7_price)
            {
                player.MONEY -= unlock_s7_price;
                btnSoldier7.IsEnabled = true;
                btnUnlock6.IsEnabled = false;
                gridControlBar.Children.Remove(btnUnlock6);
            }
        }

        #endregion

        #region 玩家即時技 btnClick function
        private void skill1_Click(object sender, RoutedEventArgs e)
        {// 敵方暫停 10秒
            player.EarnMoney(-skill1_price); //扣錢
            skill1_isEnable = true;
            buff1CountDown = 10;
            LabelBlocking(skill1, coldDownTime[7]);

    
        }

        private void skill2_Click(object sender, RoutedEventArgs e)
        {// 塔攻擊距離無限 10秒
            player.EarnMoney(-skill2_price); //扣錢
            skill2_isEnable = true;
            buff2CountDown = 10;
            LabelBlocking(skill2, coldDownTime[8]);
            player.myTower.RANGE += 1500;
         
        }

        private void skill3_Click(object sender, RoutedEventArgs e)
        {// 我方兵 血、攻、移動速度 提升2倍 10秒
            player.EarnMoney(-skill3_price); //扣錢
            skill3_isEnable = true;
            buff3CountDown = 10;
            LabelBlocking(skill3, coldDownTime[9]);
            overPower = 2;
            foreach (Soldier s in player.soldier)
            {
                s.ATK *= 2;
                s.SPEED *= 2;
                s.HP *= 2;
            }

        }
        #endregion

        private void AddImage(DockPanel parent, string imageSource)
        {
            Label image = new Label();
            image.Content = new Image
            {
                Source = new BitmapImage(new Uri(imageSource, UriKind.Relative)),
                VerticalAlignment = VerticalAlignment.Center
            };
            parent.Children.Add(image);
        }
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
                _timeInterval = 10;
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
        
        #region MenuBtn Click
        private void startGame_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            gridBG.Visibility = Visibility.Hidden;
        }
        private void about_Click(object sender, RoutedEventArgs e)
        {
            Window1 about = new Window1();
            about.Show();
        }
        private void howTo_Click(object sender, RoutedEventArgs e)
        {

        }        
        #endregion
    }
}
