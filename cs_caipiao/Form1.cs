using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

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
                _btn.Size = new Size(40, 40);
                _btn.Text = (i + 1) + "";
                _btn.Tag = (i + 1) + "";
                _btn.ForeColor = Color.Black;
                _btn.BackColor = Color.White;
                _btn.Click += btn_num_Click;
                btn_num[i] = _btn;
                tableLayoutPanel1.Controls.Add(_btn);
            }

            //触发和值全选
            checkBox_r5_fix_sum.Checked = false;
            checkBox_r5_fix_sum.Checked = true;

            // 定义特定时间
            DateTime specificTime = new DateTime(2025, 3, 23, 12, 0, 0);
            TimeSpan timeDifference = specificTime - DateTime.Now;
            double secondsDifference = timeDifference.TotalSeconds;
            if (timeDifference.TotalSeconds <= 0.0)
                this.Close();
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



        //生成全排列
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
        
        /// <summary>
        /// 根据过滤条件进行筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_gen_Click(object sender, EventArgs e)
        {
            if (num_last.Count < 6)
            {
                MessageBox.Show("请选中上期6个号码");
                return;
            }

            List<int> num_has = null;
            List<int> num_hasno = null;
            // 提前拿到一些值
            try
            {
                if (checkBox_r6_has.Checked)
                {
                    string str_has = textBox_has.Text.Trim().Replace(" ", "");
                    num_has = str_has.Split(',').Select(int.Parse).ToList();
                }
                if (checkBox_r6_hasno.Checked)
                {
                    string str_hasno = textBox_hasno.Text.Trim().Replace(" ", "");
                    num_hasno = str_hasno.Split(',').Select(int.Parse).ToList();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"输入解析失败：{ex.Message}");
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
                int h1 = comb[0];

                if (checkBox_r1_h1.Checked == false && h1 == 1)
                    continue;
                if (checkBox_r1_h2.Checked == false && h1 == 2)
                    continue;
                if (checkBox_r1_h3.Checked == false && h1 == 3)
                    continue;
                if (checkBox_r1_h4.Checked == false && h1 == 4)
                    continue;
                if (checkBox_r1_h5.Checked == false && h1 == 5)
                    continue;
                if (checkBox_r1_h6.Checked == false && h1 == 6)
                    continue;
                if (checkBox_r1_h7.Checked == false && h1 == 7)
                    continue;
                if (checkBox_r1_h8.Checked == false && h1 == 8)
                    continue;
                if (checkBox_r1_gt8.Checked == false && h1 > 8)
                    continue;


                // rule 2
                int same_ct = comb.Intersect<int>(num_last).Count<int>();


                if (checkBox_r2_same_0.Checked == false && same_ct == 0)
                    continue;
                if (checkBox_r2_same_1.Checked == false && same_ct == 1)
                    continue;
                if (checkBox_r2_same_2.Checked == false && same_ct == 2)
                    continue;
                if (checkBox_r2_same_3.Checked == false && same_ct == 3)
                    continue;
                if (checkBox_r2_same_gt3.Checked == false && same_ct >3)
                    continue;



                //rule 3
                int str2_ct = 0, str3_ct = 0, str4_ct = 0, str_5_ct = 0, str_6 = 0;
                List<int> _c = new List<int>(comb);
                if (_c[1] == _c[0] + 1 && _c[2] == _c[1] + 1 && _c[3] == _c[2] + 1 && _c[4] == _c[3] + 1 && _c[5] == _c[4] + 1)
                {
                    str_6 = 1;

                }


                for (int i = 0; i < 5; i++)
                {
                    //双连号
                    if (_c[i] + 1 == _c[i + 1] && (i + 2 > 5 || _c[i + 1] + 1 != _c[i + 2]) && (i - 1 < 0 || _c[i - 1] + 1 != _c[i]))
                    {
                        str2_ct++;
                        i += 1;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    //三连号
                    if ((_c[i] + 1 == _c[i + 1] && _c[i + 1] + 1 == _c[i + 2]) && (i + 3 > 5 || _c[i + 2] + 1 != _c[i + 3]) && (i - 1 < 0 || _c[i - 1] + 1 != _c[i]))
                    {
                        str3_ct++;
                        i += 2;
                    }
                }



                if ((int)(numericUpDown_str2.Value) != str2_ct)
                    continue;
                if ((int)(numericUpDown_str3.Value) != str3_ct)
                    continue;




                // rule 4
                int odd = 0;
                foreach(int value in comb)
                {
                    if (value % 2 == 1)
                        odd++;
                }
                if (checkBox_r4_06.Checked == false && odd == 0)
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
                if (checkBox_r4_60.Checked == false && odd == 6)
                    continue;

                // rule 5
                int sum = comb.Sum();

                if (checkBox_r5_fix_sum.Checked)
                {
                    int aim_sum = (int)numericUpDown_fix_sum.Value;
                    if (sum != aim_sum)
                        continue;
                }
                else
                {
                    if (checkBox_r5_lt71.Checked == false && sum < 71)
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
                    if (checkBox_r5_gt130.Checked == false && sum > 130)
                        continue;
                }


                // rule 6

                if (checkBox_r6_has.Checked)
                {
                    try
                    {

                        int ct_has_no = 0; //记录不包含给定值的次数
                        foreach(int _n in num_has)
                        {
                            if (comb.Contains<int>(_n) == false)
                                ct_has_no++;
                        }
                        if (ct_has_no > 0)
                            continue;

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show($"{ex.Message}");
                    }
                }
                if (checkBox_r6_hasno.Checked)
                {
                    try
                    {
                        int ct_has = 0; //记录包含给定值的次数
                        foreach (int _n in num_hasno)
                        {
                            if (comb.Contains<int>(_n) == true)
                                ct_has++;
                        }
                        if (ct_has > 0)
                            continue;

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show($"{ex.Message}");
                    }
                }



                // 到这里是全满足，加入候选
                left.Add(comb);

            }


            label_ret.Text = $"过滤结果：<{left.Count} / {combinations.Count}>条";
            int showed = 0;
            listView_ret.Items.Clear();
            foreach (int[] l in left)
            {
                // 仅显示前1000个号
                if (showed > 1000)
                    break;


                ListViewItem lvi = new ListViewItem(l[0].ToString());
                for(int j=1;j<6;j++)
                    lvi.SubItems.Add(l[j].ToString());

                listView_ret.Items.Add(lvi);
                showed++;
            }

        }
        /// <summary>
        /// 机选一组号码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ran_Click(object sender, EventArgs e)
        {
            // 生成6个随机
            Random random = new Random();
            List<int> numbers = new List<int>();
            while (numbers.Count < 6)
            {
                int randomNumber = random.Next(1, 33 + 1);
                if (!numbers.Contains(randomNumber))
                {
                    numbers.Add(randomNumber);
                }
            }
            foreach(int a in numbers)
                Console.WriteLine($"{a}");

            // 所有按钮遍历关闭
            for(int i = 0; i < 33; i++)
            {
                Button btn = btn_num[i];
                int num = Convert.ToInt32(btn.Tag);
                if(num_last.Contains(num))
                    btn_num_Click(btn, e);
            }
            for (int i = 0; i < 33; i++)
            {
                Button btn = btn_num[i];
                int num = Convert.ToInt32(btn.Tag);
                if (numbers.Contains(num))
                    btn_num_Click(btn, e);
            }
        }


        // 选固定值，则其他失效
        private void checkBox_r5_fix_sum_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_r5_fix_sum.Checked)
            {
                checkBox_r5_lt71.Enabled = false;
                checkBox_r5_71_80.Enabled = false;
                checkBox_r5_81_90.Enabled = false;
                checkBox_r5_91_100.Enabled = false;
                checkBox_r5_101_110.Enabled = false;
                checkBox_r5_111_120.Enabled = false;
                checkBox_r5_121_130.Enabled = false;
                checkBox_r5_gt130.Enabled = false;
            }
            else
            {
                checkBox_r5_lt71.Enabled = true;
                checkBox_r5_71_80.Enabled = true;
                checkBox_r5_81_90.Enabled = true;
                checkBox_r5_91_100.Enabled = true;
                checkBox_r5_101_110.Enabled = true;
                checkBox_r5_111_120.Enabled = true;
                checkBox_r5_121_130.Enabled = true;
                checkBox_r5_gt130.Enabled = true;
            }
        }

 

    }
}
