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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Inventory_Management
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                SqlConnection con = Connection.GetConnection();
                con.Open();
                
                var sqlQuery = "";
                if (productRecord(con, textBox1.Text))
                {
                    sqlQuery = @"UPDATE[dbo].[Products] SET [p_name] = '" + textBox2.Text + "', [p_brand] = '" + textBox3.Text + "' WHERE [p_code] = " + textBox1.Text + " ";
                }
                else
                {
                    sqlQuery = @"INSERT INTO [dbo].[Products] ([p_code],[p_name],[p_brand]) VALUES(" + textBox1.Text + ", '" + textBox2.Text + "', '" + textBox3.Text + "')";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();

                loadData();
                con.Close();
                button2.Text = "Add";
            }
            else
                MessageBox.Show("select a Product to update!!","Warning",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            reset();
        }

        private bool productRecord(SqlConnection con, string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("select * from [dbo].[Products] where p_code = '"+ productCode +"' ", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public void loadData()
        {
            SqlConnection con = Connection.GetConnection();
            SqlDataAdapter sda = new SqlDataAdapter("select * from [dbo].[Products]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["p_code"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["p_name"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["p_brand"].ToString();
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button2.Text = "Update";
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
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
                        sqlQuery = "DELETE FROM [dbo].[Products] WHERE [p_code] = " + textBox1.Text + " ";
                        SqlCommand cmd = new SqlCommand(sqlQuery, con);
                        cmd.ExecuteNonQuery();
                        loadData();
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

        private void reset()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            reset();
        }

        private bool validation()
        {
            if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text))
                return false;
            else
                return true;
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
                        button2.Text = "Update";
                        textBox3.Focus();
                    }
                    else
                        textBox2.Focus();
                }
            }
        }
    }
}
