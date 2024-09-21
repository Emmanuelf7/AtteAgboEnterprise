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
    public partial class frmCategoryList : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        public frmCategoryList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            LoadCategoryRecord();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmCategoryEntering frm = new frmCategoryEntering(this);
            frm.btnCSave.Enabled = true;
            frm.btnCUpdate.Enabled = false;
            frm.ShowDialog();
        }
        public void LoadCategoryRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from tblCategory order by category", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["category"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColName=="Edit")
            {
                frmCategoryEntering frm = new frmCategoryEntering(this);
                frm.btnCSave.Enabled = false;
                frm.btnCUpdate.Enabled = true;
                frm.lblIDCategory.Text = dataGridView1[1, e.RowIndex].Value.ToString();
                frm.txtCategory.Text = dataGridView1[2, e.RowIndex].Value.ToString();
                frm.ShowDialog();
            }else if(ColName=="Delete")
            {
                if(MessageBox.Show("Please confirm if you want to delete this brand!","Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblCategory where id like '"+dataGridView1[1, e.RowIndex].Value.ToString()+"'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Brand has been successfully deleted!", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategoryRecord();
                }
            }
        }
    }
}
