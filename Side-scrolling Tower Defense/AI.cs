using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Side_scrolling_Tower_Defense
{
    class AI
    {
        public Tower aiTower;
        public List<Soldier> soldier = new List<Soldier>();
        private bool counterAttcak_flag = false;


        public AI(Label _lbTowerHp)
        {
            aiTower = new Tower(1000, 50, 250, 1, false, _lbTowerHp);
        }
        public void Intelligence(List<Soldier> enemyS, Grid grid1, Label lb, Tower enemyTower)
        { //智慧產兵 XDDD 目前只會rand產兵
            Random rand = new Random();
            int playerTotalPower = enemyS.Sum(s => s.ATK); //取得玩家總戰力
            int aiTotalPower = soldier.Sum(s => s.ATK);

            if (aiTower.HP <= aiTower.MAXHP / 2 && aiTotalPower <= playerTotalPower && !counterAttcak_flag) //AI快掛時會暴走
            {
                if (rand.Next(10) < 3)
                {
                    int tmp = rand.Next(10);
                    if (tmp <= 5)
                        GenerateSolider(grid1, 1, 50);  // 60% 產第1種
                    else if (tmp > 5 && tmp <= 7)
                        GenerateSolider(grid1, 2, 100); // 20% 產第2種
                    else if (tmp == 8)
                        GenerateSolider(grid1, 3, 300); // 10% 產第3種
                    else
                        GenerateSolider(grid1, 4, 500); // 10% 產第4種
                }
                if (aiTotalPower > playerTotalPower)
                {
                    counterAttcak_flag = true;
                }
            }
            else if (rand.Next(50000) <= playerTotalPower + 10) //五萬分之敵軍戰力產兵機率
            {
                int tmp = rand.Next(10);
                if (tmp <= 5)
                    GenerateSolider(grid1, 1, 50);  // 60% 產第1種
                else if (tmp > 5 && tmp <= 7)
                    GenerateSolider(grid1, 2, 100); // 20% 產第2種
                else if (tmp == 8)
                    GenerateSolider(grid1, 3, 300); // 10% 產第3種
                else if (tmp == 9)
                    GenerateSolider(grid1, 4, 500); // 10% 產第4種
            }
            else
            {
                //aiTower等級不能比玩家高多於3等
                if (rand.Next(1000) <= 2 && aiTower.TowerLevel <= enemyTower.TowerLevel + 3)
                {
                    UpgradeTower(lb);
                }
            }
            MaintainSolidersPosition(enemyS, enemyTower);
        }
        public void GenerateSolider(Panel grid1, int type, int cost)
        {
            if (type == 1)//一般兵種
            {
                soldier.Add(new Soldier(100, 15, 50, 0.3, true, cost));
                grid1.Children.Add(soldier[soldier.Count-1].Show(50, 50, System.Windows.Media.Brushes.Gold));
            }
            else if (type == 2)//高攻 血少 慢
            {
                soldier.Add(new Soldier(70, 30, 50, 0.2, true, cost));
                grid1.Children.Add(soldier[soldier.Count-1].Show(60, 50, System.Windows.Media.Brushes.Red));
            }
            else if (type == 3)//血厚 攻低 慢
            {
                soldier.Add(new Soldier(200, 10, 50, 0.2, true, cost));
                grid1.Children.Add(soldier[soldier.Count-1].Show(80, 50, System.Windows.Media.Brushes.PaleGreen));
            }
            else if (type == 4) //高攻 血厚 速普通
            {
                soldier.Add(new Soldier(200, 25, 50, 0.2, true, cost));
                grid1.Children.Add(soldier[soldier.Count-1].Show(40, 50, System.Windows.Media.Brushes.Navy));
            }
            else
            {
                System.Windows.MessageBox.Show("Error, GenerateSolider calling error type of soldier");
            }

        }
        public void UpgradeTower(Label lb)
        {
            aiTower.Upgrade('t', aiTower.TowerLevel);
            aiTower.Upgrade('h', aiTower.MAXHP + 1);
            aiTower.Upgrade('a', aiTower.ATK + 2);
            aiTower.Upgrade('r', aiTower.RANGE + 3);

            //顯示
            lb.Content = "level:" + aiTower.TowerLevel.ToString() + '\n' + "hp:" + aiTower.HP.ToString() + '\n' + "range:" + aiTower.RANGE.ToString() + '\n' + "atk:" + aiTower.ATK.ToString();
        }
        public void MaintainSolidersPosition(List<Soldier> enemyS, Tower enemyTower)
        {
            for (int i = 0; i <soldier.Count; i++)
            {
                // 每個士兵該往前的往前，該打的打
                soldier[i].Move(enemyS, enemyTower);
                if (soldier[i].HP <= 0)
                {
                    soldier.RemoveAt(i);

                }
            }
        }
    }
}
