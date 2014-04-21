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
        public bool _enemy = false;

        public int HP { get; set; }

        public int ATK { get; set; }

        public int RANGE { get; set; }

        public int SPEED { get; set; }

        public int POSITION { get; set; }

        public bool isEnemy 
        {
            get { return _enemy; } 
        }


        //Constructor
        public Soldier()
        {
            HP = 100;
            ATK = 1;
            RANGE = 1;
            SPEED = 1;
            POSITION = 0;
        }

        /*public Soldier(int hp, int atk)
        {

        }

        public Soldier(int hp, int atk, int range)
        {

        }

        public Soldier(int hp, int atk, int range, int speed)
        {

        }*/

        public Soldier(int hp, int atk, int range, int speed, bool enemy)
        {
            HP = hp;
            ATK = atk;
            RANGE = range;
            SPEED = speed;
            _enemy = enemy;

            POSITION = 0;
        }

        //Method
        public void Attack(Soldier Enemy)
        {
            if(isEnemy)
                Enemy.HP -= ATK;
        }

        public void Move()
        {
            POSITION += SPEED;
        }

        public Soldier Die()
        {
            //C#使用記憶體自動回收
            if (HP <= 0)
                return null;
            else
                return this;
        }

        virtual private void Levelup();
        virtual private void Buff();
        virtual private void Nerf();
        virtual public void Skill();
    }
}
