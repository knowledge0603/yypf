using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace yypf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
          
            InitializeComponent();
        }

        DataSet dataSet = new DataSet();
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=product.mdb");

        #region 药物功能

        #region  输入药物名 点击检索按钮检索
        private void button1_Click(object sender, EventArgs e)
        {
            //access 数据库连接，取得药物名
          
            if(textBox1.Text.ToString()=="")
            {
                MessageBox.Show("请输入检索药物名称！");
                return;
            }
            if (textBox1.Text.ToString()!="")
            {
                con.Open();
                dataSet.Clear();
                OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select * from 药物表 where 药物名 like '%"+textBox1.Text+"%'", con);
                MyAdapter.Fill(dataSet);
                con.Close();
            }
            if (dataSet.Tables[0].Rows.Count==0) 
            {
                MessageBox.Show("没有该药物！");
                return;
            }
            string[] 药物名 = new String[dataSet.Tables[0].Rows.Count];
            for (int i = 0; i < dataSet.Tables[0].Rows.Count;i++ )
            {
                药物名[i] = dataSet.Tables[0].Rows[i]["药物名"].ToString();
            }
            //groupBox3动态添加药物名按钮
            Button[] cmd = new Button[dataSet.Tables[0].Rows.Count];
            this.flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                cmd[i] = new Button();
                cmd[i].Click += new System.EventHandler(this.btn_Click);
                cmd[i].Size = new Size(100, 30);
                cmd[0].Top = 25;
                cmd[0].Left = 10;
                if(i!=0){
                    cmd[i].Top = 25;
                    cmd[i].Top = cmd[i - 1].Top + cmd[i - 1].Height + 5;
                }
                cmd[i].Visible = true;
                cmd[i].Text = 药物名[i];
                this.flowLayoutPanel1.AutoScroll = true;
                this.flowLayoutPanel1.Controls.Add(cmd[i]);
            }
          }
        #endregion

        #region  显示药物详细内容
        //点击药物名时显示药物详细内容
        private void btn_Click(object sender, System.EventArgs e) 
        {
            //界面显示药物名称
            textBox3.Text = ((Button)sender).Text;
            con.Open();
            dataSet.Clear();
            OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select 药物简介,id,药物价格 from 药物表 where 药物名 = '" + textBox3.Text + "'", con);
            MyAdapter.Fill(dataSet);
            //界面显示药物简介
            richTextBox1.Text = dataSet.Tables[0].Rows[0]["药物简介"].ToString();
            //药物Id设为隐藏
            textBox4.Text = dataSet.Tables[0].Rows[0]["id"].ToString();
            textBox4.Hide();
            textBox11.Text = dataSet.Tables[0].Rows[0]["药物价格"].ToString();
            con.Close();
        }
        #endregion

        #region  获得中文字符串首字母
        /*/// <summary> 
        /// 在指定的字符串列表CnStr中检索符合拼音索引字符串 
        /// </summary> 
        /// <param name="CnStr">汉字字符串</param> 
        /// <returns>相对应的汉语拼音首字母串</returns> 
        public static string GetSpellCode(string CnStr)
        {
            string strTemp = "";
            int iLen = CnStr.Length;
            int i = 0;

            for (i = 0; i <= iLen - 1; i++)
            {
                strTemp += GetCharSpellCode(CnStr.Substring(i, 1));
            }

            return strTemp;
        }

        /// <summary> 
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母 
        /// </summary> 
        /// <param name="CnChar">单个汉字</param> 
        /// <returns>单个大写字母</returns> 
        private static string GetCharSpellCode(string CnChar)
        {
            long iCnChar;

            byte[] ZW = System.Text.Encoding.Default.GetBytes(CnChar);

            //如果是字母，则直接返回 
            if (ZW.Length == 1)
            {
                return CnChar.ToUpper();
            }
            else
            {
                // get the array of byte from the single char 
                int i1 = (short)(ZW[0]);
                int i2 = (short)(ZW[1]);
                iCnChar = i1 * 256 + i2;
            }
            // iCnChar match the constant 
            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {
                return "A";
            }
            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {
                return "B";
            }
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {
                return "C";
            }
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {
                return "D";
            }
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {
                return "E";
            }
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {
                return "F";
            }
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {
                return "G";
            }
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {
                return "H";
            }
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {
                return "J";
            }
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {
                return "K";
            }
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {
                return "L";
            }
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {
                return "M";
            }

            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {
                return "N";
            }
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {
                return "O";
            }
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {
                return "P";
            }
            else if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {
                return "Q";
            }
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {
                return "R";
            }
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {
                return "S";
            }
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {
                return "T";
            }
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {
                return "W";
            }
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {
                return "X";
            }
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {
                return "Y";
            }
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {
                return "Z";
            }
            else return ("?");
        }*/

        /// <summary>
        /// 取得汉字拼音的首字母
        /// </summary>
        /// <param name="strText">汉字串</param>
        /// <returns>汉字串的首字母串</returns>
        public static string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            StringBuilder myStr = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                myStr.Append(GetSpell(strText.Substring(i, 1)));
            }
            return myStr.ToString();
        }
        /// <summary>
        /// 取得一个汉字的拼音首字母
        /// </summary>
        /// <param name="cnChar">一个汉字</param>
        /// <returns>首字母</returns>
        private static string GetSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else return cnChar;
        }


        #region 药名首字母包含拼接
        //药名首字母包含拼接
        public  string strTemp = null ;
        private void button2_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "A";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "B";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "C";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "D";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "E";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "F";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "G";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "H";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "J";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "K";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "L";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "M";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "N";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "O";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "P";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "Q";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "R";
        }

        private void button20_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "S";
        }

        private void button21_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "T";
        }

        private void button22_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "W";
        }

        private void button24_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "X";
        }

        private void button26_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "Y";
        }

        private void button27_Click(object sender, EventArgs e)
        {
            strTemp = strTemp + "Z";
        }
        #endregion
        #endregion

        #region 首字母 模糊查询药物
        private void button28_Click(object sender, EventArgs e)
        {
            //--------------------------------
            if (strTemp == null || strTemp == "") 
            {
                MessageBox.Show("请输入药物首字母！");
                return;
            }
            con.Open();
            dataSet.Clear();
            OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select * from 药物表 where 药物名首字母 like '%" + strTemp + "%'", con);
            MyAdapter.Fill(dataSet);
            con.Close();
            if (dataSet.Tables[0].Rows.Count==0)
            {
                MessageBox.Show("没有包含该字母的药物！");
                strTemp = "";
                return;
            }
            string[] strTemp1 = new String[dataSet.Tables[0].Rows.Count];
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                strTemp1[i] = dataSet.Tables[0].Rows[i]["药物名"].ToString();
            }
            //groupBox3动态添加药物名按钮
            Button[] cmd = new Button[dataSet.Tables[0].Rows.Count];
            this.flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                cmd[i] = new Button();
                cmd[i].Click += new System.EventHandler(this.btn_Click);
                cmd[i].Size = new Size(100, 30);
                cmd[0].Top = 25;
                cmd[0].Left = 10;
                if (i != 0)
                {
                    cmd[i].Top = 25;
                    cmd[i].Top = cmd[i - 1].Top + cmd[i - 1].Height + 5;
                }
                cmd[i].Visible = true;
                cmd[i].Text = strTemp1[i];
                this.flowLayoutPanel1.AutoScroll = true;
                this.flowLayoutPanel1.Controls.Add(cmd[i]);
            }
            strTemp = "";
            //--------------------------------
        }
        #endregion

        # region 添加药物
        private void button10_Click(object sender, EventArgs e)
        {
            //药物名或详细内容为空时提示
            if (textBox3.Text=="")
            {
                MessageBox.Show("请输入药物名称！");
                return;
            }
            if (richTextBox1.Text == "")
            {
                MessageBox.Show("请输入药物详细内容！");
                return;
            }
            con.Open();
            dataSet.Clear();
            OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select 药物名 from 药物表 where 药物名 = '" + textBox3.Text + "'", con);
            MyAdapter.Fill(dataSet);
            con.Close();
            if (dataSet.Tables[0].Rows.Count != 0)
            {
                MessageBox.Show("该药物名已经存在！");
                return;
            }
            con.Open();
            OleDbCommand aCommand = new OleDbCommand("insert into 药物表 (药物名, 药物简介,  药物名首字母,药物价格 ) values ( '" + textBox3.Text + "','" + richTextBox1.Text + "','" + GetChineseSpell(textBox3.Text) + "'," + textBox11.Text + ")", con);
            aCommand.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("药物添加成功！");
            textBox3.Text = "";
            textBox11.Text = "";
            richTextBox1.Text = "";

        }
        # endregion

        # region 修改药物
        private void button25_Click(object sender, EventArgs e)
        {
            //药物名或详细内容为空时提示
            if (textBox3.Text == "")
            {
                MessageBox.Show("请输入药物名称！");
                return;
            }
            if (richTextBox1.Text == "")
            {
                MessageBox.Show("请输入药物详细内容！");
                return;
            }
            con.Open();
            //按id主键经行更新
            OleDbCommand aCommand = new OleDbCommand("update  药物表 set 药物价格 =" + textBox11.Text + "," + "药物名 ='" + textBox3.Text + "'," + "药物简介='" + richTextBox1.Text + "'," + "药物名首字母='" + GetChineseSpell(textBox3.Text) + "'" + " where id = " + textBox4.Text, con);
            aCommand.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("药物修改成功！");
        }
        # endregion

        # region 删除药物
        private void button23_Click(object sender, EventArgs e)
        {
            
            //MessageBox.Show("该药物删除成功！");
            DialogResult result = MessageBox.Show("确认要删除该药物！", "标题", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                con.Open();
                //按id主键经行更新
                OleDbCommand aCommand = new OleDbCommand("delete from   药物表" + " where id = " + textBox4.Text, con);
                aCommand.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("该药物删除成功！");
            }

        }
        # endregion

        #endregion 

        #region 配方功能

        #region 配方检索
        private void button29_Click(object sender, EventArgs e)
        {
            //access 数据库连接，取得药物名
            if (textBox5.Text.ToString() == "")
            {
                MessageBox.Show("请输入检索配方名称！");
                return;
            }
            if (textBox5.Text.ToString() != "")
            {
                con.Open();
                dataSet.Clear();
                OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select * from 配方表 where 配方名 like '%" + textBox5.Text + "%'", con);
                MyAdapter.Fill(dataSet);
                con.Close();
            }
            if (dataSet.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("没有该配方！");
                return;
            }
            string[] 配方名 = new String[dataSet.Tables[0].Rows.Count];
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                配方名[i] = dataSet.Tables[0].Rows[i]["配方名"].ToString();
            }
            //groupBox5动态添加配方名按钮
            Button[] cmd = new Button[dataSet.Tables[0].Rows.Count];
            this.flowLayoutPanel2.Controls.Clear();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                cmd[i] = new Button();
                cmd[i].Click += new System.EventHandler(this.btn_PeiFang_Click);
                cmd[i].Size = new Size(100, 30);
                cmd[0].Top = 25;
                cmd[0].Left = 10;
                if (i != 0)
                {
                    cmd[i].Top = 25;
                    cmd[i].Top = cmd[i - 1].Top + cmd[i - 1].Height + 5;
                }
                cmd[i].Visible = true;
                cmd[i].Text = 配方名[i];
                this.flowLayoutPanel2.AutoScroll = true;
                this.flowLayoutPanel2.Controls.Add(cmd[i]);
            }
        }
        #endregion 

        public string strPeiFang = null;
        private void button53_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "A";
        }
        #endregion

        private void button52_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "B";
        }

        private void button46_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "C";
        }

        private void button41_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "D";
        }

        private void button49_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "E";
        }

        private void button44_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "F";
        }

        private void button39_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "G";
        }

        private void button36_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "H";
        }

        private void button48_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "J";
        }

        private void button43_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "K";
        }

        private void button38_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "L";
        }

        private void button45_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "M";
        }

        private void button40_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "N";
        }

        private void button34_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "O";
        }

        private void button33_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "P";
        }

        private void button32_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "Q";
        }

        private void button31_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "R";
        }

        private void button51_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "S";
        }

        private void button50_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "T";
        }

        private void button47_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "W";
        }

        private void button42_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "X";
        }

        private void button37_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "Y";
        }

        private void button35_Click(object sender, EventArgs e)
        {
            strPeiFang = strPeiFang + "Z";
        }

        private void button30_Click(object sender, EventArgs e)
        {
            //--------------------------------
            if (strPeiFang == null || strPeiFang == "")
            {
                MessageBox.Show("请输入配方首字母！");
                return;
            }
            con.Open();
            dataSet.Clear();
            OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select * from 配方表 where 配方首字母 like '%" + strPeiFang + "%'", con);
            MyAdapter.Fill(dataSet);
            con.Close();
            if (dataSet.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("没有包含该字母的配方！");
                strPeiFang = "";
                return;
            }
            string[] strPeiFang1 = new String[dataSet.Tables[0].Rows.Count];
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                strPeiFang1[i] = dataSet.Tables[0].Rows[i]["配方名"].ToString();
            }
            //groupBox3动态添加药物名按钮
            Button[] cmd = new Button[dataSet.Tables[0].Rows.Count];
            this.flowLayoutPanel2.Controls.Clear();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                cmd[i] = new Button();
                cmd[i].Click += new System.EventHandler(this.btn_PeiFang_Click);
                cmd[i].Size = new Size(100, 30);
                cmd[0].Top = 25;
                cmd[0].Left = 10;
                if (i != 0)
                {
                    cmd[i].Top = 25;
                    cmd[i].Top = cmd[i - 1].Top + cmd[i - 1].Height + 5;
                }
                cmd[i].Visible = true;
                cmd[i].Text = strPeiFang1[i];
                this.flowLayoutPanel2.AutoScroll = true;
                this.flowLayoutPanel2.Controls.Add(cmd[i]);
            }
            strPeiFang = "";
            //--------------------------------
        }

        #region  显示配方详细内容
        //点击药物名时显示药物详细内容
        private void btn_PeiFang_Click(object sender, System.EventArgs e)
        {
            //界面显示药物名称
            textBox8.Text = ((Button)sender).Text;
            con.Open();
            dataSet.Clear();
            OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select 配方详细内容,id ,一级分类,二级分类 from 配方表 where 配方名 = '" + textBox8.Text + "'", con);
            MyAdapter.Fill(dataSet);
            //界面显示药物简介
            richTextBox2.Text = dataSet.Tables[0].Rows[0]["配方详细内容"].ToString();
            //配方Id设为隐藏
            textBox6.Text = dataSet.Tables[0].Rows[0]["id"].ToString();
            textBox6.Hide();
            textBox9.Text = dataSet.Tables[0].Rows[0]["一级分类"].ToString();
            textBox10.Text = dataSet.Tables[0].Rows[0]["二级分类"].ToString();
            con.Close();
        }
        #endregion

        private void button54_Click(object sender, EventArgs e)
        {
            //配方名或详细内容为空时提示
            if (textBox8.Text == "")
            {
                MessageBox.Show("请输入配方名称！");
                return;
            }
            if (richTextBox2.Text == "")
            {
                MessageBox.Show("请输入配方详细内容！");
                return;
            }
            con.Open();
            dataSet.Clear();
            OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select 配方名 from 配方表 where 配方名 = '" + textBox8.Text + "'", con);
            MyAdapter.Fill(dataSet);
            con.Close();
            if (dataSet.Tables[0].Rows.Count != 0)
            {
                MessageBox.Show("该配方名已经存在！");
                return;
            }
            con.Open();
            OleDbCommand aCommand = new OleDbCommand("insert into 配方表 (配方名, 配方详细内容,  配方首字母 ,一级分类,二级分类) values ( '" + textBox8.Text + "','" + richTextBox2.Text + "','" + GetChineseSpell(textBox8.Text) + "','" + textBox9.Text + "','" + textBox10.Text + "'" + ")", con);
            aCommand.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("配方添加成功！");
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            richTextBox2.Text = "";
        }

        private void button55_Click(object sender, EventArgs e)
        {
            //配方名或详细内容为空时提示
            if (textBox8.Text == "")
            {
                MessageBox.Show("请输入配方名称！");
                return;
            }
            if (richTextBox2.Text == "")
            {
                MessageBox.Show("请输入配方详细内容！");
                return;
            }
            con.Open();
            //按id主键经行更新
            OleDbCommand aCommand = new OleDbCommand("update  配方表 set 一级分类 ='" + textBox9.Text + "'," + "二级分类 ='" + textBox10.Text + "'," + "配方名 ='" + textBox8.Text + "'," + "配方详细内容='" + richTextBox2.Text + "'," + "配方首字母='" + GetChineseSpell(textBox8.Text) + "'" + " where id = " + textBox6.Text, con);
            aCommand.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("配方修改成功！");
        }

        private void button56_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("确认要删除该配方！", "标题", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                con.Open();
                //按id主键经行更新
                OleDbCommand aCommand = new OleDbCommand("delete from   配方表" + " where id = " + textBox6.Text, con);
                aCommand.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("该配方删除成功！");
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        
     
        //分类一选择时显示分类一项目
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string temp2 = this.comboBox1.SelectedItem.ToString();
            con.Open();
            dataSet.Clear();
            OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select distinct 二级分类 from 配方表 where  一级分类 = '" + temp2 + "'", con);
            MyAdapter.Fill(dataSet);
            con.Close();
            comboBox2.Items.Clear();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                comboBox2.Items.Add(dataSet.Tables[0].Rows[i]["二级分类"].ToString());
            }
        }
        //分类一选择时显示分类一项目
        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            con.Open();
            dataSet.Clear();
            OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select distinct 一级分类 from 配方表 ", con);
            MyAdapter.Fill(dataSet);
            con.Close();
            comboBox1.Items.Clear();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                comboBox1.Items.Add( dataSet.Tables[0].Rows[i]["一级分类"].ToString());
            }
        }


        //点击二级分类显示二级分类下的配方名
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            string temp3 = this.comboBox2.SelectedItem.ToString();
            con.Open();
            dataSet.Clear();
            OleDbDataAdapter MyAdapter = new OleDbDataAdapter("select * from 配方表 where 二级分类 = '" + temp3 + "'", con);
            MyAdapter.Fill(dataSet);
            con.Close();
            if (dataSet.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("没有该配方！");
                return;
            }
            string[] 配方名 = new String[dataSet.Tables[0].Rows.Count];
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                配方名[i] = dataSet.Tables[0].Rows[i]["配方名"].ToString();
            }
            //groupBox5动态添加配方名按钮
            Button[] cmd = new Button[dataSet.Tables[0].Rows.Count];
            this.flowLayoutPanel2.Controls.Clear();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                cmd[i] = new Button();
                cmd[i].Click += new System.EventHandler(this.btn_PeiFang_Click);
                cmd[i].Size = new Size(100, 30);
                cmd[0].Top = 25;
                cmd[0].Left = 10;
                if (i != 0)
                {
                    cmd[i].Top = 25;
                    cmd[i].Top = cmd[i - 1].Top + cmd[i - 1].Height + 5;
                }
                cmd[i].Visible = true;
                cmd[i].Text = 配方名[i];
                this.flowLayoutPanel2.AutoScroll = true;
                this.flowLayoutPanel2.Controls.Add(cmd[i]);
            }
        }

    }
}
 