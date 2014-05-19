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
        private int _op; // AIs' soldier OverPower
        private int _chanceToGenSoldier;
        private int _chanceToUpgradeTower;
        private int _towerLVlimit;
        private bool _canCounterAttack;


        public AI( Grid _grid, Grid _gridTopBar, Player _player, int difficulty)
        {
            aiTower = new Tower(1000, 50, 250, 1, false, _grid, _gridTopBar);
            player = _player;
            grid = _grid;

            switch (difficulty)
            {
                case 1:
                    _op = 1;
                    _chanceToGenSoldier = 100000;
                    _chanceToUpgradeTower = 100000;
                    _towerLVlimit = 1;
                    _canCounterAttack = false;
                    break;
                case 2:
                    _op = 1;
                    _chanceToGenSoldier = 70000;
                    _chanceToUpgradeTower = 50000;
                    _towerLVlimit = 3;
                    _canCounterAttack = true;
                    break;
                case 3:
                    _op = 2;
                    _chanceToGenSoldier = 50000;
                    _chanceToUpgradeTower = 10000;
                    _towerLVlimit = 5;
                    _canCounterAttack = true;
                    break;
                case 4:
                    _op = 3;
                    _chanceToGenSoldier = 30000;
                    _chanceToUpgradeTower = 1000;
                    _towerLVlimit = 10;
                    _canCounterAttack = true;
                    break;
                default:
                    MessageBox.Show("AI Difficulty Error");
                    break;
            }
        }
        public void Intelligence()
        { //智慧產兵 XDDD 目前只會rand產兵
            Random rand = new Random();
            int playerTotalPower = player.soldier.Sum(s => s.ATK); //取得玩家總戰力
            int aiTotalPower = soldier.Sum(s => s.ATK);

            if (aiTower.HP <= aiTower.MAXHP / 2 && aiTotalPower <= playerTotalPower && !_canCounterAttack) //AI快掛時會暴走
            {
                if (rand.Next(_chanceToGenSoldier / 5) <= playerTotalPower + player.MONEY / 100) 
                {
                    int tmp = rand.Next(7);
                    GenerateSolider(grid, tmp + 1, _op); 
                }
                if (aiTotalPower > playerTotalPower)
                {
                    _canCounterAttack = false;
                }
            }
            else if (rand.Next(_chanceToGenSoldier) <= playerTotalPower + player.MONEY / 100) 
            {
                int tmp = rand.Next(7);
                    GenerateSolider(grid, tmp+1, _op); 
            }
            else
            {
                //aiTower等級不能比玩家高多於3等
                if (rand.Next(_chanceToUpgradeTower) <= 2 && aiTower.TowerLevel <= player.myTower.TowerLevel + _towerLVlimit)
                    UpgradeTower();
            }
            MaintainSolidersPosition(player.soldier, player.myTower);
        }
        public void GenerateSolider(Panel grid1, int type, int op)
        {
            if(type == 1)
            {
                soldier.Add(new Saber(true, op));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(85, 70, "/Images/eSoldier1Move.gif"));
            }
            else if(type==2)
            {
                soldier.Add(new Archer(true, op, grid));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(65, 55, "/Images/eSoldier5Move.gif"));
            }
            else if (type == 3)
            {
                soldier.Add(new Caster(true, op));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(70, 60, "/Images/eSoldier2Move.gif"));
            }
            else if (type == 4)
            {
                soldier.Add(new Rider(true, op));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(80, 75, "/Images/eSoldier3Move.gif"));
            }
            else if (type == 5)
            {
                soldier.Add(new Assassin(true, op));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(90, 50, "/Images/eSoldier4Move.gif"));
            }
            else if (type == 6)
            {
                soldier.Add(new Lancer(true, op));
                grid1.Children.Add(soldier[soldier.Count - 1].Show(100, 50, "/Images/tower_test2.gif"));
            }
            else if (type == 7)
            {
                soldier.Add(new Berserker(true, op));
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
                //// 每個士兵該往前的往前，該打的打
                //if (soldier[i].HP <= 0)
                //{
                //    soldier.RemoveAt(i);
                //}
                soldier[i].Move(enemyS, enemyTower);
            }
        }
    }
}
