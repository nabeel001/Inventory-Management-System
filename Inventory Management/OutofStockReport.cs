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
    public partial class OutofStockReport : Form
    {
        public OutofStockReport()
        {
            InitializeComponent();
        }

        private void OutofStockReport_Load(object sender, EventArgs e)
        {
            SqlConnection con = Connection.GetConnection();
            LoadData(con);

            if (dataGridView1.Rows.Count > 0)
            {
                SqlDataAdapter sda1 = new SqlDataAdapter("EXEC Report @status = 0", con);
                DataTable dt1 = new DataTable();
                sda1.Fill(dt1);
                label3.Text = dt1.Rows[0][0].ToString();
            }
            else
            {
                label3.Text = "0";
            }
        }

        public void LoadData(SqlConnection con)
        {
            SqlDataAdapter sda = new SqlDataAdapter("select p.p_code, p.p_name, p.p_brand, s.p_qty, s.date, s.p_status from Products p inner join Stock s on p.p_code = s.p_code and s.p_status = 0 ", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = n + 1;
                dataGridView1.Rows[n].Cells[1].Value = item["p_code"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["p_name"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["p_brand"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = float.Parse(item["p_qty"].ToString());
                dataGridView1.Rows[n].Cells[5].Value = Convert.ToDateTime(item["date"].ToString()).ToString("dd/MM/yyyy");
                dataGridView1.Rows[n].Cells[6].Value = "Out of Stock";
            }
        }
    }
}
