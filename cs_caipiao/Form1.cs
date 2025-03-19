using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs_caipiao
{
    public partial class Form1: Form
    {
        Button[] btn_num = new Button[33];
        SortedSet<int> num_last = new SortedSet<int>(); //上一期

        // 全排列
        List<int[]> combinations;  
        //过滤剩余
        List<int[]> left;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初始化按钮
            for(int i = 0; i < 33; i++)
            {
                Button _btn = new Button();
                _btn.Size = new Size(39, 39);
                _btn.Text = (i + 1) + "";
                _btn.Tag = (i + 1) + "";
                _btn.ForeColor = Color.Black;
                _btn.BackColor = Color.White;
                _btn.Click += btn_num_Click;
                btn_num[i] = _btn;
                tableLayoutPanel1.Controls.Add(_btn);
            }


        }

        // 按钮选数
        private void btn_num_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            int num = Convert.ToInt32(b.Tag);
            
            if (num_last.Contains(num))
            {
                num_last.Remove(num);
                b.BackColor = Color.White;
            }
            else
            {
                if (num_last.Count < 6)
                {
                    num_last.Add(num);
                    b.BackColor = Color.Green;
                }
            }


            //展示 
            string s = "";
            foreach(int n in num_last)
            {
                s = s + n + " ";
            }
            label_last.Text = s;
        }



        //生成
        private List<int[]> GenerateCombinations(int n, int k)
        {
            List<int[]> result = new List<int[]>();
            int[] combination = new int[k];

            void Backtrack(int start, int index)
            {
                if (index == k)
                {
                    result.Add((int[])combination.Clone());
                    return;
                }

                for (int i = start; i <= n; i++)
                {
                    combination[index] = i;
                    Backtrack(i + 1, index + 1);
                }
            }

            Backtrack(1, 0);
            return result;
        }

        private void button_gen_Click(object sender, EventArgs e)
        {
            if (num_last.Count < 6)
            {
                MessageBox.Show("请选中上期6个号码");
                return;
            }



            //全排列 

            int n = 33;
            int k = 6;
            combinations = GenerateCombinations(n, k);
            left = new List<int[]>();

            // 进入筛选
            foreach (var comb in combinations)
            {
                // rule 1
                if (checkBox_r1_head.Checked)
                {
                    if (comb[0] > 8)
                        continue;
                }

                // rule 2
                int same_ct = comb.Intersect<int>(num_last).Count<int>();
                if (same_ct > 2)
                    continue;

                if (checkBox_r2_same_0.Checked == false && same_ct == 0)
                    continue;

                if (checkBox_r2_same_1.Checked == false && same_ct == 1)
                    continue;

                if (checkBox_r2_same_2.Checked == false && same_ct == 2)
                    continue;


                //rule 3
                int str_ct = 0;
                for(int i = 1; i < 6; i++)
                {
                    if (comb[i] == comb[i - 1] + 1)
                    {
                        str_ct++;
                    }
                }
                if (checkBox_r3_straight.Checked && str_ct != 1)
                    continue;



                // rule 4
                int odd = 0;
                foreach(int value in comb)
                {
                    if (value % 2 == 1)
                        odd++;
                }
                if (odd == 0 || odd == 6)
                    continue;
                if (checkBox_r4_15.Checked == false && odd == 1)
                    continue;
                if (checkBox_r4_24.Checked == false && odd == 2)
                    continue;
                if (checkBox_r4_33.Checked == false && odd == 3)
                    continue;
                if (checkBox_r4_42.Checked == false && odd == 4)
                    continue;
                if (checkBox_r4_51.Checked == false && odd == 5)
                    continue;


                // rule 5
                int sum = comb.Sum();
                if (sum < 71 || sum > 130)
                    continue;

                if (checkBox_r5_71_80.Checked == false && sum >= 71 && sum <= 80)
                    continue;
                if (checkBox_r5_81_90.Checked == false && sum >= 81 && sum <= 90)
                    continue;
                if (checkBox_r5_91_100.Checked == false && sum >= 91 && sum <= 100)
                    continue;
                if (checkBox_r5_101_110.Checked == false && sum >= 101 && sum <= 110)
                    continue;
                if (checkBox_r5_111_120.Checked == false && sum >= 111 && sum <= 120)
                    continue;
                if (checkBox_r5_121_130.Checked == false && sum >= 121 && sum <= 130)
                    continue;


                // 到这里是全满足，加入候选
                left.Add(comb);
            }


            label_ret.Text = "过滤结果：<" + left.Count + ">条";
            int showed = 0;
            foreach (int[] l in left)
            {
                if (showed > 1000)
                    break;


                ListViewItem lvi = new ListViewItem(l[0].ToString());
                for(int j=1;j<6;j++)
                    lvi.SubItems.Add(l[j].ToString());

                listView_ret.Items.Add(lvi);
                showed++;
            }

        }
    }
}
