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
		private int _money;
		private int _towerUpgradePrice;

        //Label[] soldier = new Label[1000]; 
        //       public Soldier[] soldier = new Soldier[1000];   //場上最多只能有1000士兵

        public List<Soldier> soldier = new List<Soldier>();   
        int _soliderOnField = 0;                 //場上的我方士兵數(?) <--怪怪

        public int MONEY{
            get { return _money; }
            set { _money = value; }
        }
        public int UPGRADEPRICE{
            get { return _towerUpgradePrice; }
        }
		//int unlockSoliderPrice;

        public Player()
        {
            _money = 1000;      // 初始資金
            _towerUpgradePrice = 100; //塔升級費用
            myTower = new Tower(100, 10, 10, 1);
          //  myTower.TowerLevel = 1; // 初始塔等級
        }
        public void GenerateSolider(Panel grid1){
            // new Soldier?
            soldier.Add(new Soldier(100, 8, 50,0.3, false));
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
            myTower.Upgrade('t', myTower.TowerLevel);
            myTower.Upgrade('h', myTower.HP +1);
            myTower.Upgrade('a', myTower.ATK + 2);
            myTower.Upgrade('r', myTower.RANGE+ 3);
            
            //顯示
            lb.Content = "level:" + myTower.TowerLevel.ToString() + '\n' + "hp:" + myTower.HP.ToString() + '\n' + "range:" + myTower.RANGE.ToString() + '\n' + "atk:" + myTower.ATK.ToString();
        }
        public void MaintainSolidersPosition(List<Soldier> enemyS)
        {
            for (int i = 0; i < _soliderOnField; i++)
            {
                // 每個士兵該往前的往前，該打的打
                soldier[i].Move(enemyS);
                //Checking life
                if (soldier[i].HP <= 0)
                {
                    soldier.RemoveAt(i);
                    _soliderOnField--;
                }
            }
        }
	}
}
