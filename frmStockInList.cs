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
    public partial class frmStockInList : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        string stitle = "POS System";
        SqlDataReader dr;
        public frmStockInList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            LoadStockIn();
            LoadVendor();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        public void LoadStockIn()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwStockIn where refno like'" + txtRefNo.Text + "'and status like 'Pending'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["refno"].ToString(), dr["pcode"].ToString(), dr["pdescription"].ToString(), dr["qty"].ToString(), dr["sdate"].ToString(), dr["stockinby"].ToString(), dr["vendor"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(ColName=="Delete")
            {
                if(MessageBox.Show("Remove this item ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblStockIn where id = '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Record has been successfully deleted!", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStockIn();
                }
                
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmSearchProductStockin frm = new frmSearchProductStockin(this);
            frm.LoadProduct();
            frm.Show();
        }

        private void LoadStockInHistory()
        {
            int i = 0;
           dataGridView2.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwStockIn where  sdate between '"+date1.Value.ToString("dd-MM-yyyy")+ "'and'" +date2.Value.ToString("dd-MM-yyyy") + "' and status like 'Done'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr["vendor"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        public void Clear()
        {
            txtContactPerson.Clear();
            txtRefNo.Clear();
            txtStockInBy.Clear();
            dtStockInDate.CustomFormat = "";
            cboVendor.Text = "";
            txtAddress.Clear();
        }
        private void btnSTSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(dataGridView1.Rows.Count > 0)
                {
                    
                        // update product qty
                        if(MessageBox.Show("Please confirm if you want to save this record?",stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                            cn.Open();
                            cm = new SqlCommand("update tblProduct set qty = qty + " + int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString()) + "where pcode like'" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "'", cn);
                            cm.ExecuteNonQuery();
                            cn.Close();
                            // update tblStockIn
                            cn.Open();
                            cm = new SqlCommand("update tblStockIn set qty = qty + " + int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString()) + ", status = 'Done' where id like '" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'", cn);
                            cm.ExecuteNonQuery();
                            cn.Close();
                            // MessageBox.Show("Stock in has been save successfully");
                        }
                        Clear();
                        LoadStockIn();
                        
                    }
                }
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadStockInHistory();
        }

        public void LoadVendor()
        {
            cboVendor.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select * from tblVendor", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboVendor.Items.Add(dr["vendor"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void cboVendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboVendor_TextChange(object sender, EventArgs e)
        {
            cn.Open();
            cm = new SqlCommand("select * from tblVendor where vendor like '" + cboVendor.Text + "'", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                lblVendor.Text = dr["id"].ToString();
                txtContactPerson.Text = dr["contactperson"].ToString();
                txtAddress.Text = dr["address"].ToString();
            }
            dr.Close();
            cn.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Random rnd = new Random();
            txtRefNo.Clear();
            txtRefNo.Text += rnd.Next();
          
            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
