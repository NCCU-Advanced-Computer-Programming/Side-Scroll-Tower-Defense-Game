using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower
{
    class Tower
    {
        /*attr*/
        private int hp;
        private int atk;
        private int range;
        private int towerLevel;

        /*method*/
        public Tower(int _hp, int _atk, int _range, int _towerLevel)
        {
            hp = _hp;
            atk = _atk;
            range = _range;
            towerLevel = _towerLevel;
        }
        public void Attack()
        {
            int i = 0;
            while (enemySolider[i] == null)
                i++;

        }
        public void GetHurt(int quaintity)
        {
            hp -= quaintity;
        }
        public void Crash()
        {
            // display the animation of crashing
            // call game.Game_over();
        }
        public void Upgrade(char item, int quantity)
        {
            if (item == 'h')
                hp += quantity;
            else if (item == 'a')
                atk += quantity;
            else if (item == 'r')
                range += quantity;
            else if (item == 't')
                towerLevel += quantity;
        }
        public void Skill();


    }
}
