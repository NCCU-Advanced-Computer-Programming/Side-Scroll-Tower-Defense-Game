using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool isEnemy = false;

        public int HP { get; set; }

        public int ATK { get; set; }

        public int RANGE { get; set; }

        public int SPEED { get; set; }

        public int POSITION { get; set; }

        //Constructor
        public Soldier()
        {
            //hp=? , atk=? , range = ? , speed =? 
            HP = 100;
            ATK = 1;
            RANGE = 1;
            SPEED = 1;
            POSITION = 0;
        }

        public Soldier(int hp, int atk, int range, int speed, bool enemy)
        {
            HP = hp;
            ATK = atk;
            RANGE = range;
            SPEED = speed;
            isEnemy = enemy;

            if (isEnemy)
                POSITION = 1000;
            else
                POSITION = 0;

        }

        //Method

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
        public virtual void Levelup()
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
