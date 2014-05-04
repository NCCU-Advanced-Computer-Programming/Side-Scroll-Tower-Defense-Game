﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Side_scrolling_Tower_Defense
{
    class AI
    {
        private Tower aiTower= new Tower(100,10,10,1);
//		private int _money;
//		private int _towerUpgradePrice;

        //Label[] soldier = new Label[1000]; 
 //       public Soldier[] soldier = new Soldier[1000];   //場上最多只能有1000士兵
        public List<Soldier> soldier = new List<Soldier>();   

        int _soliderOnField = 0;                 //場上的我方士兵數(?) <--怪怪


        public AI()
        {
           aiTower = new Tower(100, 10, 10, 1);
        }
        public void GenerateSolider(Panel grid1){
            // new Soldier?
            soldier.Add( new Soldier(100,10,50,0.3,true));
            grid1.Children.Add(soldier[_soliderOnField].Show());
            _soliderOnField++;
        }

		public void UpgradeTower(Label lb){
            //玩家花錢升級塔，塔等級提升，下級所需金費增加
 //           _money -= _towerUpgradePrice;
            //myTower.TowerLevel++;

            //所有數值暫時都*2
 //           _towerUpgradePrice = _towerUpgradePrice * 2;
           aiTower.Upgrade('t',aiTower.TowerLevel);
           aiTower.Upgrade('h',aiTower.HP +1);
           aiTower.Upgrade('a',aiTower.ATK + 2);
           aiTower.Upgrade('r',aiTower.RANGE+ 3);
            
            //顯示
            lb.Content = "level:" +aiTower.TowerLevel.ToString() + '\n' + "hp:" +aiTower.HP.ToString() + '\n' + "range:" +aiTower.RANGE.ToString() + '\n' + "atk:" +aiTower.ATK.ToString();
        }
        public void MaintainSolidersPosition(List<Soldier> enemyS)
        {
            for (int i = 0; i < _soliderOnField; i++)
            {
                // 每個士兵該往前的往前，該打的打
                soldier[i].Move(enemyS);
                if (soldier[i].HP <= 0)
                {
                    soldier.RemoveAt(i);
                    _soliderOnField--;
                }
            }
        }
    }
}
