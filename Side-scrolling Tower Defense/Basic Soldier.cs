using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Windows.Controls;

namespace Side_scrolling_Tower_Defense
{
    class Soldier
    {
        
        //Property
        private int _hp;
        private int _atk;
        //private int _defense;
        private int _range;
        private int _speed;
        private int _axis;
        private double _attackspeed;
        public bool isEnemy = false;
        public Label Image;

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

        public int SPEED
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public double AS
        {
            get { return _attackspeed; }
            set { _attackspeed = value; }
        }

        public int POSITION
        {
            get { return _axis; }
            set { _axis = value; }
        }


        //Constructor
        public Soldier()
        {
            //hp=? , atk=? , range = ? , speed =? 
            HP = 100;
            ATK = 1;
            RANGE = 1;
            SPEED = 1;
            AS = 1.0;
            POSITION = 0;
            Image = new Label();
        }

        public Soldier(int hp, int atk, int range, int speed, bool enemy)
        {
            HP = hp;
            ATK = atk;
            RANGE = range;
            SPEED = speed;
            isEnemy = enemy;
            AS = 1.0;
            Image = new Label();

            if (isEnemy)
                POSITION = 1000;
            else
                POSITION = 0;

        }

        //Method

        public Label Show() 
        {
            Image.Margin = new System.Windows.Thickness(800,85,0,0);
            Image.Height = 50;
            Image.Width = 50;
            Image.Background = System.Windows.Media.Brushes.Gold;

            return Image;
        }

        //單體攻擊敵方士兵_1
        public void Attack(Soldier Enemy)
        {
            if (Enemy.isEnemy && (Enemy.POSITION - this.POSITION) <= this.RANGE)
                Enemy.HP -= ATK;
        }

        //單體攻擊敵方士兵_2
        public void Attack(Soldier[] Enemy)
        {
            int target = Int32.MaxValue;
            int nearest = Int32.MaxValue;
            for (int i = 0; i < Enemy.Length; i++)
            {
                int distance = Enemy[i].POSITION - this.POSITION;
                if (nearest > distance && distance >= 0)
                {
                    nearest = distance;
                    target = i;
                }
            }

            if (nearest <= this.RANGE)
                Enemy[target].HP -= this.ATK;
        }

        //攻擊敵方塔
        public void Attack(Tower Enemy)
        {
            if (1000 - this.POSITION <= this.RANGE)
                Enemy.GetHurt(ATK);
        }


        public void Move()
        {
            if (isEnemy)
                POSITION -= SPEED;
            else
                POSITION += SPEED;
        }

        //Die()
        public Soldier LifeCheck()
        {
            //C#使用記憶體自動回收
            if (HP <= 0)
                return null;
            else
                return this;
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
