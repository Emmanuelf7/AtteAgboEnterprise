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
    public partial class frmSettle : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
       
        frmPOS fm;
       // string stitle = "Purchasing System";
        public frmSettle(frmPOS flist)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            fm = flist;
        }

        private void frmSettle_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double sales = double.Parse(txtSales.Text);
                double cash = double.Parse(txtCash.Text);
                double change = cash - sales;
                txtChange.Text = change.ToString("#,##0.00");
            }catch(Exception)
            {
                txtChange.Text = "0.00";
            }
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn7.Text;
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn8.Text;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn9.Text;
        }

        private void tbnC_Click(object sender, EventArgs e)
        {
            txtCash.Clear();
            txtCash.Focus();
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn4.Text;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn5.Text;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn6.Text;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn0.Text;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn1.Text;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn2.Text;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn3.Text;
        }

        private void btn00_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn00.Text;
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
           try
           {
               if((double.Parse(txtChange.Text) < 0) || (txtCash.Text == String.Empty))
               {
                   MessageBox.Show("Insufficient amount. Please enter the correct amount!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   return;
               }else
                   {
                      
                   for(int i = 0; i<fm.dataGridView1.Rows.Count; i++)
                   {
                       cn.Open();
                       cm = new SqlCommand("update tblproduct set qty = qty - "+ int.Parse(fm.dataGridView1.Rows[i].Cells[5].Value.ToString())+ "where pcode = '" +fm.dataGridView1.Rows[i].Cells[2].Value.ToString()+"'", cn);
                       cm.Parameters.AddWithValue("@transno", fm.lblTransno.Text);
                       cm.ExecuteNonQuery();
                       cn.Close();

                           cn.Open();
                           cm = new SqlCommand("update tblCart set status = 'Sold' where id = '" + fm.dataGridView1.Rows[i].Cells[1].Value.ToString()+"'", cn);
                           cm.ExecuteNonQuery();
                           cn.Close();
                           
                   }
                   MessageBox.Show("Payment successfully saved!", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   fm.LoadCart();
                   fm.GetTransNo();
                   this.Dispose();
                   }
           }catch(Exception)
           {
               MessageBox.Show("Insufficient amount. Please enter the correct amount!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
           }
        }
    }
}
