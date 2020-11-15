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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = Connection.GetConnection();
            SqlDataAdapter sda = new SqlDataAdapter("select * from [dbo].[Login] where user_id = " + textBox1.Text + " and pwd ='" + textBox2.Text + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                this.Hide();
                InventoryMain main = new InventoryMain();
                main.Show();
            }
            else
            {
                MessageBox.Show("Invalid user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1_Click(sender, e);
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection con = Connection.GetConnection();
            SqlDataAdapter sda = new SqlDataAdapter("select * from [dbo].[Login] where user_id = " + textBox3.Text + " ", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                MessageBox.Show("Already Existing User", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button4_Click(sender, e);
            }
            else
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"INSERT INTO [dbo].[Login]
           ([user_id]
           ,[user_name]
           ,[pwd])
     VALUES
           ('" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "')", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("New User Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                button4_Click(sender, e);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox3.Focus();
        }
    }
}
