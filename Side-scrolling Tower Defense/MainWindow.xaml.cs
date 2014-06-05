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
using WpfAnimatedGif;

namespace Side_scrolling_Tower_Defense
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer         timer;
        int                     _timeInterval ;
        int                     time                    = 0;
        Player                  player ;
        AI                      ai ;

        Label                   totalTimeDisplay        = new Label(); //
        List<Label>             lbCD                    = new List<Label>(); //遮蓋兵種按鈕的Cool-Down Label 
        private const int       kSECOND                 = 40; //Tick.Interval = 25ms , 40*Interval = 1 second
        private int             cdCounter               = 0; //CD時間計數 (40 為1秒)
        int                     timeCounter             = 0; //遊戲時間記數 (40 為1秒)
        private bool            isStarted               = false;
        DockPanel               dock1                   = new DockPanel();
        Button                  btnResume;

        #region 參數設定區
        /*-----------------Price--------------------*/
        private const int       s1_price                = 100;
        private const int       s2_price                = 150;
        private const int       s3_price                = 200;
        private const int       s4_price                = 300;
        private const int       s5_price                = 400;
        private const int       s6_price                = 500;
        private const int       s7_price                = 600;
        private const int       unlock_s2_price         = 500;
        private const int       unlock_s3_price         = 500;
        private const int       unlock_s4_price         = 700;
        private const int       unlock_s5_price         = 1000;
        private const int       unlock_s6_price         = 1500;
        private const int       unlock_s7_price         = 2000;
        //private const int     skill1_price            = 3000;
        //private const int     skill2_price            = 2000;
        //private const int     skill3_price            = 5000;
        //private const int     skill4_price            = 5000;
        private int             overPower               = 1;       //技能3--狂戰士倍率
        /*-----------------Price--------------------*/
        /*-----------------Flag--------------------*/
        private bool            skill1_isEnable         = false;// 敵方暫停 10秒
        private bool            skill2_isEnable         = false;// 我方塔攻擊距離無限 10秒
        private bool            skill3_isEnable         = false;// 我方兵 血、攻、移動速度 提升2倍 10秒
        /*-----------------Flag--------------------*/
        /*-----------------Counter--------------------*/
        private int             buff1CountDown          = 10;
        private int             buff2CountDown          = 10;
        private int             buff3CountDown          = 10;
        private int             skillCounter1           = 0;
        private int             skillCounter2           = 0;
        private int             skillCounter3           = 0;
  //    private int             skillCounter4           = 0;
        /*-----------------Counter--------------------*/
        private int[]           coldDownTime            = { 5, 8, 10, 15, 20, 30, 40, 40, 30, 70, 60 };//順序: 兵種1~7 CD, 技能1~4 CD      
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            InitializeToolTip();
        }

        private void InitializeToolTip()
        {
            //動態設定下方面板中部分元件的 Tooltip
            player = new Player(grid1, gridTopBar);
            foreach (Button btn in gridControlBar.Children)
            {
                ToolTip tp = new System.Windows.Controls.ToolTip();
                tp.Background = Brushes.LightSteelBlue;
                tp.BorderBrush = Brushes.Black;
                tp.BorderThickness = new Thickness(2);
                tp.Content = btn.ToolTip;
                //if (btn.Name == "btnUpgradeTower")
                //    tp.Content = "下一級:\n血　量:" + (player.myTower.HP + 100).ToString() + '\n' + "射　程:" + (player.myTower.RANGE + 10).ToString() + '\n' + "攻擊力:" + (player.myTower.ATK + 10).ToString();
                if (btn.Name.ToString().Contains("Unlock"))
                    tp.Content = "解鎖兵種";
                btn.ToolTip = tp;
            }

        }

        private void Reset(int difficulty)
        {
            #region 清除遊戲畫面、上方面板與下方面板的LabelCD
            grid1.Children.Clear();
            gridTopBar.Children.Clear();
   
            foreach (Label lb in lbCD)
                gridControlBar.Children.Remove(lb);
            lbCD.Clear();
            #endregion

            //Resetting dockpanel, 顯示 BUFF
            #region BUFF Panel resetting
            dock1.Width = 105;
            dock1.Height = 35;
            dock1.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            dock1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            dock1.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            dock1.Margin = new Thickness(800,17,0,0);
            //dock1.Background = Brushes.LightGray;
            grid1.Children.Add(dock1);
            #endregion

            //Resetting all btn 
            #region 士兵解鎖與購買按鈕
            btnUnlock1.Visibility = System.Windows.Visibility.Visible;
            btnUnlock2.Visibility = System.Windows.Visibility.Visible;
            btnUnlock3.Visibility = System.Windows.Visibility.Visible;
            btnUnlock4.Visibility = System.Windows.Visibility.Visible;
            btnUnlock5.Visibility = System.Windows.Visibility.Visible;
            btnUnlock6.Visibility = System.Windows.Visibility.Visible;

            btnSoldier2.IsEnabled = false;
            btnSoldier3.IsEnabled = false;
            btnSoldier4.IsEnabled = false;
            btnSoldier5.IsEnabled = false;
            btnSoldier6.IsEnabled = false;
            btnSoldier7.IsEnabled = false;

            btnUnlock1.IsEnabled = true;
            btnUnlock2.IsEnabled = true;
            btnUnlock3.IsEnabled = true;
            btnUnlock4.IsEnabled = true;
            btnUnlock5.IsEnabled = true;
            btnUnlock6.IsEnabled = true;
            #endregion

            #region 技能按鈕與技能冷卻時間倒數
            skill1_isEnable = false;
            skill2_isEnable = false;
            skill3_isEnable = false;

            skillCounter1 = 0;
            skillCounter2 = 0;
            skillCounter3 = 0;
            #endregion

            #region 遊戲進行時間計時面板
            grid1.Children.Add(totalTimeDisplay);
            totalTimeDisplay.Margin = new Thickness(0, 30, 0, 0);
            totalTimeDisplay.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            totalTimeDisplay.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            totalTimeDisplay.Content = "00:00";
            #endregion

            isStarted = true;
            player = new Player(grid1, gridTopBar);
            ai = new AI(grid1, gridTopBar, player, difficulty);

            #region 下方面板按鈕 Content
            btnSpeedUp.Content = ">>";

            #region 升級塔按鈕
            btnUpgradeTower.Content = "升級塔\n$" + player.UPGRADEPRICE.ToString();
            ToolTip tp = new System.Windows.Controls.ToolTip();
            tp = new System.Windows.Controls.ToolTip();
            tp.Background = Brushes.LightSteelBlue;
            tp.BorderBrush = Brushes.Black;
            tp.BorderThickness = new Thickness(2);
            //tp.Content = btnUpgradeTower.ToolTip.ToString();
            tp.Content = "下一級:\n血　量:" + (player.myTower.HP + 100).ToString() + '\n' + "射　程:" + (player.myTower.RANGE + 10).ToString() + '\n' + "攻擊力:" + (player.myTower.ATK + 10).ToString();
            btnUpgradeTower.ToolTip = tp;
            #endregion

            foreach (Button btn in gridControlBar.Children)
                if (btn.IsEnabled == false)
                    btn.Opacity = 0.5;
            
            #region 解鎖按鈕內容
            btnUnlock1.Content = "$" + unlock_s2_price.ToString();
            btnUnlock2.Content = "$" + unlock_s3_price.ToString();
            btnUnlock3.Content = "$" + unlock_s4_price.ToString();
            btnUnlock4.Content = "$" + unlock_s5_price.ToString();
            btnUnlock5.Content = "$" + unlock_s6_price.ToString();
            btnUnlock6.Content = "$" + unlock_s7_price.ToString(); 
            #endregion
            #endregion

            #region 設定計時器
            _timeInterval = 25;
            timeCounter = 0;
            time = 0;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(_timeInterval);
            timer.Tick += timer_Tick;
            timer.Start();
            #endregion

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //遊戲時間計時
            if ((++timeCounter % kSECOND) == 0)
            {
                timeCounter = 0;
                TimeSpan t = TimeSpan.FromSeconds(++time);
                totalTimeDisplay.Content = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            }

            //金錢增加
            player.MoneyGain();

            //玩家塔攻擊判斷
            player.myTower.Attack(ai.soldier);

            //玩家士兵移動
            player.MaintainSolidersPosition(ai.soldier, ai.aiTower);

            //技能圖示動畫與技能結束檢查
            checkSkill();

            if ((++cdCounter % kSECOND) == 0)
                checkCD();

            checkPrice();

            #region GAMEOVER
            if (player.myTower.CRASHED || ai.aiTower.CRASHED)
            {
                isStarted = false;
                foreach (Soldier s in ai.soldier)
                {
                    var controller = ImageBehavior.GetAnimationController(s.Image);
                    controller.Pause();
                }
                foreach (Soldier s in player.soldier)
                {
                    var controller = ImageBehavior.GetAnimationController(s.Image);
                    controller.Pause();
                }
                timer.Stop();
                if (player.myTower.CRASHED)
                    MessageBox.Show("歷經了 " + totalTimeDisplay.Content.ToString() + " 的苦戰，您還是被打爆了~!");
                else
                    MessageBox.Show("歷經了 " + totalTimeDisplay.Content.ToString() + " 的苦戰，您終於獲勝了~!");
                btnReturnToMenu_Click(null,null);
            }
            #endregion


        } 
        private void LabelBlocking(Button btn, int CDtime) //即時產生擋住 btn 的 label
        {
            #region 產生labelCD
            Label tmp = new Label();
            tmp.Height = btn.Height;
            tmp.Width = btn.Width;
            tmp.Margin = new Thickness(btn.Margin.Left, btn.Margin.Top, btn.Margin.Right, btn.Margin.Bottom);
            tmp.Background = System.Windows.Media.Brushes.Black;
            tmp.Foreground = System.Windows.Media.Brushes.White;
            tmp.FontWeight = FontWeights.Bold;
            tmp.VerticalAlignment = VerticalAlignment.Top;
            tmp.HorizontalAlignment = HorizontalAlignment.Left;
            tmp.Opacity = 0.7;
            tmp.Visibility = Visibility.Visible;
            tmp.ToolTip = btn.ToolTip;

            /*如果CD為0就不會在checkCD()進行檢查*/
            if (CDtime != 0)
                tmp.Content = CDtime.ToString();
            else
                tmp.Content = "";
            #endregion

            #region LabelCD 遮住Button
            bool isBlocked = false;     //判斷這個按鈕是否已被擋住(無論是因CD或是因錢不夠)
            //lbCD無綁定該button之Index
            foreach (Label l in lbCD)
                if (l.Margin == tmp.Margin)
                    isBlocked = true;
            if (!isBlocked)
            {
                gridControlBar.Children.Add(tmp);
                lbCD.Add(tmp);
            }
            #endregion
        }

        private void checkCD()
        {

            for (int i = 0; i < lbCD.Count; i++ )
            {
                if (lbCD[i] != null && lbCD[i].Content.ToString() != "")
                {
                    int res;//冷卻時間
                    Int32.TryParse(lbCD[i].Content.ToString(), out res);
                    if (res > 1)
                    {
                        lbCD[i].Content = (res - 1).ToString();
                    }
                    else
                    {
                        //lbCD[i].Visibility = Visibility.Hidden;
                        gridControlBar.Children.Remove(lbCD[i]);
                        lbCD.RemoveAt(i);
                    }
                }
            }
        }

        private void checkPrice()
        {
            #region 升級塔按鈕
            if (player.MONEY < player.UPGRADEPRICE)
                LabelBlocking(btnUpgradeTower, 0);
            else
            {
                foreach(Label l in lbCD)
                    if(l.Margin == btnUpgradeTower.Margin && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 1 按鈕
            if (player.MONEY < s1_price && btnSoldier1.IsEnabled)
                LabelBlocking(btnSoldier1, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnSoldier1.Margin && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 2 按鈕
            if (player.MONEY < s2_price && btnSoldier2.IsEnabled)
                LabelBlocking(btnSoldier2, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnSoldier2.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 3 按鈕
            if (player.MONEY < s3_price && btnSoldier3.IsEnabled)
                LabelBlocking(btnSoldier3, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnSoldier3.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 4 按鈕
            if (player.MONEY < s4_price && btnSoldier4.IsEnabled)
                LabelBlocking(btnSoldier4, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnSoldier4.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 5 按鈕
            if (player.MONEY < s5_price && btnSoldier5.IsEnabled)
                LabelBlocking(btnSoldier5, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnSoldier5.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 6 按鈕
            if (player.MONEY < s6_price && btnSoldier6.IsEnabled)
                LabelBlocking(btnSoldier6, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnSoldier6.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 7 按鈕
            if (player.MONEY < s7_price && btnSoldier7.IsEnabled)
                LabelBlocking(btnSoldier7, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnSoldier7.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 2 解鎖按鈕
            if (player.MONEY < unlock_s2_price && btnUnlock1.IsEnabled)
                LabelBlocking(btnUnlock1, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnUnlock1.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 3 解鎖按鈕
            if (player.MONEY < unlock_s3_price && btnUnlock2.IsEnabled)
                LabelBlocking(btnUnlock2, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnUnlock2.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 4 解鎖按鈕
            if (player.MONEY < unlock_s4_price && btnUnlock3.IsEnabled)
                LabelBlocking(btnUnlock3, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnUnlock3.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 5 解鎖按鈕
            if (player.MONEY < unlock_s5_price && btnUnlock4.IsEnabled)
                LabelBlocking(btnUnlock4, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnUnlock4.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 6 解鎖按鈕
            if (player.MONEY < unlock_s6_price && btnUnlock5.IsEnabled)
                LabelBlocking(btnUnlock5, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnUnlock5.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

            #region 士兵 7 解鎖按鈕
            if (player.MONEY < unlock_s7_price && btnUnlock6.IsEnabled)
                LabelBlocking(btnUnlock6, 0);
            else
            {
                foreach (Label l in lbCD)
                    if (l.Margin == btnUnlock6.Margin  && l.Content.ToString() == "")
                    {
                        gridControlBar.Children.Remove(l);
                        lbCD.Remove(l);
                        break;
                    }
            }
            #endregion

        }

        /*處理技能冷卻時間倒數與技能圖示動畫*/
        private void checkSkill()
        {
            //技能消失前閃爍
            const int kFlashingInterval = 5;

            dock1.Children.Clear();

            #region 時間暫停
            if (skill1_isEnable) //判斷技能--時間暫停
            {
                Label tmplb = AddImage(dock1, "Images/skill1.PNG");

                //技能持續時間剩3秒以下，技能狀態圖示閃爍
                if (buff1CountDown <= 3 && (skillCounter1% 10) < kFlashingInterval)
                {
                    tmplb.Visibility = Visibility.Hidden;
                }
                //技能持續時間以每秒做計算(40個skillCounter為1秒)
                if ((++skillCounter1 % kSECOND) == 0)
                {
                    //技能效果維持時間減少 1 秒
                    buff1CountDown--;
                    //計秒器歸零
                    skillCounter1 = 0;

                    #region 時間暫停效果結束
                    if (buff1CountDown <= 0)
                    {
                        skill1_isEnable = false;
                        foreach (Soldier s in ai.soldier)
                        {
                            var controller = ImageBehavior.GetAnimationController(s.Image);
                            controller.Play();
                        }

                    }
                    #endregion
                }
            }
            else
            {
                ai.aiTower.Attack(player.soldier);//塔要隨時判斷是否有攻擊對象
                ai.Intelligence();//AI智慧操作
            }
            #endregion

            #region 無限射程
            if (skill2_isEnable) //判斷技能--無限射程
            {
                Label tmplb = AddImage(dock1, "Images/skill2.PNG");
                //能持續時間剩２秒以下，技能狀態圖示閃爍
                if (buff2CountDown <= 2 && (skillCounter2 % 10) < kFlashingInterval)
                {
                    tmplb.Visibility = Visibility.Hidden;
                } 
                #region 無限射程效果結束
                if ((++skillCounter2 % kSECOND) == 0)
                {
                    buff2CountDown--;
                    skillCounter2 = 0;
                    if (buff2CountDown <= 0)
                    {
                        skill2_isEnable = false;
                        //(ToolTip)(btnUpgradeTower.ToolTip).tp.Content = "下一級:\n血　量:" + (player.myTower.HP + 100).ToString() + '\n' + "射　程:" + (player.myTower.RANGE + 10).ToString() + '\n' + "攻擊力:" + (player.myTower.ATK + 10).ToString();
                        player.myTower.RANGE -= 1500;
                    }
                }
                #endregion
            }
            #endregion

            #region 狂戰士
            if (skill3_isEnable) //判斷技能--狂戰士
            {
                Label tmplb = AddImage(dock1, "Images/skill3.PNG");
                if (buff3CountDown <= 2 && (skillCounter2 % 10) < kFlashingInterval)
                {
                    tmplb.Visibility = Visibility.Hidden;
                } 
                if ((++skillCounter3 % kSECOND) == 0)
                {
                    buff3CountDown--;
                    skillCounter3 = 0;
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
            #endregion

        }

        #region 產兵的 btnClick function
        private void btnSoldier1_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s1_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 1, overPower);
                player.EarnMoney(-s1_price);
            }
            LabelBlocking(btnSoldier1, coldDownTime[0]);
        }
        private void btnSoldier2_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s2_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 2, overPower);
                player.EarnMoney(-s2_price);
            }
            LabelBlocking(btnSoldier2, coldDownTime[1]);
        }
        private void btnSoldier3_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s3_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 3, overPower);
                player.EarnMoney(-s3_price);
            }
            LabelBlocking(btnSoldier3, coldDownTime[2]);
        }
        private void btnSoldier4_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s4_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 4, overPower);
                player.EarnMoney(-s4_price);
            }
            LabelBlocking(btnSoldier4, coldDownTime[3]);
        }
        private void btnSoldier5_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s5_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 5, overPower);
                player.EarnMoney(-s5_price);
            }
            LabelBlocking(btnSoldier5, coldDownTime[4]);
        }
        private void btnSoldier6_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s6_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 6, overPower);
                player.EarnMoney(-s6_price);
            }
            LabelBlocking(btnSoldier6, coldDownTime[5]);
        }
        private void btnSoldier7_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > s7_price)   //錢夠才能產兵
            {
                player.GenerateSolider(grid1, 7, overPower);
                player.EarnMoney(-s7_price);
            }
            LabelBlocking(btnSoldier7, coldDownTime[6]);
        }
        #endregion

        #region 解鎖士兵的 btnClick function
        private void btnUnlock1_Click(object sender, RoutedEventArgs e)
        {
            player.MONEY -= unlock_s2_price;
            btnSoldier2.IsEnabled = true;
            btnSoldier2.Opacity = 1;
            btnUnlock1.Visibility = Visibility.Hidden;
            btnUnlock1.IsEnabled = false;
        }
        private void btnUnlock2_Click(object sender, RoutedEventArgs e)
        {
            player.MONEY -= unlock_s3_price;
            btnSoldier3.IsEnabled = true;
            btnSoldier3.Opacity = 1;
            btnUnlock2.Visibility = System.Windows.Visibility.Hidden;
            btnUnlock2.IsEnabled = false;
        }
        private void btnUnlock3_Click(object sender, RoutedEventArgs e)
        {
            player.MONEY -= unlock_s4_price;
            btnSoldier4.IsEnabled = true;
            btnSoldier4.Opacity = 1;
            btnUnlock3.Visibility = System.Windows.Visibility.Hidden;
            btnUnlock3.IsEnabled = false;
        }
        private void btnUnlock4_Click(object sender, RoutedEventArgs e)
        {
            player.MONEY -= unlock_s5_price;
            btnSoldier5.Opacity = 1;
            btnSoldier5.IsEnabled = true;
            btnUnlock4.Visibility = System.Windows.Visibility.Hidden;
            btnUnlock4.IsEnabled = false;
        }
        private void btnUnlock5_Click(object sender, RoutedEventArgs e)
        {
            player.MONEY -= unlock_s6_price;
            btnSoldier6.IsEnabled = true;
            btnSoldier6.Opacity = 1;
            btnUnlock5.Visibility = System.Windows.Visibility.Hidden;
            btnUnlock5.IsEnabled = false;
        }
        private void btnUnlock6_Click(object sender, RoutedEventArgs e)
        {
            player.MONEY -= unlock_s7_price;
            btnSoldier7.IsEnabled = true;
            btnSoldier7.Opacity = 1;
            btnUnlock6.Visibility = System.Windows.Visibility.Hidden;
            btnUnlock6.IsEnabled = false;
        }

        #endregion

        #region 玩家即時技 btnClick function
        private void skill1_Click(object sender, RoutedEventArgs e)
        {// 敵方暫停 10秒
            skill1_isEnable = true;
            buff1CountDown = 10;
            foreach (Soldier s in ai.soldier)
            {
                var controller = ImageBehavior.GetAnimationController(s.Image);
                controller.Pause();
            }
            LabelBlocking(skill1, coldDownTime[7]);
        }
        private void skill2_Click(object sender, RoutedEventArgs e)
        {// 塔攻擊距離無限 10秒
            //player.EarnMoney(-skill2_price); //扣錢
            skill2_isEnable = true;
            buff2CountDown = 10;
            LabelBlocking(skill2, coldDownTime[8]);
            player.myTower.RANGE += 1500;
        }
        private void skill3_Click(object sender, RoutedEventArgs e)
        {// 我方兵 血、攻、移動速度 提升2倍 10秒
            //player.EarnMoney(-skill3_price); //扣錢
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
        private void skill4_Click(object sender, RoutedEventArgs e)
        {//秒殺全場敵人
            //player.EarnMoney(-skill4_price); //扣錢
            player.myTower.Skill(ai.soldier);
            LabelBlocking(skill4, coldDownTime[10]);
        }

        #endregion

        private Label AddImage(DockPanel parent, string imageSource)
        {
            Label image = new Label();
            image.Content = new Image
            {
                Source = new BitmapImage(new Uri(imageSource, UriKind.Relative)),
                VerticalAlignment = VerticalAlignment.Center
            };
            parent.Children.Add(image);
            return image;
        }

        private void btnUpgradeTower_Click(object sender, RoutedEventArgs e)
        {
            if (player.MONEY > player.UPGRADEPRICE)
            {
                player.UpgradeTower();
                btnUpgradeTower.Content = "升級塔\n$" + player.UPGRADEPRICE.ToString();
                ToolTip tp = new System.Windows.Controls.ToolTip();
                tp = new System.Windows.Controls.ToolTip();
                tp.Background = Brushes.LightSteelBlue;
                tp.BorderBrush = Brushes.Black;
                tp.BorderThickness = new Thickness(2);
                tp.Content = btnUpgradeTower.ToolTip.ToString();
                if(skill2_isEnable)
                    tp.Content = "下一級:\n血　量: " + (player.myTower.HP + 100).ToString() + '\n' + "射　程: " + (player.myTower.RANGE - 990).ToString() + '\n' + "攻擊力: " + (player.myTower.ATK + 10).ToString();
                else
                    tp.Content = "下一級:\n血　量: " + (player.myTower.HP + 100).ToString() + '\n' + "射　程: " + (player.myTower.RANGE + 10).ToString() + '\n' + "攻擊力: " + (player.myTower.ATK + 10).ToString();
                btnUpgradeTower.ToolTip = tp;
            }
        }

        private void btnSpeedUp_Click(object sender, RoutedEventArgs e)
        {
            if (btnSpeedUp.Content.ToString() == ">>")
            {
                _timeInterval = 10;
                timer.Interval = TimeSpan.FromMilliseconds(_timeInterval);
                btnSpeedUp.Content = "||";
            }
            else if(btnSpeedUp.Content.ToString() == "||")
            {
                timer.IsEnabled=false;
                foreach (Soldier s in ai.soldier)
                {
                    var controller = ImageBehavior.GetAnimationController(s.Image);
                    controller.Pause();
                }
                foreach (Soldier s in player.soldier)
                {
                    var controller = ImageBehavior.GetAnimationController(s.Image);
                    controller.Pause();
                }
                btnSpeedUp.Content = ">";
            }
            else if (btnSpeedUp.Content.ToString() == ">")
            {
                foreach (Soldier s in ai.soldier)
                {
                    var controller = ImageBehavior.GetAnimationController(s.Image);
                    controller.Play();
                }
                foreach (Soldier s in player.soldier)
                {
                    var controller = ImageBehavior.GetAnimationController(s.Image);
                    controller.Play();
                }
                _timeInterval = 25;
                timer.Interval = TimeSpan.FromMilliseconds(_timeInterval);
                btnSpeedUp.Content = ">>";
                timer.IsEnabled = true ;
            }
            else
            {
                MessageBox.Show("Error with SpeedUp btn");
            }

        }

        private void btnReturnToMenu_Click(object sender, RoutedEventArgs e)

        {
            if (isStarted)
            {

                if (btnResume == null)
                {
                    btnResume = new Button();
                    btnResume.Content = "繼　續";
                    btnResume.Height = 45;
                    btnResume.Margin = new Thickness(0,10,0,10);
                    btnResume.Padding = new Thickness(5);
                    btnResume.FontSize = 16;
                    btnResume.FontWeight = FontWeights.Bold;
                    btnResume.Background = null;
                    btnResume.BorderBrush = null;
                    btnResume.Foreground = Brushes.White;
                    btnResume.Click += btnResume_Click;
                    stackMenu.Children.Insert(0, btnResume);
                }

                else 
                {
                    btnResume.Visibility = Visibility.Visible;
                }

                timer.IsEnabled = false;
                gridBG.Visibility = Visibility.Visible;
                stackMenu.IsEnabled = true;
                spDiffcultly.Visibility = System.Windows.Visibility.Hidden;

                /*
                if (MessageBox.Show("遊戲還在進行，確定要返回主選單嗎?", "確認", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    isStarted = false;
                    timer.Stop();
                    timer = null;
                    gridBG.Visibility = System.Windows.Visibility.Visible;
                    stackMenu.IsEnabled = true;
                    spDiffcultly.Visibility = System.Windows.Visibility.Hidden;
                }*/
            }
            else
            {
                if (btnResume != null && stackMenu.Children.Contains(btnResume))
                    stackMenu.Children.Remove(btnResume);
                gridBG.Visibility = System.Windows.Visibility.Visible;
                //gridBG.Visibility = System.Windows.Visibility.Visible;
                stackMenu.IsEnabled = true;
                spDiffcultly.Visibility = System.Windows.Visibility.Hidden;
            }

        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            gridBG.Visibility = Visibility.Hidden;
            btnResume.Visibility = Visibility.Hidden;
            timer.IsEnabled = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                btnReturnToMenu_Click(null, null);
        }
   
        #region MenuBtn Click
        private void startGame_Click(object sender, RoutedEventArgs e)
        {
          //  stackMenu.Opacity = 0.6;
            //stackMenu.IsEnabled = false;
            if (!spDiffcultly.IsVisible)
            {
                spDiffcultly.IsEnabled = true;
                spDiffcultly.Visibility = System.Windows.Visibility.Visible;
            }

            else 
            {
                spDiffcultly.Visibility = System.Windows.Visibility.Hidden;
                spDiffcultly.IsEnabled = false;
            }
            //Reset();
            //gridBG.Visibility = Visibility.Hidden;
        }
        private void about_Click(object sender, RoutedEventArgs e)
        {
            new Window1().ShowDialog();
        }
        private void howTo_Click(object sender, RoutedEventArgs e)
        {
            new Window2().ShowDialog();
        }        
 
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (isStarted)
            //{
            //    MessageBox.Show("不准中離!!");
            //    e.Cancel = true;
            //}
        }
        private void gridBG_MouseDown(object sender, MouseButtonEventArgs e)
        {
            stackMenu.IsEnabled =true;
            spDiffcultly.IsEnabled = false;
            spDiffcultly.Visibility = System.Windows.Visibility.Hidden;
        }

        #region 難度選擇
        private void btnEasy_Click(object sender, RoutedEventArgs e)
        {
            Reset(1);
            gridBG.Visibility = System.Windows.Visibility.Hidden;
            Label d = new Label();
            d.Content = "簡　單";
            d.FontSize = 18;
            d.FontWeight = FontWeights.Bold;
            d.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            d.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            d.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            d.Margin = new Thickness(0, 0, 0, 0);
            gridTopBar.Children.Add(d);

        }

        private void btnNormal_Click(object sender, RoutedEventArgs e)
        {
            Reset(2);
            gridBG.Visibility = System.Windows.Visibility.Hidden;
            Label d = new Label();
            d.Content = "中　級";
            d.FontSize = 18;
            d.FontWeight = FontWeights.Bold;
            d.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            d.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            d.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            d.Margin = new Thickness(0, 0, 0, 0);
            gridTopBar.Children.Add(d);

        }

        private void btnHard_Click(object sender, RoutedEventArgs e)
        {
            Reset(3);
            gridBG.Visibility = System.Windows.Visibility.Hidden;
            Label d = new Label();
            d.Content = "困　難";
            d.FontSize = 18;
            d.FontWeight = FontWeights.Bold;
            d.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            d.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            d.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            d.Margin = new Thickness(0, 0, 0, 0);
            gridTopBar.Children.Add(d);
        }

        private void btnSuperHard_Click(object sender, RoutedEventArgs e)
        {
            Reset(4);
            gridBG.Visibility = System.Windows.Visibility.Hidden;

            Label d = new Label();
            d.Content = "絕望級";
            d.FontSize = 18;
            d.FontWeight = FontWeights.Bold;
            d.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            d.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            d.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            d.Margin = new Thickness(0, 0, 0, 0);
            gridTopBar.Children.Add(d);
        }
        #endregion


    }
}
