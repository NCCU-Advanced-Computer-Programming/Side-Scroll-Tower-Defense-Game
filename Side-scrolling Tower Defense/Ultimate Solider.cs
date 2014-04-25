using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Side_scrolling_Tower_Defense
{
    class HERO : Soldier
    {
        private int _experience;
        private int _level;

        private int EXP 
        {
            get { return _experience; }
            set { _experience = value; }
        }

        public int LEVEL
        {
            get { return _level; }
            set { _level = value; }
        }

        //hp=? , atk=? , range = ? , speed =? 
        public HERO()
            : base(500,15,1,1,false)
        {
            //依玩家選取英雄類型，調整屬性數值
        }

        public new void Attack(Soldier Enemy)
        {

            if (Enemy.isEnemy && (Enemy.POSITION - this.POSITION) <= this.RANGE)
                Enemy.HP -= this.ATK;

            if (Enemy.HP <= 0)
            {
                if (Enemy.GetType() == typeof(Saber)) { EXP += 1; }
                else if (Enemy.GetType() == typeof(Saber)) { EXP += 1; }
                else if (Enemy.GetType() == typeof(Archer)) { EXP += 1; }
                else if (Enemy.GetType() == typeof(Caster)) { EXP += 1; }
                else if (Enemy.GetType() == typeof(Berserker)) { EXP += 1; }
                else if (Enemy.GetType() == typeof(Rider)) { EXP += 1; }
                else if (Enemy.GetType() == typeof(Lancer)) { EXP += 1; }
                else if (Enemy.GetType() == typeof(Assassin)) { EXP += 1; }
                else if (Enemy.GetType() == typeof(HERO)) { EXP += 1; }
                else if (Enemy.GetType() == typeof(BOSS)) { EXP += 1; }
            }
        }

        private void Levelup()
        {
            //每級所需經驗值公式
        }

        private new void Skill()
        {

        }

    }

    class BOSS : Soldier
    {

        //hp=? , atk=? , range = ? , speed =? 
        public BOSS()
            : base(500, 15, 1, 1, true)
        {
            //依關卡難度改變
        }

        private new void Skill()
        {

        }
    }
}
