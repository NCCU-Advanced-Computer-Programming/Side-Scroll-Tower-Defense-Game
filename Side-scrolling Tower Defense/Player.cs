using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Side_scrolling_Tower_Defense
{
    class Player
    {
    
		private Tower myTower;
		//mySolider[1000]
		private int _money;
		private int _towerUpgradePrice;
        //private int _towerLevel;

        Label[] soldier = new Label[1000]; //場上最多只能有1000我方士兵
        int _soliderOnField = 0;           //場上的我方士兵數(?)

		//int unlockSoliderPrice;

        public Player()
        {
            _money = 1000;   // 初始資金
            myTower.TowerLevel = 1; // 初始塔等級
            _towerUpgradePrice = 100; //塔升級費用
            myTower = new Tower(100, 10, 10, myTower.TowerLevel);
        }
        public void GenerateSolider(Panel grid1){
            // new Soldier?
            soldier[_soliderOnField] = new Label();
            grid1.Children.Add(soldier[_soliderOnField]);
            soldier[_soliderOnField].Margin = new Thickness(800, 85, 0, 0);
            soldier[_soliderOnField].Height = 50;
            soldier[_soliderOnField].Width = 50;
            soldier[_soliderOnField].Background = System.Windows.Media.Brushes.Green;
            _soliderOnField++;
        }
		public void EarnMoney(int moneyAdd){
            //殺敵 +金錢
            _money += moneyAdd;
        }
		public void UpgradeTower(){
            //玩家花錢升級塔，塔等級提升，下級所需金費增加
            _money -= _towerUpgradePrice;
            myTower.TowerLevel++;
            _towerUpgradePrice = _towerUpgradePrice * 2;
           // _myTower.Upgrade();
        }
        public void UnlockSolider()
        {

        }
	}
}
