﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Side_scrolling_Tower_Defense
{
    class Player
    {
    
		private Tower _myTower;
		//mySolider[1000]
		private int _money;
		private int _towerUpgradePrice;
        private int _towerLevel;
		//int unlockSoliderPrice;
    
        public void GenerateSolider(){
            // new Soldier?
        }
		public void EarnMoney(int moneyAdd){
            //殺敵 +金錢
            _money += moneyAdd;
        }
		public void UpgradeTower(){
            //玩家花錢升級塔，塔等級提升，下級所需金費增加
            _money -= _towerUpgradePrice;
            _towerLevel++;
            _towerUpgradePrice = _towerUpgradePrice * 2;
        }
        public void UnlockSolider()
        {

        }
	}
}