using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Controls;

namespace Side_scrolling_Tower_Defense
{
    class Soldier
    {
        
        //Property
        private int _hp;
        private int _atk;
        private int _range;
        private double _speed;
        private double _axis;
        private int _attackspeed;
        private int _price;
        public bool isEnemy = false;
        public Label Image;

        private int counter = 0;//控制是否攻擊，每呼叫一次counter++  counter % AS==0 就攻擊

        #region get & set
        public int HP
        {
            get { return _hp; }
            set { _hp = value; }
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
            ATK = 1;
            RANGE = 1;
            SPEED = 1;
            APS = 100;
            POSITION = 0;
            PRICE = 0;
            Image = new Label();
        }

        public Soldier(int hp, int atk, int range, double speed, bool enemy, int price)
        {
            HP = hp;
            ATK = atk;
            RANGE = range;
            SPEED = speed;
            isEnemy = enemy;
            APS = 100;
            PRICE = price;
            Image = new Label();

            if (isEnemy)
                POSITION = 1000;
            else
                POSITION = 0;
        }

        //Method

        /*--暫且用Label代替圖片，本區塊設定label樣式--*/
        public Label Show(int height, int width, System.Windows.Media.SolidColorBrush Color)
        {
            if(isEnemy)
                Image.Margin = new System.Windows.Thickness(0,0,800,15); //AI士兵出生位置
            else
                Image.Margin = new System.Windows.Thickness(800,0,0,15); //Player士兵出生位置
            Image.Height = height;
            Image.Width = width;
            Image.Background = Color;
            Image.VerticalAlignment = VerticalAlignment.Bottom;
 //           Image.Opacity = 0.7;
            Image.BorderBrush = System.Windows.Media.Brushes.Black;
            Image.BorderThickness = new Thickness(1, 1, 1, 1);

            return Image;
        }

        //單體攻擊敵方士兵_1
        public bool Attack(Soldier Enemy)
        {
            //兩個兵陣營不相同 && 兩兵間距離小於攻擊範圍
            if ((Enemy.isEnemy!=this.isEnemy) && Math.Abs(Enemy.POSITION - this.POSITION) <= this.RANGE)
            {
                Enemy.HP -= ATK;
                Enemy.LifeCheck();
                return true;
            }
            return false;
        }

        //單體攻擊敵方士兵_2
        public bool Attack(List<Soldier> Enemy)
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
                    Enemy[target].HP -= this.ATK;
                    Enemy[target].LifeCheck();
                }
                return true;
            }
            return false;
        }
 
        //攻擊敵方塔
        public bool Attack(Tower Enemy)
        {
            if (Math.Abs(Enemy.POSITION - this.POSITION) <= this.RANGE)
            {
                Enemy.GetHurt(ATK);
                return true;
            }
            return false;
        }

        public void Move(List<Soldier> enemyS, Tower enemyTower)
        {
            if (!Attack(enemyS) && !Attack(enemyTower))
            {
                if (isEnemy)
                {
                    Image.Margin = new Thickness(Image.Margin.Left + SPEED, Image.Margin.Top, Image.Margin.Right - SPEED, Image.Margin.Bottom);
                    POSITION = Image.Margin.Right + Image.Width / 2; //POSITION = 方塊中點位置(右邊緣 + 寬度的一半)
                   // Image.Content = POSITION.ToString();
                }
                else
                {
                    Image.Margin = new Thickness(Image.Margin.Left - SPEED, Image.Margin.Top, Image.Margin.Right + SPEED, Image.Margin.Bottom);
                    POSITION = Image.Margin.Right + Image.Width / 2; //POSITION = 方塊中點位置(右邊緣 + 寬度的一半)
              //      Image.Content = POSITION.ToString();
                }
            }
            Image.Content = HP.ToString();//顯示血量
            //Image.Content = POSITION + " \n" + HP;
        }

        //Die()
        public Soldier LifeCheck()
        {
            //C#使用記憶體自動回收
            if (HP <= 0)
            {
               //player
                Image.Visibility = Visibility.Hidden;
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
