using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Side_scrolling_Tower_Defense
{
    class Soldier
    {
        
        //Property
        private int _hp;
        private int _maxHp;
        private double _hpWidth;
        private int _atk;
        private int _range;
        private double _speed;
        private double _axis;
        private int _attackspeed;
        private int _price;
        public bool isEnemy = false;
        public StackPanel spImg;
        public GifImage Image;
        public Label hp;
        protected string imgSourceMove;   //移動的gif完整路徑
        protected string imgSourceAttack; //攻擊的gif完整路徑
        protected bool isAttack = false;　//判斷是否需要換gif圖

        protected int counter = 0;//控制是否攻擊，每呼叫一次counter++  counter % AS==0 就攻擊

        #region get & set
        public int HP
        {
            get { return _hp; }
            set { _hp = value; }
        }
        public int MAX_HP
        {
            get { return _maxHp; }
            set { _maxHp = value; }
        }
        public int ATK
        {
            get { return _atk; }
            set { _atk = value; }
        }
        public int RANGE
        {
            get { return _range; }
            set { _range = value; }
        }
        public double SPEED
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public int APS // attack per second
        {
            get { return _attackspeed; }
            set { _attackspeed = value; }
        }
        public double POSITION
        {
            get { return _axis; }
            set { _axis = value; }
        }
        public int PRICE
        {
            get { return _price; }
            set { _price = value; }
        }
        #endregion

        //Constructor
        public Soldier()
        {
            //hp=? , atk=? , range = ? , speed =? 
            HP = 100;
            MAX_HP = 100;
            ATK = 1;
            RANGE = 1;
            SPEED = 1;
            APS = 100;
            POSITION = 0;
            PRICE = 0;
            Image = new GifImage();
            spImg = new StackPanel();
        }

        public Soldier(int hp, int atk, int range, double speed, bool enemy, int price)
        {
            HP = hp;
            MAX_HP = hp;
            ATK = atk;
            RANGE = range;
            SPEED = speed;
            isEnemy = enemy;
            APS = 100;
            PRICE = price;
            Image = new GifImage();
            spImg = new StackPanel();

            if (isEnemy)
                POSITION = 1000;
            else
                POSITION = 0;
       }

        public StackPanel Show(int height, int width, string imageSource)
        {
            if(isEnemy)
                spImg.Margin = new System.Windows.Thickness(0,0,958-width,10); //AI士兵出生位置
            else
                spImg.Margin = new System.Windows.Thickness(0,0,36,10); //Player士兵出生位置
            spImg.Width = width;
            spImg.VerticalAlignment = VerticalAlignment.Bottom;
            spImg.HorizontalAlignment = HorizontalAlignment.Right;
           // spImg.Background = System.Windows.Media.Brushes.Black;

            imgSourceMove = Directory.GetCurrentDirectory();
            imgSourceMove = imgSourceMove.Replace("\\", "/");
            imgSourceMove = imgSourceMove + imageSource;
            //MessageBox.Show(imageAbsolutePath);
            var _image = new BitmapImage();
            _image.BeginInit();
            _image.UriSource = new Uri(imgSourceMove, UriKind.Absolute);
            _image.EndInit();
            ImageBehavior.SetAnimatedSource(Image, _image);
            Image.Height = height;
            Image.VerticalAlignment = VerticalAlignment.Bottom;

            _hpWidth = width;
            hp = new Label();
            hp.Width = _hpWidth;
            hp.Height = 5;
            hp.Background = System.Windows.Media.Brushes.Red;
            hp.BorderThickness = new Thickness(1);
            hp.BorderBrush = System.Windows.Media.Brushes.Black;

            spImg.Children.Add(hp);
            spImg.Children.Add(Image);

            return spImg;
        }

        //單體攻擊敵方士兵_1
        public bool Attack(Soldier Enemy)
        {
            //兩個兵陣營不相同 && 兩兵間距離小於攻擊範圍
            if ((Enemy.isEnemy!=this.isEnemy) && Math.Abs(Enemy.POSITION - this.POSITION) <= this.RANGE)
            {
                Enemy.GetHurt(ATK);
                if (!isAttack) //判斷是否需要換gif圖
                {
                    if (imgSourceAttack == null)
                        imgSourceAttack = imgSourceMove.Replace("Move", "Attack");
                    //MessageBox.Show(imgSourceAttack);

                    var _image = new BitmapImage();
                    _image.BeginInit();
                    _image.UriSource = new Uri(imgSourceAttack, UriKind.Absolute);
                    _image.EndInit();
                    ImageBehavior.SetAnimatedSource(Image, _image);
                    isAttack = true;
                }
                return true;
            }
            return false;
        }

        //單體攻擊敵方士兵_2
        public virtual bool Attack(List<Soldier> Enemy)
        {
            int target = Int32.MaxValue;
            double nearest = double.MaxValue;

            for (int i = 0; i < Enemy.Count; i++)
            {
                double distance = Math.Abs(Enemy[i].POSITION - this.POSITION);
                if (nearest > distance)
                {
                    nearest = distance;
                    target = i;
                }
            }
            if (nearest <= this.RANGE )
            {
                if ((++counter % APS) == 0)
                {
                    counter = 0;
                    Enemy[target].GetHurt(ATK);
                }
                
                if (!isAttack)
                {
                    if (imgSourceAttack == null)
                        imgSourceAttack = imgSourceMove.Replace("Move", "Attack");
                    //MessageBox.Show(imgSourceAttack);

                    var _image = new BitmapImage();
                    _image.BeginInit();
                    _image.UriSource = new Uri(imgSourceAttack, UriKind.Absolute);
                    _image.EndInit();
                    ImageBehavior.SetAnimatedSource(Image, _image);
                    //Image.Height = Image.ActualHeight;

                    Image.Width = Image.ActualWidth + 15;
                    spImg.Width = Image.Width;
                   // Image.Height = Image.ActualHeight;
                    isAttack = true;
                }
                return true;
            }
            return false;
        }
 
        //攻擊敵方塔
        public virtual bool Attack(Tower Enemy)
        {
            if (Math.Abs(Enemy.POSITION - this.POSITION) <= this.RANGE)
            {
                if ((++counter % APS) == 0)
                {
                    Enemy.GetHurt(ATK);
                    counter = 0;
                }
                if (!isAttack) //判斷是否需要換gif圖
                {
                    if (imgSourceAttack == null)
                        imgSourceAttack = imgSourceMove.Replace("Move", "Attack");
                    //MessageBox.Show(imgSourceAttack);

                    var _image = new BitmapImage();
                    _image.BeginInit();
                    _image.UriSource = new Uri(imgSourceAttack, UriKind.Absolute);
                    _image.EndInit();
                    ImageBehavior.SetAnimatedSource(Image, _image);
                    Image.Width = Image.ActualWidth + 15;
                    spImg.Width = Image.Width;
                    isAttack = true;
                }

                return true;
            }
            return false;
        }

        public void Move(List<Soldier> enemyS, Tower enemyTower)
        {
            if (!Attack(enemyS) && !Attack(enemyTower))
            {
                if (isAttack) //判斷是否要換gif
                {
                    var _image = new BitmapImage();
                    _image.BeginInit();
                    _image.UriSource = new Uri(imgSourceMove, UriKind.Absolute);
                    _image.EndInit();
                    ImageBehavior.SetAnimatedSource(Image, _image);
                    Image.Width = Image.ActualWidth - 15;
                    spImg.Width = Image.Width;
                }
                
                if (isEnemy)
                {
                    spImg.Margin = new Thickness(spImg.Margin.Left + SPEED, spImg.Margin.Top, spImg.Margin.Right - SPEED, spImg.Margin.Bottom);
                    POSITION = spImg.Margin.Right;
                }
                else
                {
                    spImg.Margin = new Thickness(spImg.Margin.Left - SPEED, spImg.Margin.Top, spImg.Margin.Right + SPEED, spImg.Margin.Bottom);
                    POSITION = spImg.Margin.Right + spImg.Width ; 
                }
                isAttack = false;
            }
        }

        public void GetHurt(int q)
        {
            this.HP -= q;
            this.LifeCheck();
            if (_hpWidth * (double)((double)HP / (double)MAX_HP) > 0)
                this.hp.Width = _hpWidth * (double)((double)HP / (double)MAX_HP);
            else
                this.hp.Width = 0;
        }
        //Die()
        public virtual Soldier LifeCheck()
        {
            //C#使用記憶體自動回收
            if (HP <= 0)
            {
                spImg.Visibility = Visibility.Hidden;
                return null;
            }
            else
            {
                return this;
            }       
        }

        public virtual void Skill()
        {

        }
        public virtual void Buff()
        {

        }
        public virtual void Nerf()
        {

        }

    }
}
