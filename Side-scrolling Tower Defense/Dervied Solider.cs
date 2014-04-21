using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Side_scrolling_Tower_Defense
{
    class Saber : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Saber(bool isEnemy)
            : base(100, 10, 1, 5, isEnemy)
        {

        }

        public void Skill()
        {

        }

/*        private void Nerf()
        {

        }

        private void Buff()
        {

        }*/
    }

    class Archer : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Archer(bool isEnemy)
            : base(60, 6, 20, 3, isEnemy)
        {

        }

        public void Skill()
        {

        }

    }

    class Caster : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Caster(bool isEnemy)
            : base(40, 15, 10, 4, isEnemy)
        {

        }

        public void Skill()
        {

        }

    }

    class Berserker : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Berserker(bool isEnemy)
            : base(200, 20, 1, 1, isEnemy)
        {

        }

        public void Skill()
        {

        }

    }

    class Rider : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Rider(bool isEnemy)
            : base(100, 7, 2, 10, isEnemy)
        {

        }

        public void Skill()
        {

        }


    }

    class Lancer : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Lancer(bool isEnemy)
            : base(120, 8, 5, 5, isEnemy)
        {

        }

        public void Skill()
        {

        }


    }

    class Assassin : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Assassin(bool isEnemy)
            : base(30, 40, 1, 10, isEnemy)
        {

        }

        public void Skill()
        {

        }


    }


}
