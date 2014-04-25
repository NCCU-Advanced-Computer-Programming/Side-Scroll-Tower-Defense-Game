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
    
		private Tower myTower= new Tower(100,10,10,1);
		//mySolider[1000]
		private int _money;
		private int _towerUpgradePrice;
        //private int _towerLevel;

        //Label[] soldier = new Label[1000]; //場上最多只能有1000我方士兵
        Soldier[] soldier = new Soldier[1000];
        int _soliderOnField = 0;           //場上的我方士兵數(?)

        public int MONEY{
            get { return _money; }
            set { _money = value; }
        }
		//int unlockSoliderPrice;

        public Player()
        {
            _money = 1000;      // 初始資金
            _towerUpgradePrice = 100; //塔升級費用
            myTower = new Tower(100, 10, 10, 1);
            myTower.TowerLevel = 1; // 初始塔等級
        }
        public void GenerateSolider(Panel grid1){
            // new Soldier?
            /*
            soldier[_soliderOnField] = new Label();
            grid1.Children.Add(soldier[_soliderOnField]);
            soldier[_soliderOnField].Margin = new Thickness(800, 85, 0, 0);
            soldier[_soliderOnField].Height = 50;
            soldier[_soliderOnField].Width = 50;
            soldier[_soliderOnField].Background = System.Windows.Media.Brushes.Green;
            */
            soldier[_soliderOnField] = new Soldier();
            grid1.Children.Add(soldier[_soliderOnField].Show());
            _soliderOnField++;
        }
		public void EarnMoney(int moneyAdd, Label lbMoney){
            //殺敵 +金錢
            _money += moneyAdd;
            lbMoney.Content = _money.ToString();
        }
		public void UpgradeTower(Label lb){
            //玩家花錢升級塔，塔等級提升，下級所需金費增加
            _money -= _towerUpgradePrice;
            //myTower.TowerLevel++;

            //所有數值暫時都*2
            _towerUpgradePrice = _towerUpgradePrice * 2;
            myTower.Upgrade('t', myTower.TowerLevel * 2);
            myTower.Upgrade('h', myTower.HP * 2);
            myTower.Upgrade('a', myTower.ATK * 2);
            myTower.Upgrade('r', myTower.RANGE * 2);
            
            //顯示
            lb.Content = "level:" + myTower.TowerLevel.ToString() + '\n' + "hp:" + myTower.HP.ToString() + '\n' + "range:" + myTower.RANGE.ToString() + '\n' + "atk:" + myTower.ATK.ToString();
        }
        public void UnlockSolider()
        {

        }
	}
}
