using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using RestSharp;
using System.Web;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
//using System.Data.SqlClient;

namespace OfflineBusData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string conn = "Data Source=" + Application.StartupPath + "\\offlinedata.db";
        public static SQLiteConnection connnect;
        static DataTable dt;
        bool paused = false;
        private string GetNumber(string par)
        {
            return Regex.Replace(par, @"[^\d]*", "");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            connnect = new SQLiteConnection(conn);
            connnect.Open();
            if (File.Exists(Application.StartupPath + "\\offlinedata.db"))
            {
                dt = DAL.GetDataTable(
                    "SELECT [line_id] as ID ,[line_name] as Line_Name,'' as Stations,''"
                    + " as Info FROM [offline_data] where [state]=0 order by [line_id]", connnect);
                dataGridView1.DataSource = dt;
                //foreach (DataGridViewRow dgr in dataGridView1.Rows)
                //{
                //    dgr.Cells["Stations"].Value = GetNumber(dgr.Cells["Line_Name"].Value.ToString());
                //}
                this.Text = "爱帮公交数据采集   ---现有数据" + dataGridView1.RowCount.ToString() + "条---";
            }
            //SqlConnection sc = new SqlConnection("Data Source=.;Initial Catalog=busOffLine;Persist Security Info=True;User ID=sa;Password=barcode;Connect Timeout=5");
            //dt= RaywindStudio.DAL.DAL.GetDataTable("select * from offline_data", sc);
            //dataGridView1.DataSource = dt;
            ////foreach (DataGridViewRow dgr in dataGridView1.Rows)
            ////{
            ////    dgr.Cells["Stations"].Value = GetNumber(dgr.Cells["Line_Name"].Value.ToString());
            ////}
            //this.Text += "   ---现有数据" + dataGridView1.RowCount.ToString() + "条---";
        }

        /// <summary>
        /// 通过路线名查询线路站点及站点坐标,utf-8编码  //f41c8afccc586de03a99c86097e98ccb //9c489bc685a5bd95d42a9f92a318218a
        /// </summary>
        public static string lineStationAPI =
            "http://openapi.aibang.com/bus/lines?app_key=9c489bc685a5bd95d42a9f92a318218a&city=%E5%8C%97%E4%BA%AC&with_xys=1&alt=json&q=";

        private void QueryStation(string lineName, DataTable dtd)
        {
            string url = (textBox2.Text.Trim().Length > 0 ?
                lineStationAPI.Replace("9c489bc685a5bd95d42a9f92a318218a", textBox2.Text.Trim()) : lineStationAPI)
                + HttpUtility.UrlEncode(lineName).Replace("(", "%28").Replace(")", "%29");
            var client = new RestClient(url);
            //MessageBox.Show(url);
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content;
                aibangBusLineClass jsr = (aibangBusLineClass)JsonConvert.DeserializeObject(jsn, typeof(aibangBusLineClass));
                if (!string.IsNullOrEmpty(jsr.result_num) && int.Parse(jsr.result_num) > 0)
                {
                    int index = lineName.IndexOf('(');
                    string line1 = lineName.Substring(0, index);
                    string tmp = lineName.Substring(index + 1, lineName.Length - index - 2);
                    string line2 = tmp.Split('-')[0];
                    string line3 = tmp.Split('-')[1];
                    //MessageBox.Show(lineName + "\r" + line1 + "\r" + line2 + "\r" + line3 + "\r");
                    //aibangBusLineClass.Line ln = jsr.lines.line.First((s) => s.name == lineName);
                    aibangBusLineClass.Line ln = new aibangBusLineClass.Line();
                    foreach (var ttt in jsr.lines.line)
                    {
                        int indexa = ttt.name.IndexOf('(');
                        string line1a = ttt.name.Substring(0, indexa);
                        string tmpa = ttt.name.Substring(indexa + 1, ttt.name.Length - indexa - 2);
                        string line2a = tmpa.Split('-')[0];
                        string line3a = tmpa.Split('-')[1];
                        if (checkBox1.Checked)
                        {
                            if (line1a == line1 && line2a == line2 && line3a == line3)
                            {
                                ln = ttt;
                                break;
                            }
                        }
                        else
                        {
                            if (GetNumber(line1a) == GetNumber(line1) && line2a.Contains(line2) && line3a.Contains(line3))
                            {
                                ln = ttt;
                                break;
                            }
                        }

                    }

                    if (ln != null)
                    {
                        string stastr = ln.stats;
                        string xys = ln.stat_xys;
                        string[] sr1 = stastr.Split(';');
                        string[] sr2 = xys.Split(';');
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < sr1.Length; i++)
                            sb.Append(sr1[i] + "," + sr2[i] + ";");

                        foreach (DataRow dr in dtd.Rows)
                        {
                            if (dr["Line_Name"].ToString() == lineName)
                            {
                                dr["Stations"] = sb.ToString().Substring(0, sb.Length - 1);
                                dr["Info"] = ln.info;
                                break;
                            }
                        }
                        //dataGridView1.DataSource = dt;
                    }
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Text = "暂停";
            button1.Enabled = false;
            button2.Enabled = true;
            numericUpDown1.Enabled = false;
            progressBar1.Maximum = dataGridView1.Rows.Count;

            progressBar1.Value = 0;
            progressBar1.Visible = true;
            DataTable dttt = (DataTable)dataGridView1.DataSource;
            foreach (DataRow dr in dttt.Rows)
            {
                Application.DoEvents();
                while (paused)
                {
                    Application.DoEvents();
                    RaywindStudio.DTime.Delay.delay(Convert.ToDouble(numericUpDown1.Value));
                }
                if (dr["Stations"].ToString().Trim().Length == 0)
                {
                    QueryStation(dr["Line_Name"].ToString(), dttt);
                    RaywindStudio.DTime.Delay.delay(Convert.ToDouble(numericUpDown1.Value));
                    dataGridView1.DataSource = dttt;
                    progressBar1.Value += 1;
                }
            }
            button2.Text = "保存txt";
            paused = false;
            button1.Enabled = true;
            //button2.Enabled = true;
            numericUpDown1.Enabled = true;
            progressBar1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "暂停")
            {
                paused = true;
                button2.Text = "恢复";
            }
            else if (button2.Text == "恢复")
            {
                paused = false;
                button2.Text = "暂停";
            }
            else
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    string filePath = Application.StartupPath + "\\aibangLines.txt";
                    if (checkBox1.Checked)
                        filePath = Application.StartupPath + "\\aibangLines_tmp.txt";
                    using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        StringBuilder sb = new StringBuilder("");
                        sb.AppendLine("【start】");
                        foreach (DataGridViewRow dr in dataGridView1.Rows)
                        {
                            sb.AppendLine(dr.Cells["ID"].Value.ToString() + "ÿ" + dr.Cells["Line_Name"].Value.ToString() + "ÿ"
                                + dr.Cells["Stations"].Value.ToString() + "ÿ" + dr.Cells["Info"].Value.ToString() + "");
                        }
                        sb.AppendLine("【end】");
                        sw.Write(sb.ToString());
                        sw.Close();
                    }
                    if (MessageBox.Show("保存完成！\n\n" + filePath+"\n\n是否打开？","文件已保存",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                        System.Diagnostics.Process.Start(filePath);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length > 0)
            {
                DataTable dtt = dt.Clone();
                dtt.Clear();
                dt.Select("Line_Name like '%" + textBox1.Text.Trim() + "%'").CopyToDataTable(dtt, LoadOption.OverwriteChanges);   
                //dt = dtt.Copy();
                dataGridView1.DataSource = dtt;
                this.Text = "爱帮公交数据采集   ---现有数据" + dataGridView1.RowCount.ToString() + "条---";
                return;
            }

            if (tbFile.Text.Trim().Length > 0 && File.Exists(Application.StartupPath+"\\"+tbFile.Text))
            {
                //dt = DAL.GetDataTable(
                //    "SELECT [line_id] as ID ,[line_name] as Line_Name,'' as Stations,''"
                //    + " as Info FROM [offline_data] where [state]=0 order by [line_id]", connnect);

                DataTable dtt = dt.Clone();
                dtt.Clear();
                if (radioButton3.Checked)
                {
                    dtt = dt.Copy();
                }
                else
                {
                    string textTmp = File.ReadAllText(Application.StartupPath + "\\" + tbFile.Text, Encoding.UTF8);
                    if (radioButton1.Checked)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (textTmp.Contains(dr["ID"].ToString().Trim() + "ÿ"))
                                dtt.Rows.Add(dr["ID"], dr["Line_Name"], dr["Stations"], dr["Info"]);
                        }
                    }
                    else if (radioButton2.Checked)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!textTmp.Contains(dr["ID"].ToString().Trim() + "ÿ"))
                                dtt.Rows.Add(dr["ID"], dr["Line_Name"], dr["Stations"], dr["Info"]);
                        }
                    }
                }
                dataGridView1.DataSource = dtt;
                this.Text = "爱帮公交数据采集   ---现有数据" + dataGridView1.RowCount.ToString() + "条---";
                return;
            }
        }

        private void 从浏览器查看当前路线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.CurrentRow.Index >= 0)
                System.Diagnostics.Process.Start(
                "http://openapi.aibang.com/bus/lines?app_key=9c489bc685a5bd95d42a9f92a318218a&city=%E5%8C%97%E4%BA%AC&q="
                 + GetNumber(dataGridView1.CurrentRow.Cells["Line_Name"].Value.ToString()));
        }

        private void 从ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.CurrentRow.Index >= 0)
                System.Diagnostics.Process.Start(
                "http://bjgj.aibang.com:8899/bus.php?city=%E5%8C%97%E4%BA%AC&no=1&encrypt=0&id="
                + dataGridView1.CurrentRow.Cells["ID"].Value.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tbRec.Text.Trim().Length > 0 && File.Exists(Application.StartupPath + "\\" + tbRec.Text))
            {
                //dt = DAL.GetDataTable(
                //    "SELECT [line_id] as ID ,[line_name] as Line_Name,'' as Stations,''"
                //    + " as Info FROM [offline_data] where [state]=0 order by [line_id]", connnect);

                DataTable dtt = dt.Clone(); 
                dtt.Clear();
                string textTmp = File.ReadAllText(Application.StartupPath + "\\" + tbRec.Text, Encoding.UTF8);
                StreamReader sr = new StreamReader(Application.StartupPath + "\\" + tbRec.Text, Encoding.UTF8);
                while (sr.Peek() > 0)
                {
                    string temp = sr.ReadLine();
                    if(temp.Contains("ÿ"))
                    {
                        string[] tmps=temp.Split('ÿ');
                        dtt.Rows.Add(tmps[0], tmps[1], tmps.Length > 2 ? tmps[2] : "", tmps.Length > 3 ? tmps[3] : "");
                    }                    
                }
                dataGridView1.DataSource = dtt;
                this.Text = "爱帮公交数据采集   ---现有数据" + dataGridView1.RowCount.ToString() + "条---";
                return;
            }
        }
    }
}
