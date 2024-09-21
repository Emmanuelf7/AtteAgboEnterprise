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
    public partial class frmCancelDetails : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        frmSoldItems f;
        public frmCancelDetails(frmSoldItems fm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = fm;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cboAction_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if((cboAction.Text!=String.Empty) && (txtQty.Text != String.Empty) && (txtReason.Text!=String.Empty))
                {
                    if(int.Parse(txtQty.Text) >= int.Parse(txtCancelQty.Text))
                    {
                        frmVoid frm = new frmVoid(this);
                        frm.ShowDialog();
                    }
                    
                }
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void RefresList()
        {
            f.LoadRecord();
        }
    }
}
