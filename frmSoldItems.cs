using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace AttehAgboEnterprise
{
    public partial class frmSoldItems : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        public string suser;
        public frmSoldItems()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
          
           
            LoadRecord();
            LoadCashier();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadRecord()
        {
            int i = 0;
            double _total = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            if (cboCashier.Text == "All Cashier")
            {
                cm = new SqlCommand("select c.id, c.transno, c.pcode, p.pdescription, c.price, c.qty, c.discount, c.total from tblCart as c inner join tblProduct as p on c.pcode=p.pcode where status like 'Sold' and sdate between'" + dt1.Value.ToString("dd-MM-yyyy") + "' and'" + dt2.Value.ToString("dd-MM-yyyy") + "'", cn);
            }else
            {
                cm = new SqlCommand("select c.id, c.transno, c.pcode, p.pdescription, c.price, c.qty, c.discount, c.total from tblCart as c inner join tblProduct as p on c.pcode=p.pcode where status like 'Sold' and sdate between'" + dt1.Value.ToString("dd-MM-yyyy") + "' and'" + dt2.Value.ToString("dd-MM-yyyy") + "'and Cashier like '" + cboCashier.Text + "'", cn);
            }
            dr = cm.ExecuteReader();

            while (dr.Read())
            {
                i += 1;
                _total += double.Parse(dr["total"].ToString());
                dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdescription"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["discount"].ToString(), dr["total"].ToString());
            }
            dr.Close();
            cn.Close();
            lblTotal.Text = _total.ToString("#,##0.00");
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(colName=="colCancel")
            {
                frmCancelDetails frm = new frmCancelDetails(this);
                frm.txtID.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                frm.txtTransno.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                frm.txtPCode.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                frm.txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                frm.txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                frm.txtQty.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                frm.txtDiscount.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                frm.txtTotal.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                frm.txtCancel.Text = suser;
                frm.Show();
            }
        }

        private void dt1_ValueChanged(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void dt2_ValueChanged(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmSoldReport fm = new frmSoldReport(this);
            fm.LoadReport();
            fm.Show();
        }

        private void cboCashier_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public void LoadCashier()
        {
            cboCashier.Items.Clear();
            cboCashier.Items.Add("All Cashier");
            cn.Open();
            cm = new SqlCommand("select * from tblUser where role like 'Cashier'", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                cboCashier.Items.Add(dr["username"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void cboCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
           LoadRecord();
        }
    }
}
