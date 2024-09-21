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
    public partial class frmProduct : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        frmProductList flist;
        public frmProduct(frmProductList fm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            flist = fm;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadCategory()
        {
            cboCategory.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select category from tblCategory", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboCategory.Items.Add(dr[0].ToString());

            }
            dr.Close();
            cn.Close();
        }

        public void LoadBrand()
        {
            cboBrand.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select brand from tblBrand", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboBrand.Items.Add(dr[0].ToString());

            }
            dr.Close();
            cn.Close();
        }

        private void Clear()
        {
            txtPcode.Clear();
            txtBarcode.Clear();
            txtDescription.Clear();
            cboBrand.Text = "";
            cboBrand.Text = "";
            txtPrice.Clear();
            txtReorder.Clear();
            txtPcode.Focus();
            btnPSave.Enabled = true;
            btnPUpdate.Enabled = false;
        }

        public bool IsValidated()
        {
            if(txtPcode.Text.Trim()==String.Empty)
            {
                MessageBox.Show("Product code is required?","Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                txtPcode.Focus();
                return false;
            }
            if(txtDescription.Text.Trim()==String.Empty)
            {
                MessageBox.Show("Description is required?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                txtDescription.Focus();
                return false;
            }
            if(cboBrand.Text.Trim()==String.Empty)
            {
                MessageBox.Show("Brand is required?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                cboBrand.Focus();
                return false;
            }
            if(cboCategory.Text.Trim()==String.Empty)
            {
                MessageBox.Show("Category is required?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                cboCategory.Focus();
                return false;
            }
            if(txtPrice.Text.Trim()==String.Empty)
            {
                MessageBox.Show("Price is required?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                txtPrice.Focus();
                return false;
            }else
            {
                decimal Price;
                bool isDecimal = decimal.TryParse(txtPrice.Text.Trim(), out Price);
                if(!isDecimal)
                {
                    MessageBox.Show("Price should be figures?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Clear();
                    txtPrice.Focus();
                    return false;

                }
            }
            return true;
        }
        private void btnPSave_Click(object sender, EventArgs e)
        {
            if(IsValidated())
            {
                try
                {
                    if (MessageBox.Show("Please confirm if you want to save this record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string bid = ""; string cid = "";
                        cn.Open();
                        cm = new SqlCommand("select id from tblBrand where brand like'" + cboBrand.Text + "'", cn);
                        dr = cm.ExecuteReader();
                        dr.Read();
                        if (dr.HasRows)
                        {
                            bid = dr[0].ToString();
                        }
                        dr.Close();
                        cn.Close();

                        cn.Open();
                        cm = new SqlCommand("select id from tblCategory where category like'" + cboCategory.Text + "'", cn);
                        dr = cm.ExecuteReader();
                        dr.Read();
                        if (dr.HasRows)
                        {
                            cid = dr[0].ToString();
                        }
                        dr.Close();
                        cn.Close();

                        cn.Open();
                        cm = new SqlCommand("insert into tblProduct(pcode,barcode, pdescription, bid, cid, price, reorder)values(@pcode, @barcode, @pdescription, @bid, @cid, @price, @reorder)", cn);
                        cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                        cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                        cm.Parameters.AddWithValue("@pdescription", txtDescription.Text);
                        cm.Parameters.AddWithValue("@bid", bid);
                        cm.Parameters.AddWithValue("@cid", cid);
                        cm.Parameters.AddWithValue("@price", txtPrice.Text);
                        cm.Parameters.AddWithValue("@reorder", txtReorder.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Record has been successfully saved!", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clear();
                        flist.LoadProduct();
                       this.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnPUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Please confirm if you want to update this record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string bid = ""; string cid = "";
                    cn.Open();
                    cm = new SqlCommand("select id from tblBrand where brand like'" + cboBrand.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        bid = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("select id from tblCategory where category like'" + cboCategory.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        cid = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("update tblProduct set barcode=@barcode, pdescription=@pdescription, bid=@bid, cid=@cid, price=@price, reorder=@reorder where pcode like @pcode", cn);
                    cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdescription", txtDescription.Text);
                    cm.Parameters.AddWithValue("@bid", bid);
                    cm.Parameters.AddWithValue("@cid", cid);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", int.Parse(txtReorder.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Record has been successfully updated!", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    flist.LoadProduct();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPDelete_Click(object sender, EventArgs e)
        {
            Clear();
        }

        
    }
}
