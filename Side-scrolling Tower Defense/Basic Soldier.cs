﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

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
        public GifImage Image;
        private string imgSourceMove;
        private string imgSourceAttack;
        private bool isAttack = false;

        protected int counter = 0;//控制是否攻擊，每呼叫一次counter++  counter % AS==0 就攻擊

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
            Image = new GifImage();
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
            Image = new GifImage();

            if (isEnemy)
                POSITION = 1000;
            else
                POSITION = 0;
        }

        //Method

        /*--暫且用Label代替圖片，本區塊設定label樣式--*/
        public GifImage Show(int height, int width, string imageSource)
        {
            if(isEnemy)
                Image.Margin = new System.Windows.Thickness(0,0,958-width,10); //AI士兵出生位置
            else
                Image.Margin = new System.Windows.Thickness(0,0,36,10); //Player士兵出生位置
            Image.Height = height;
            Image.Width = width;

            imgSourceMove = Directory.GetCurrentDirectory();
            imgSourceMove = imgSourceMove.Replace("\\", "/");
            imgSourceMove = imgSourceMove + imageSource;
            //MessageBox.Show(imageAbsolutePath);
            var _image = new BitmapImage();
            _image.BeginInit();
            _image.UriSource = new Uri(imgSourceMove, UriKind.Absolute);
            _image.EndInit();
            ImageBehavior.SetAnimatedSource(Image, _image);

            Image.VerticalAlignment = VerticalAlignment.Bottom;
            Image.HorizontalAlignment = HorizontalAlignment.Right;
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
                    Enemy[target].HP -= this.ATK;
                    Enemy[target].Image.ToolTip = Enemy[target].HP.ToString();
                    Enemy[target].LifeCheck();

                }
                
                if (!isAttack)
                {
                    if (imgSourceAttack == null)
                        imgSourceAttack = imgSourceMove.Replace("2", "");
                    //MessageBox.Show(imgSourceAttack);

                    var _image = new BitmapImage();
                    _image.BeginInit();
                    _image.UriSource = new Uri(imgSourceAttack, UriKind.Absolute);
                    _image.EndInit();
                    ImageBehavior.SetAnimatedSource(Image, _image);
                }
                isAttack = true;
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
                return true;
            }
            return false;
        }

        public void Move(List<Soldier> enemyS, Tower enemyTower)
        {
            if (!Attack(enemyS) && !Attack(enemyTower))
            {
                if (isAttack)
                {
                    var _image = new BitmapImage();
                    _image.BeginInit();
                    _image.UriSource = new Uri(imgSourceMove, UriKind.Absolute);
                    _image.EndInit();
                    ImageBehavior.SetAnimatedSource(Image, _image);
                }
                
                if (isEnemy)
                {
                    Image.Margin = new Thickness(Image.Margin.Left + SPEED, Image.Margin.Top, Image.Margin.Right - SPEED, Image.Margin.Bottom);
                    POSITION = Image.Margin.Right;
                   //Image.Content = POSITION.ToString();
                }
                else
                {
                    Image.Margin = new Thickness(Image.Margin.Left - SPEED, Image.Margin.Top, Image.Margin.Right + SPEED, Image.Margin.Bottom);
                    POSITION = Image.Margin.Right + Image.Width ; //POSITION = 方塊中點位置(右邊緣 + 寬度的一半)
                  //  Image.Content = POSITION.ToString();
                }
                isAttack = false;
            }
            Image.ToolTip = HP.ToString();
            // Image.Content = HP.ToString();//顯示血量
            //Image.Content = POSITION + " \n" + HP;
        }

        //Die()
        public virtual Soldier LifeCheck()
        {
            //C#使用記憶體自動回收
            if (HP <= 0)
            {
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
