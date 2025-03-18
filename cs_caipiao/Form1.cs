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
                _btn.ForeColor = Color.Red;
                btn_num[i] = _btn;
                tableLayoutPanel1.Controls.Add(_btn);
            }
        }
    }
}
