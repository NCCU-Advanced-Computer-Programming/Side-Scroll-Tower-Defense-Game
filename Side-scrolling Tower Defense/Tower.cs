using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace Side_scrolling_Tower_Defense
{
    class Tower
    {
        /*attr*/
        private int maxHP; // 定值，不會因受攻而減少
        private int hp;
        private int atk;
        private int range;
        private int towerLevel;
        private double _axis;
        private int _attackspeed;
        private int counter=0;
        private bool isCrash = false;
        private bool isEnemy = false;
        ToolTip tp = new ToolTip();
        Label lbTowerHP;//在畫面上顯示的血量
        Label lbHP_BG;
        GifImage ImgTower;  //在畫面上顯示的塔
        Label bullet;
        Grid grid;//主畫面(顯示子彈用
        double movePerStepX = 25; //單位時間移動的 X 軸值, 定值
        double movePerStepY = 0; //即時計算每單位時間移動的 Y 軸值
        int startTime = 0; //每發子彈的開槍時機
        double angle = 0;//子彈角度
        GifImage beam; //Tower Skill Anime
        private string gifSource = Directory.GetCurrentDirectory().Replace("\\", "/")+"/Images/Skill4_Anime.gif"; 


        
        
        #region get & set
        public double POSITION
        {
            get { return _axis; }
            set { _axis = value; }
        }
        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }
        public int MAXHP
        {
            get { return maxHP; }
            set { maxHP = value; }
        }
        public int ATK
        {
            get { return atk; }
            set { atk = value; }
        }
        public int RANGE
        {
            get { return range; }
            set { range = value; }
        }
        public int TowerLevel
        {
            get { return towerLevel; }
            set { towerLevel = value; }
        }
        public bool CRASHED
        {
            get { return isCrash; }
            set { isCrash = value; }
        }
        #endregion

        /*method*/
        public Tower(int _hp, int _atk, int _range, int _towerLevel, bool _isPlayer, Grid _grid, Grid _gridTopBar)
        {
            maxHP = _hp;
            hp = _hp;
            atk = _atk;
            range = _range;
            towerLevel = _towerLevel;
            _attackspeed = 100;//單位是10毫秒
            isEnemy = !_isPlayer;
            grid = _grid;
            LabelSetting();
            grid.Children.Add(ImgTower);
            _gridTopBar.Children.Add(lbHP_BG);
            _gridTopBar.Children.Add(lbTowerHP);

            if (isEnemy)
                _axis = 886;//lbTower.Margin.Right;
            else
                _axis = 108;//lbTower.Margin.Right + lbTower.Width;
            tp.Content = "LV:" + TowerLevel.ToString() + '\n' + "HP:" + HP.ToString() + '\n' + "Range:" + RANGE.ToString() + '\n' + "Damage:" + ATK.ToString();
            tp.Background = Brushes.LightSteelBlue;
            tp.BorderBrush = Brushes.Black;
            tp.BorderThickness = new Thickness(2);
            ImgTower.ToolTip = tp;
        }
        public void Attack(List<Soldier> enemyS)
        {
            int target = Int32.MaxValue;
            double nearest = double.MaxValue;
            for (int i = 0; i < enemyS.Count; i++)
            {
                double distance = Math.Abs(enemyS[i].POSITION - this.POSITION);
                if (nearest > distance )
                {
                    nearest = distance;
                    target = i;
                }
            }

            if (nearest <= range)
            {
                if (startTime == 0)
                {
                    if (nearest / movePerStepX > 2)
                    {
                        movePerStepX = 25;
                        startTime = _attackspeed - (int)((nearest + this.ImgTower.Width) / movePerStepX);
                        movePerStepY = (ImgTower.Height - (enemyS[target].Image.Height / 2)) / (_attackspeed - startTime);

                    }
                    else //太近了
                    {
                        movePerStepX = (nearest+this.ImgTower.Width) / 5 ;
                        startTime = _attackspeed - 10;
                        movePerStepY = (ImgTower.Height - (enemyS[target].Image.Height / 2)) / (_attackspeed - startTime);

                    }
                }

                bool isShooted = false; //如果已開槍，則在未打到目標前 isShooted == true
                if (bullet == null && counter == startTime)
                {
                    grid.Children.Add(BulletShow()); //把子彈放進Grid
                    angle = Math.Atan2((ImgTower.Height - enemyS[target].Image.Height / 2), nearest + 40) * 57; //設定角度，弧度角轉角度
                    RotateTransform transform = new RotateTransform(angle); 
                    if (!isEnemy)
                        transform = new RotateTransform(-angle);
                    else
                        transform = new RotateTransform(angle);
                    bullet.RenderTransform = transform;
                }
                if (counter > startTime)
                    isShooted = true;
                
                if ((++counter % _attackspeed) == 0) //控制攻速
                {
                    counter = 0;
                    enemyS[target].GetHurt(this.ATK); 
                    grid.Children.Remove(bullet);
                    bullet = null;
                    startTime = 0;
                }
                else
                {
                    if (bullet != null && isShooted)
                    {
                        if (isEnemy)
                        {
                            bullet.Margin = new System.Windows.Thickness(0, 0, bullet.Margin.Right - movePerStepX, bullet.Margin.Bottom - movePerStepY);
                            if (bullet.Margin.Right + movePerStepX < enemyS[target].POSITION)
                            {
                                grid.Children.Remove(bullet);
                                bullet = null;
                                counter = _attackspeed - 1; //摸到就直接等同攻擊到
                            }
                        }
                        else
                        {
                            bullet.Margin = new System.Windows.Thickness(0, 0, bullet.Margin.Right + movePerStepX, bullet.Margin.Bottom - movePerStepY);
                            if (bullet.Margin.Right - movePerStepX > enemyS[target].POSITION)
                            {
                                grid.Children.Remove(bullet);
                                bullet = null;
                                counter = _attackspeed - 1;
                            }
                        }
                    }
                }
            }
            else
            {
                grid.Children.Remove(bullet); 
                bullet = null;
                startTime = 0;
            }
        }
        public void GetHurt(int quaintity)
        {
            hp -= quaintity;
            int remainHP_Width =(int)(200 * ((double)hp / (double)maxHP));
            if (remainHP_Width < 0)
                remainHP_Width = 0;
            lbTowerHP.Width = remainHP_Width;
            tp.Content = "LV:" + TowerLevel.ToString() + '\n' + "HP:" + HP.ToString() + '\n' + "Range:" + RANGE.ToString() + '\n' + "Damage:" + ATK.ToString();
            ImgTower.ToolTip = tp;
            if (hp <= 0)
                Crash();
        }
        public void Crash()
        {
            CRASHED = true;

            // display the animation of crashing
            // call game.Game_over();
        }
        public void Upgrade(char item, int quantity)
        {
            if (item == 'h')
            {
                maxHP = quantity;
                hp = quantity;
                int remainHP_Width = (int)(200 * ((double)hp / (double)maxHP));
                if (remainHP_Width < 0)
                    remainHP_Width = 0;
                lbTowerHP.Width = remainHP_Width;

            }
            if (item == 'a')
                atk = quantity;
            if (item == 'r')
                range = quantity;
            if (item == 't')
                towerLevel++;

            tp.Content = "LV:" + TowerLevel.ToString() + '\n' + "HP:" + HP.ToString() + '\n' + "Range:" + RANGE.ToString() + '\n' + "Damage:" + ATK.ToString();
            ImgTower.ToolTip = tp;
        }

        /*範圍技大招 傳入對方士兵陣列*/
        public void Skill(List<Soldier> EnemyS)
        {
            SkillAnimate();
            if (!this.isEnemy)/*玩家塔*/
            {
                for (int i = 0; i < EnemyS.Count; i++)
                    EnemyS[i].GetHurt(1000);
            }
        }
        private void SkillAnimate()
        {
            if (beam != null)
                grid.Children.Remove(beam);

            beam = new GifImage();
            beam.HorizontalAlignment = HorizontalAlignment.Right;
            beam.VerticalAlignment = VerticalAlignment.Bottom;
            beam.Width = 780;
            beam.Margin = new Thickness(0, 0, 120, 10);
            var _image = new BitmapImage();
            _image.BeginInit();
            _image.UriSource = new Uri(gifSource, UriKind.Absolute);
            _image.EndInit();
            ImageBehavior.SetAnimatedSource(beam, _image);
            ImageBehavior.SetRepeatBehavior(beam, new RepeatBehavior(1));
            grid.Children.Add(beam);
        }

        
        private Label BulletShow()
        {
            bullet = new Label();
            bullet.Width = 40;
            bullet.Height = 3;
            if(isEnemy)
                bullet.Margin = new System.Windows.Thickness(0, 0, 923, ImgTower.Height-20);
            else
                bullet.Margin = new System.Windows.Thickness(0, 0, 36, ImgTower.Height -20);

            bullet.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            bullet.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            bullet.Background = System.Windows.Media.Brushes.Purple ;

            return bullet;
        }
        public void LabelSetting()
        {
            ImgTower = new GifImage();
            lbTowerHP = new Label();
            lbHP_BG = new Label();

            ImgTower.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            ImgTower.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            lbTowerHP.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            lbTowerHP.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbHP_BG.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            lbHP_BG.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            string imgSource = Directory.GetCurrentDirectory();
            imgSource = imgSource.Replace("\\", "/");
            imgSource+="/Images/tower_test2.gif";
            if (isEnemy)
            {

                ImgTower.Margin = new System.Windows.Thickness(0, 0, 886, 0);
                //ImgTower.Content = new Image
                //{
                //    Source = new BitmapImage(new Uri("Images/tower_test2.gif", UriKind.Relative)),
                //    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                //    Stretch = Stretch.Fill
                //};
             //   string imgSource = Directory.GetCurrentDirectory().Replace("\\", "/") + "Images/tower_test2.gif";
                var _image = new BitmapImage();
                _image.BeginInit();
                _image.UriSource = new Uri(imgSource, UriKind.Absolute);
                _image.EndInit();
                ImageBehavior.SetAnimatedSource(ImgTower, _image);


               lbHP_BG.Margin = new System.Windows.Thickness(35, 0, 0, 10);
                lbTowerHP.Margin = new System.Windows.Thickness(35, 0, 0, 10);
            }
            else
            {
                ImgTower.Margin = new System.Windows.Thickness(0, 0, 36, 0);
                //imgSourceMove = Directory.GetCurrentDirectory();
                //imgSourceMove = imgSourceMove.Replace("\\", "/");
                //imgSourceMove = imgSourceMove + imageSource;
                //MessageBox.Show(imageAbsolutePath);
              //  string imgSource = Directory.GetCurrentDirectory().Replace("\\", "/") + "Images/tower_test2.gif";
                var _image = new BitmapImage();
                _image.BeginInit();
                _image.UriSource = new Uri(imgSource, UriKind.Absolute);
                _image.EndInit();
                ImageBehavior.SetAnimatedSource(ImgTower, _image);
             //   ImgTower.Height = height;
                //ImgTower.Content = new Image
                //{
                //    Source = new BitmapImage(new Uri("Images/tower_test2.gif", UriKind.Relative)),
                //    VerticalAlignment = System.Windows.VerticalAlignment.Center, Stretch = Stretch.Fill
                //};

                lbHP_BG.Margin = new System.Windows.Thickness(757, 0, 0, 10);
                lbTowerHP.Margin = new System.Windows.Thickness(757, 0, 0, 10);
            }
            ImgTower.Height = 180;
            ImgTower.Width = 120;
            //ImgTower.FontSize = 10;
            lbTowerHP.Width = 200;
            lbTowerHP.Height = 25;
            lbTowerHP.Background = Brushes.Red;
            lbTowerHP.BorderBrush = Brushes.Black;
            lbTowerHP.BorderThickness = new System.Windows.Thickness(2, 2, 2, 2);  
            //ImgTower.BorderBrush = System.Windows.Media.Brushes.Black;

            lbHP_BG.Width = 200;
            lbHP_BG.Height = 25;
            lbHP_BG.Background = Brushes.Black;
        }
    }
}
