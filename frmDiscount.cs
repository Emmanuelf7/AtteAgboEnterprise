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
    public partial class frmDiscount : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        string stitle = "Purchasing System";
        frmPOS f;
        public frmDiscount(frmPOS fm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = fm;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Add Discount?","Click yes to confirm.", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("update tblCart set discount=@discount, discount_percent=@discount_percent where id=@id", cn);
                    cm.Parameters.AddWithValue("@discount", Double.Parse(txtAmount.Text));
                    cm.Parameters.AddWithValue("@discount_percent", Double.Parse(txtDiscount.Text));
                    cm.Parameters.AddWithValue("@id", int.Parse(lblID.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    f.LoadCart();
                    this.Dispose();
                }
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double discount = Double.Parse(txtPrice.Text) * Double.Parse(txtDiscount.Text);
                txtAmount.Text = discount.ToString("#,##0.00");
            }
            catch (Exception)
            {
               
                txtAmount.Text = "0";
                    
            }
        }
    }
}
