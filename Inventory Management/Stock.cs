using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Management
{
    public partial class Stock : Form
    {
        public Stock()
        {
            InitializeComponent();
        }

        private void Stock_Load(object sender, EventArgs e)
        {
            reset();
            this.ActiveControl = dateTimePicker1;
            comboBox1.SelectedIndex = -1;
            LoadData();
        }

        private void reset()
        {
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            button1.Text = "Add";
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            reset();
        }

        private bool productRecord(SqlConnection con, string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("select * from [dbo].[Stock] where p_code = '" + productCode + "' ", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                SqlConnection con = Connection.GetConnection();
                con.Open();
                bool status;
                if (textBox3.Text == "0")
                    comboBox1.SelectedIndex = 1;
                else
                    comboBox1.SelectedIndex = 0;

                if (comboBox1.SelectedIndex == 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                var sqlQuery = "";
                if (productRecord(con, textBox1.Text))
                {
                    sqlQuery = @"UPDATE[dbo].[Stock] SET [p_name] = '" + textBox2.Text + "', [p_qty] = " + textBox3.Text + ", [p_status] = '" + status + "', [date] = '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "' WHERE [p_code] = " + textBox1.Text + " ";
                }
                else
                {
                   sqlQuery = @"INSERT INTO [dbo].[Stock] ([p_code],[p_name],[date],[p_qty],[p_status]) VALUES(" + textBox1.Text + ", '" + textBox2.Text + "', '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "', " + textBox3.Text + ", '" + status + "')";
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();
                LoadData();
                button1.Text = "Add";
            }
            else
                MessageBox.Show("select a Product to update!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            reset();
        }

        private bool validation()
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || comboBox1.SelectedIndex < 0)
                return false;
            else
                return true;
        }

        public void LoadData()
        {
            SqlConnection con = Connection.GetConnection();
            SqlDataAdapter sda = new SqlDataAdapter("select * from [dbo].[Stock]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = n + 1;
                dataGridView1.Rows[n].Cells[1].Value = item["p_code"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["p_name"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = float.Parse(item["p_qty"].ToString());
                dataGridView1.Rows[n].Cells[4].Value = Convert.ToDateTime(item["date"].ToString()).ToString("dd/MM/yyyy");
                if ((bool)item["p_status"])
                {
                    dataGridView1.Rows[n].Cells[5].Value = "Available";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[5].Value = "Out of Stock";
                }
            }

            if (dataGridView1.Rows.Count > 0)
            {
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT COUNT(p_code), SUM(p_qty) FROM [dbo].[Stock]", con);
                DataTable dt1 = new DataTable();
                sda1.Fill(dt1);
                label7.Text = dt1.Rows[0][0].ToString();
                label9.Text = dt1.Rows[0][1].ToString();
            }
            else
            {
                label7.Text = "0";
                label9.Text = "0";
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button1.Text = "Update";
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            dateTimePicker1.Text = DateTime.Parse(dataGridView1.SelectedRows[0].Cells[4].Value.ToString()).ToString("dd/MM/yyyy");
            if (dataGridView1.SelectedRows[0].Cells[5].Value.ToString() == "Available")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                SqlConnection con = Connection.GetConnection();
                var sqlQuery = "";
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Product", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (productRecord(con, textBox1.Text))
                {
                    if (result == DialogResult.Yes)
                    {
                        con.Open();
                        sqlQuery = "DELETE FROM [dbo].[Stock] WHERE [p_code] = " + textBox1.Text + " ";
                        SqlCommand cmd = new SqlCommand(sqlQuery, con);
                        cmd.ExecuteNonQuery();
                        LoadData();
                        con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Product Doesn't exist", "Missing Product", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
                MessageBox.Show("select a Product to delete!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            reset();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text.Length > 0)
                {
                    SqlConnection con = Connection.GetConnection();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT p_name FROM [dbo].[Products] where p_code = '" + textBox1.Text + "'", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        textBox2.Text = dt.Rows[0][0].ToString();
                        button1.Text = "Update";
                        textBox3.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Product code!! \nPlease update the Products list", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        textBox1.Focus();
                    }
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (float.Parse(textBox3.Text) > 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
                else
                    comboBox1.SelectedIndex = 1;
            }
        }
    }
}
