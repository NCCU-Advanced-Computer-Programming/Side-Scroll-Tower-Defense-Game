using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Side_scrolling_Tower_Defense
{
    class AI
    {
        public Tower aiTower;
        public List<Soldier> soldier = new List<Soldier>();
        private Player player;
        private Grid grid;
 //       private bool counterAttcak_flag = false;


        public AI( Grid _grid, Grid _gridTopBar, Player _player)
        {
            aiTower = new Tower(1000, 50, 250, 1, false, _grid, _gridTopBar);
            player = _player;
            grid = _grid;
        }
        public void Intelligence()
        { //智慧產兵 XDDD 目前只會rand產兵
            Random rand = new Random();
            int playerTotalPower = player.soldier.Sum(s => s.ATK); //取得玩家總戰力
            int aiTotalPower = soldier.Sum(s => s.ATK);
/*
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
            else*/ if (rand.Next(100000) <= playerTotalPower + 100) 
            {
                int tmp = rand.Next(5);
                    GenerateSolider(grid,5, 500); 
            }
            else
            {
                //aiTower等級不能比玩家高多於3等
                if (rand.Next(1000) <= 2 && aiTower.TowerLevel <= player.myTower.TowerLevel + 3)
                {
                    UpgradeTower();
                }
            }
            MaintainSolidersPosition(player.soldier, player.myTower);
        }
        public void GenerateSolider(Panel grid1, int type, int cost)
        {
            if(type == 1)
            {
                soldier.Add(new Saber(true, 1));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(85, 70, "/Images/eSoldier1Move.gif"));
            }
            else if(type==2)
            {
                soldier.Add(new Archer(true, 1, grid));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(65, 55, "/Images/eSoldier5Move.gif"));
            }
            else if (type == 3)
            {
                soldier.Add(new Caster(true, 1));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(70, 60, "/Images/eSoldier2Move.gif"));
            }
            else if (type == 4)
            {
                soldier.Add(new Rider(true, 1));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(80, 75, "/Images/eSoldier3Move.gif"));
            }
            else if (type == 5)
            {
                soldier.Add(new Assassin(true, 1));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(90, 50, "/Images/eSoldier4Move.gif"));
            }
            else if (type == 6)
            {
                soldier.Add(new Lancer(true, 1));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(100, 50, "/Images/tower_test2.gif"));
            }
            else if (type == 7)
            {
                soldier.Add(new Berserker(true, 1));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(110, 50, "/Images/tower_test2.gif"));
            }
            else
            {
                MessageBox.Show("Error, GenerateSolider calling error type of soldier");
            }

        }
        public void UpgradeTower()
        {
            aiTower.Upgrade('t', aiTower.TowerLevel);
            aiTower.Upgrade('h', aiTower.MAXHP + 100);
            aiTower.Upgrade('a', aiTower.ATK + 10);
            aiTower.Upgrade('r', aiTower.RANGE + 10);
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
