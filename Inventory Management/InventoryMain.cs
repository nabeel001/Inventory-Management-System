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
    public partial class InventoryMain : Form
    {
        public InventoryMain()
        {
            InitializeComponent();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Products pro = new Products();
            pro.Show();
            pro.MdiParent = this;
        }

        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stock list = new Stock();
            list.Show();
            list.MdiParent = this;
        }

        private void productListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AvailableReport rpt = new AvailableReport();
            rpt.Show();
            rpt.MdiParent = this;
        }

        private void stockListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutofStockReport rpt = new OutofStockReport();
            rpt.Show();
            rpt.MdiParent = this;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About abt = new About();
            abt.Show();
            abt.MdiParent = this;
        }

        bool close = true;
        private void InventoryMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to Exit", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    close = false;
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                } 
            }
            
        }

        private void InventoryMain_Load(object sender, EventArgs e)
        {
            SqlConnection con = Connection.GetConnection();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * from [LoginView]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["user_id"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["user_name"].ToString();
            }

        }
    }
}
