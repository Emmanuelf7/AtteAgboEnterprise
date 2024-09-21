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
    public partial class frmAdjustment : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        Form1 f;
        int _qty = 0;
        public frmAdjustment(Form1 fm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            this.f = fm;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void ReferenceNo()
        {
            Random rnd = new Random();
            txtRefno.Text = rnd.Next().ToString();
        }
        public void LoadProduct()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select p.pcode,p.barcode, p.pdescription, b.brand, c.category, p.price, p.qty from tblProduct as p inner join tblBrand as b on b.id=p.bid inner join tblCategory as c on c.id=p.cid where p.pdescription like '%" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                LoadProduct();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(ColName=="Select")
            {
                txtPCode.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() + "" + dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() + "" +dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                _qty = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString()); 
            }
        }

        private void btnSTSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation for empty field
                if(int.Parse(txtQty.Text) > _qty)
                {
                    MessageBox.Show("STOCK QUANTITY ON HAND SHOULD BE GREATER THAN THE ADJUSTMENT QTY.","WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // update stock
                if(cboCommand.Text == "REMOVE FROM INVENTORY")
                {
                    SqlStatement ("update tblProduct set qty = (qty- " + int.Parse(txtQty.Text) + ") where pcode like'" +txtPCode.Text+ "'");
                }else if(cboCommand.Text =="ADD TO INVENTORY")
                {
                    SqlStatement("update tblProduct set qty = (qty+ " + int.Parse(txtQty.Text) + ") where pcode like'" + txtPCode.Text + "'");
                }
                SqlStatement("insert into tblAjustment (refno, pcode, qty, action, remarks, sdate, [user])values('"+txtRefno.Text+"','" +txtPCode.Text+ "' ,'"+int.Parse(txtQty.Text)+"','" +cboCommand.Text+ "' ,'" +txtRemarks.Text+ "','"+ DateTime.Now.ToString("dd-MM-yyyy")+ "','" + txtUser.Text+ "')");
                MessageBox.Show("STOCK HAS BEEN SUCCESSFULLY ADJUSTED.", "PROCESS COMPLETED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProduct();
                Clear();
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void Clear()
        {
            txtRefno.Clear();
            txtDescription.Clear();
            txtPCode.Clear();
            txtQty.Clear();
            txtRefno.Clear();
            txtRemarks.Clear();
            cboCommand.Text = "";
            ReferenceNo();
        }
        public void SqlStatement(string _sql)
        {
            cn.Open();
            cm = new SqlCommand(_sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }
    }
}
