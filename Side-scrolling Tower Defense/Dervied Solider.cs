using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Side_scrolling_Tower_Defense
{
    class Saber : Soldier
    {
        //hp=? , atk=? , range = ? , speed =? 
        public Saber(bool isEnemy, int overPower)
            : base(70*overPower, 10*overPower, 1, 0.5*overPower, isEnemy, 100)
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
        private Label bullet;
        private Grid grid;
        public Archer(bool isEnemy, int overPower, Grid _grid)
            : base(200 * overPower, 15 * overPower, 150, 0.5 * overPower, isEnemy, 1000)
        {
            grid = _grid;
            if (isEnemy)
            {
                this.APS = 100;
            }
            else
            {
                this.APS = 80;
            }
        }
        public override bool Attack(List<Soldier> Enemy)
        {
            int target = Int32.MaxValue;
            double nearest = double.MaxValue;
            int movePerStepX = this.RANGE / 10;
            for (int i = 0; i < Enemy.Count; i++)
            {
                double distance = Math.Abs(Enemy[i].POSITION - this.POSITION);
                if (nearest > distance)
                {
                    nearest = distance;
                    target = i;
                }
            }

            if (nearest <= this.RANGE)
            {
                bool isShooted = false; //如果已開槍，則在未打到目標前 isShooted == true
                if (bullet == null && counter == this.APS-10)
                {
                    grid.Children.Add(BulletShow()); //把子彈放進Grid
                }
                if (counter >= this.APS-10)
                    isShooted = true;

                if ((++counter % this.APS) == 0) //控制攻速
                {
                    counter = 0;
                    Enemy[target].GetHurt(this.ATK, null);
                    //Enemy[target].HP -= this.ATK;
                    //Enemy[target].LifeCheck();
                    grid.Children.Remove(bullet);
                    bullet = null;
                }
                else
                {
                    if (bullet != null && isShooted)
                    {
                        if (isEnemy)
                        {
                            bullet.Margin = new System.Windows.Thickness(0, 0, bullet.Margin.Right - movePerStepX, bullet.Margin.Bottom );
                            if (bullet.Margin.Right + movePerStepX < Enemy[target].POSITION)
                            {
                                grid.Children.Remove(bullet);
                                bullet = null;
                                counter = this.APS - 1; //摸到就直接等同攻擊到
                            }
                        }
                        else
                        {
                            bullet.Margin = new System.Windows.Thickness(0, 0, bullet.Margin.Right + movePerStepX, bullet.Margin.Bottom);
                            if (bullet.Margin.Right - movePerStepX > Enemy[target].POSITION)
                            {
                                grid.Children.Remove(bullet);
                                bullet = null;
                                counter = this.APS - 1; //摸到就直接等同攻擊到
                            }
                        }
                    }
                }
                if (!isAttack) //判斷是否需要換gif圖
                {
                    if (imgSourceAttack == null)
                        imgSourceAttack = imgSourceMove.Replace("Move", "Attack");
                    //MessageBox.Show(imgSourceAttack);

                    var _image = new BitmapImage();
                    _image.BeginInit();
                    _image.UriSource = new Uri(imgSourceAttack, UriKind.Absolute);
                    _image.EndInit();
                    ImageBehavior.SetAnimatedSource(Image, _image);
                    Image.Width = Image.ActualWidth + 15;
                    spImg.Width = Image.Width;

                    isAttack = true;
                }

                return true;
            }
            else
            {
                //grid.Children.Remove(bullet);
                //bullet = null;
                return false;
            }

        }
        public override bool Attack(Tower Enemy)
        {
            
            int movePerStepX = this.RANGE / 10 ;
            if (Math.Abs(Enemy.POSITION - this.POSITION) <= this.RANGE)
            {
                bool isShooted = false; //如果已開槍，則在未打到目標前 isShooted == true
                if (bullet == null && counter == this.APS-15)
                {
                    grid.Children.Add(BulletShow()); //把子彈放進Grid
                }
                if (counter >= this.APS-15)
                    isShooted = true;

                if ((++counter % this.APS) == 0) //控制攻速
                {
                    counter = 0;
                    Enemy.GetHurt(this.ATK);
                    grid.Children.Remove(bullet);
                    bullet = null;
                }
                else
                {
                    if (bullet != null && isShooted)
                    {
                        if (isEnemy)
                        {
                            bullet.Margin = new System.Windows.Thickness(0, 0, bullet.Margin.Right - movePerStepX, bullet.Margin.Bottom );
                        }
                        else
                        {
                            bullet.Margin = new System.Windows.Thickness(0, 0, bullet.Margin.Right + movePerStepX, bullet.Margin.Bottom );
                        }
                    }
                }
                if (!isAttack) //判斷是否需要換gif圖
                {
                    if (imgSourceAttack == null)
                        imgSourceAttack = imgSourceMove.Replace("Move", "Attack");
                    //MessageBox.Show(imgSourceAttack);

                    var _image = new BitmapImage();
                    _image.BeginInit();
                    _image.UriSource = new Uri(imgSourceAttack, UriKind.Absolute);
                    _image.EndInit();
                    ImageBehavior.SetAnimatedSource(Image, _image);
                    Image.Width = Image.ActualWidth + 15;
                    spImg.Width = Image.Width;

                    isAttack = true;
                }

                return true;
            }
            else
            {
                grid.Children.Remove(bullet);
                bullet = null;
                return false;
            }

        }
        private Label BulletShow()
        {
            bullet = new Label();
            bullet.Width = 25;
            bullet.Height = 25;
            bullet.Margin = new System.Windows.Thickness(0, 0, this.spImg.Margin.Right, this.spImg.Margin.Bottom + this.Image.Height / 2-10);
            string imgSource;
            imgSource = System.IO.Directory.GetCurrentDirectory().Replace("\\", "/") + "/Images/cannon.PNG";
            bullet.Content = new Image
            {
                Source = new BitmapImage(new Uri(imgSource, UriKind.Absolute)),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Stretch= System.Windows.Media.Stretch.UniformToFill
            };
            bullet.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            bullet.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;

            return bullet;
        }
        public override StackPanel LifeCheck()
        {
            //C#使用記憶體自動回收
            if (HP <= 0)
            {
                if (bullet != null)
                    grid.Children.Remove(bullet);
                spImg.Visibility = System.Windows.Visibility.Hidden;
                grid.Children.Remove(spImg);
                return null;
            }
            else
            {
                return null;
            }
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
            : base(150 * overPower, 35 * overPower, 10, 0.4 * overPower, isEnemy, 100)
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
            : base(1000 * overPower, 20 * overPower, 1, 0.1 * overPower, isEnemy, 1000)
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
            : base(300 * overPower, 7 * overPower, 2, 0.8 * overPower, isEnemy, 100)
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
            : base(200 * overPower, 8 * overPower, 5, 0.5 * overPower, isEnemy, 100)
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
            : base(100 * overPower, 40 * overPower, 1, 0.7 * overPower, isEnemy, 100)
        {

        }

        public new void Skill()
        {
            //Shadow Stab 對敵方單體造成超高傷害 使用之後必定死亡 CD?
        }


    }


}
