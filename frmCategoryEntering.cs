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
    public partial class frmCategoryEntering : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        frmCategoryList flist;
        public frmCategoryEntering(frmCategoryList fm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            flist = fm;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Clear()
    {
        btnCSave.Enabled = true;
        btnCUpdate.Enabled = false;
        txtCategory.Clear();
        txtCategory.Focus();
    }
        private void btnCSave_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("insert into tblCategory (category)values(@category)", cn);
                cm.Parameters.AddWithValue("@category", txtCategory.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Category has been successfully saved!", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear();
                flist.LoadCategoryRecord();
                this.Dispose();
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("update tblCategory set category=@category where id like '" + lblIDCategory.Text + "'", cn);
                cm.Parameters.AddWithValue("category", txtCategory.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Category has been successfully saved!", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear();
                flist.LoadCategoryRecord();
                this.Dispose();
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCDelete_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
