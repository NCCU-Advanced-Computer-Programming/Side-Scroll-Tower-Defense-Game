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
        Label[] soldier = new Label[1000];
        int _soliderOnField = 0;    //場上的我方士兵數

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
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
            /*   for (int i = 0; i < _soliderOnField; i++)
            {
                soldier[i].Margin=new Thickness(800-i*10,85,0,0);// = new Thickness(800-soldier[i]);
               // soldier[i].Margin -= 10;
            }
            
            */
        }

        private void btnSoldier1_Click(object sender, RoutedEventArgs e)
        {
            soldier[_soliderOnField] = new Label();
            grid1.Children.Add(soldier[_soliderOnField]);
            soldier[_soliderOnField].Margin = new Thickness(800, 85, 0, 0);
            soldier[_soliderOnField].Height = 50;
            soldier[_soliderOnField].Width = 50;
            soldier[_soliderOnField].Background = System.Windows.Media.Brushes.Green;
            _soliderOnField++;
        }
    }
}
