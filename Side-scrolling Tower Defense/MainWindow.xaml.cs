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
        int _timeCount = 0;
        Player player = new Player();
 
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();

          //  reset();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            /*for each soldier 
             * if(範圍內無敵人)   MOVE
             * else    ATTACK
             */

            //以下測試
            /*
            _timeCount++;
            lbTime.Content = _timeCount.ToString();
              for (int i = 0; i < _soliderOnField; i++)
            {
                soldier[i].Margin=new Thickness(800-i*100,85,i*100,0);// = new Thickness(800-soldier[i]);
               // soldier[i].Margin -= 10;
            }*/
            
            
        }

        private void btnSoldier1_Click(object sender, RoutedEventArgs e)
        {
            player.GenerateSolider(grid1);
        }

        private void btnUpgradeTower_Click(object sender, RoutedEventArgs e)
        {
           player.UpgradeTower(lbmyTproperty);
            
        }
    }
}
