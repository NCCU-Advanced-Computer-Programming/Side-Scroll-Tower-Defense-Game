﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Side_scrolling_Tower_Defense
{
    class Saber : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Saber(bool isEnemy, int overPower)
            : base(100*overPower, 10*overPower, 1, 0.5*overPower, isEnemy, 100)
        {

        }

        public new void Skill()
        {
            //Excalibur 可造成極近距離複數敵人 高度傷害 CD時間? 
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
        public Archer(bool isEnemy, int overPower)
            : base(60 * overPower, 6 * overPower, 20, 0.3 * overPower, isEnemy, 0)
        {

        }

        public new void Skill()
        {
            //Arrow Shower 造成中距離內敵人 中度傷害 並短時間阻止敵方動作 CD時間?
        }

    }

    class Caster : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Caster(bool isEnemy, int overPower)
            : base(40 * overPower, 15 * overPower, 10, 0.4 * overPower, isEnemy, 0)
        {

        }

        public new void Skill()
        {
            //對全範圍之敵方(包括塔)造成 低度傷害 隨機附加一詛咒狀態
        }

    }

    class Berserker : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Berserker(bool isEnemy, int overPower)
            : base(200 * overPower, 20 * overPower, 1, 0.1 * overPower, isEnemy, 0)
        {

        }

        public new void Skill()
        {
            //God Hands 全陣營友方暫時性無敵 持續時間?  CD時間?
        }

    }

    class Rider : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Rider(bool isEnemy, int overPower)
            : base(100 * overPower, 7 * overPower, 2, 1 * overPower, isEnemy, 0)
        {

        }

        public new void Skill()
        {
            //王之軍勢（Ionioi Hetairoi）大幅增加所有友方之能力值 並增加擊退屬性  CD時間?
        }


    }

    class Lancer : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Lancer(bool isEnemy, int overPower)
            : base(120 * overPower, 8 * overPower, 5, 0.5 * overPower, isEnemy, 0)
        {

        }

        public new void Skill()
        {
            //必滅的黃薔薇（Gae Buidhe） 回復所有我方生命值(不含塔)與異常狀態
        }
    }

    class Assassin : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Assassin(bool isEnemy, int overPower)
            : base(30 * overPower, 40 * overPower, 1, 1 * overPower, isEnemy, 0)
        {

        }

        public new void Skill()
        {
            //Shadow Stab 對敵方單體造成超高傷害 使用之後必定死亡 CD?
        }


    }


}
