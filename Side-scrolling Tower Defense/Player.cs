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
		private int _money;
        private int moneyGainSpeed;
        private int moneyGainCounter = 0;
		private int _towerUpgradePrice;

        public Label lbMoney;
        public Tower myTower;
        public List<Soldier> soldier = new List<Soldier>();   

        #region get & set
        public int MONEY{
            get { return _money; }
            set { _money = value; }
        }
        public int UPGRADEPRICE{
            get { return _towerUpgradePrice; }
        }
        #endregion

        public Player(Label _lbmoney, Label _lbTowerHP)
        {
            _money = 10000;      // 初始資金
            _towerUpgradePrice = 100; //塔升級費用
            moneyGainSpeed = 1;
            lbMoney = _lbmoney;
            myTower = new Tower(1000, 50, 250, 1, true, _lbTowerHP);
        }
        public void MoneyGain()
        {
            if ((++moneyGainCounter % moneyGainSpeed) == 0)
            {
                _money++;
                moneyGainCounter = 0;        
            }
        }
        public void GenerateSolider(Panel grid1, int type, int overPower, int cost){
            if(type == 1)//一般兵種
            {
                soldier.Add(new Soldier(100 * overPower, 15 * overPower, 50, 0.7 * overPower, false, cost));
                grid1.Children.Add(soldier[soldier.Count-1].Show(50, 50, System.Windows.Media.Brushes.Gold));
            }
            else if(type==2)//高攻 血少 慢
            {
                soldier.Add(new Soldier(70 * overPower, 30 * overPower, 50, 0.4 * overPower, false, cost));
                grid1.Children.Add(soldier[soldier.Count-1].Show(60, 50, System.Windows.Media.Brushes.Red));
            }
            else if (type == 3)//血厚 攻低 慢
            {
                soldier.Add(new Soldier(200 * overPower, 10 * overPower, 50, 0.2 * overPower, false, cost));
                grid1.Children.Add(soldier[soldier.Count-1].Show(80, 50, System.Windows.Media.Brushes.PaleGreen));
            }
            else if (type == 4) //高攻 血厚 速普通
            {
                soldier.Add(new Soldier(200 * overPower, 25 * overPower, 50, 0.5 * overPower, false, cost));
                grid1.Children.Add(soldier[soldier.Count-1].Show(40, 50, System.Windows.Media.Brushes.Navy));
            }
            else
            {
                MessageBox.Show("Error, GenerateSolider calling error type of soldier");
            }
        }
		public void EarnMoney(int moneyAdd){
            //殺敵 +金錢
            _money += moneyAdd;
            lbMoney.Content = "$"+_money.ToString();
        }
		public void UpgradeTower(Label lb){
            //玩家花錢升級塔，塔等級提升，下級所需金費增加(2倍)
            _money -= _towerUpgradePrice;

            //個別數值提升
            _towerUpgradePrice = _towerUpgradePrice * 2;
            myTower.Upgrade('t', myTower.TowerLevel);
            myTower.Upgrade('h', myTower.MAXHP +1);
            myTower.Upgrade('a', myTower.ATK + 2);
            myTower.Upgrade('r', myTower.RANGE+ 3);
            
            //顯示
            lb.Content = "level:" + myTower.TowerLevel.ToString() + '\n' + "hp:" + myTower.HP.ToString() + '\n' + "range:" + myTower.RANGE.ToString() + '\n' + "atk:" + myTower.ATK.ToString();
        }
        public void MaintainSolidersPosition(List<Soldier> enemyS, Tower enemyTower)
        {
            for (int i = 0; i <soldier.Count; i++)
            {
                // 每個士兵該往前的往前，該打的打
                soldier[i].Move(enemyS, enemyTower);
                //Checking life
                if (soldier[i].HP <= 0)
                {
                    soldier.RemoveAt(i);
                }
            }
            foreach (Soldier s in enemyS)//敵人如果有死就加錢
            {
                if (s.HP <= 0)
                {
                    EarnMoney(s.PRICE);
                }
            }
        }
	}
}
