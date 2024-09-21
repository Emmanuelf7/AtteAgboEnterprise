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
    public partial class frmVendorList : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        public frmVendorList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void frmVendorList_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmVendor f = new frmVendor(this);
            f.btnUpdate.Enabled = false;
            f.btnSave.Enabled = true;
            f.Show();
        }

        public void LoadVendor()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select vendor, address, contactperson, phone, email from tblVendor",cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr["vendor"].ToString(), dr["address"].ToString(), dr["contactperson"].ToString(), dr["phone"].ToString(), dr["email"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(ColName=="Edit")
            {
                frmVendor fm = new frmVendor(this);
                fm.btnSave.Enabled = false;
                fm.btnUpdate.Enabled = true;
                fm.lblID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                fm.txtVendor.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                fm.txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                fm.txtContactperson.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                fm.txtPhoneNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                fm.txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                fm.Show();
                LoadVendor();
            }else if(ColName=="Delete")
            {
                if(MessageBox.Show("DELETE THIS RECORD? CLICK YES TO CONFIRM","Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblVendor where id like '"+dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()+ "'",cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Record has been successfully deleted");
                    LoadVendor();
                }
            }
        }
    }
}
