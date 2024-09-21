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
    public partial class frmPOS : Form
    {
        String id;
        String price;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        frmSecurity f;
        int qty;
       
        string stitle = "Purchasing System";
        public frmPOS(frmSecurity fm)
        {
            
            InitializeComponent();
            lblDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            cn = new SqlConnection(dbcon.MyConnection());
            this.KeyPreview = true;
            f = fm;

            
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                return;
            }
            GetTransNo();
            txtPSearch.Enabled = true;
            txtPSearch.Focus();
        }

        public void GetTransNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("ddMMyyyy");
                string transno;
                int count;
                cn.Open();
                cm = new SqlCommand("select top 1 transno from tblCart where transno like '"+sdate+"%'order by id desc", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransno.Text = sdate + (count + 1);
                   
                }else
                {
                    transno = sdate + "1001";
                    lblTransno.Text = transno;
                }
                dr.Close();
                cn.Close();
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void GetCartTotal()
        {
         
            //double subTotal = Double.Parse(lblSubTotal.Text);
            double discount = Double.Parse(lblDiscount.Text);
            double Total = Double.Parse(lblTotal.Text);
            //lblSubTotal.Text = subTotal.ToString("#,##0.00");
           // lblDiscount.Text = discount.ToString("#,##0.00");
            lblTotal.Text = Total.ToString("#,##0.00");
            lblDisplayTotal.Text = Total.ToString("#,##0.00");
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(lblTransno.Text =="000000000000000")
            {
                return;
            }
            frmLookUp frm = new frmLookUp(this);
            frm.LoadProduct();
            frm.ShowDialog();
        }

        private void txtPSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(txtPSearch.Text == String.Empty)
                {
                    return;
                }else
                {
                    String _pcode;
                    double _price;
                    int _qty;
                    cn.Open();
                    cm = new SqlCommand("select * from tblProduct where barcode like '"+txtPSearch.Text+"'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        // validate product quantity to show avoid selling when product is out of stock
                        qty = int.Parse(dr["qty"].ToString());
                        _pcode = dr["pcode"].ToString();
                        _price = double.Parse(dr["price"].ToString());
                        _qty = int.Parse(txtQty.Text);

                        dr.Close();
                        cn.Close();

                        AddToCart( _pcode,  _price,  _qty);
                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
       
        private void AddToCart(String _pcode, double _price, int _qty)
        {
            String id = "";
            bool found = false;
            int cart_qty=0;
            cn.Open();
            cm = new SqlCommand("select * from tblCart where transno=@transno and pcode=@pcode", cn);
            cm.Parameters.AddWithValue("@transno", lblTransno.Text);
            cm.Parameters.AddWithValue("@pcode", _pcode);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                found = true;
                id = dr["id"].ToString();
                cart_qty = int.Parse(dr["qty"].ToString());
            }
            else
            {
                found = false;
            }
            dr.Close();
            cn.Close();

             if(found == true)
               {
                   // validate product quantity by adding and subtracting
                   if (qty < int.Parse(txtQty.Text)+ cart_qty)
                   {
                       MessageBox.Show("Unable to proceeed. Remaining qty on hand is " + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                       return;
                   }

                   cn.Open();
                   cm = new SqlCommand("update tblCart set qty = (qty +" +_qty+") where id='"+id+"'", cn);
                   cm.ExecuteNonQuery();
                   cn.Close();

                   txtPSearch.SelectionStart = 0;
                   txtPSearch.SelectionLength = txtPSearch.Text.Length;
                  LoadCart();
                this.Dispose();
               }else
               {

                   // validate product quantity to show avoid selling when product is out of stock 
                   if (qty < int.Parse(txtQty.Text))
                   {
                       MessageBox.Show("Unable to proceeed. Remaining qty on hand is " + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                       return;
                   }

                   cn.Open();
                   cm = new SqlCommand("insert into tblCart (transno, pcode, price, qty, sdate, cashier)values(@transno, @pcode, @price, @qty, @sdate,@cashier)", cn);
                   cm.Parameters.AddWithValue("@transno", lblTransno.Text);
                   cm.Parameters.AddWithValue("@pcode", _pcode);
                   cm.Parameters.AddWithValue("@price", _price);
                   cm.Parameters.AddWithValue("@qty", _qty);
                   cm.Parameters.AddWithValue("@sdate", DateTime.Now.ToString("dd-MM-yyyy"));
                   cm.Parameters.AddWithValue("@cashier", lblUser.Text);
                   cm.ExecuteNonQuery();
                   cn.Close();
                   MessageBox.Show("Product quatity has been successfully added", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                   txtPSearch.SelectionStart = 0;
                   txtPSearch.SelectionLength = txtPSearch.Text.Length;
                   LoadCart();
                  this.Dispose();
               }
            }
        
          
        public void LoadCart()
        {
            try
            {
                Boolean hasrecord = false;
                dataGridView1.Rows.Clear();
                int i = 0;
                double Total = 0;
                double discount = 0;
              
                cn.Open();
                cm = new SqlCommand("select c.id, c.pcode, p.pdescription, c.price, c.qty, c.discount, c.total from tblCart as c inner join tblProduct as p on p.pcode = c.pcode where transno like '"+lblTransno.Text+"'and status like 'Pending'", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                   i ++;
                 
                   
                   Total += Double.Parse(dr["total"].ToString());
                   discount += Double.Parse(dr["discount"].ToString());
                   dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["pcode"].ToString(), dr["pdescription"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["discount"].ToString(), Double.Parse(dr["total"].ToString()).ToString("#,##0.00"),"[ + ]", "[ - ]");
                   hasrecord = true;
                }
                dr.Close();
                cn.Close();
             
                lblDiscount.Text = discount.ToString("#,##0.00");
                lblTotal.Text = Total.ToString("#,##0.00");
                GetCartTotal();
                if(hasrecord == true)
                {
                    btnSettlePayment.Enabled = true;
                    btnDiscount.Enabled = true;
                    btnCancel.Enabled = true;
                }
                else
                {
                    btnSettlePayment.Enabled = false;
                    btnDiscount.Enabled = false;
                    btnCancel.Enabled = false;
                }
            }catch(Exception ex)
            {
               
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cn.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(ColName=="Delete")
            {
                if(MessageBox.Show("Remove item from the cart?",stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblCart where id like '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()+"'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been successfully been removed!", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
                 }else if (ColName == "colAdd")
             
                {
                    int i = 0;
                    cn.Open();
                    cm = new SqlCommand("select sum(qty) as qty from tblProduct where pcode like '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()+"'group by pcode", cn);
                    i = int.Parse(cm.ExecuteScalar().ToString());
                    cn.Close();
                  if(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString()) < i)
                  {
                      cn.Open();
                      cm = new SqlCommand("update tblCart set qty = qty + "+ int.Parse(txtQty.Text)+"where transno like '"+lblTransno.Text+ "'and pcode like '"+dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()+ "'", cn);
                      cm.ExecuteNonQuery();
                      cn.Close();
                      LoadCart();
                  }else
                  {
                      MessageBox.Show("Remaining quantity on hand is '" + i + "!", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                      return;
                  }
                }
            else if (ColName == "colRemove")
            {
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select sum(qty) as qty from tblCart where pcode like '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'and transno like '"+lblTransno.Text+ "' group by transno, pcode", cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
                if (i > 1)
                {
                    cn.Open();
                    cm = new SqlCommand("update tblCart set qty = qty - " + int.Parse(txtQty.Text) + "where transno like '" + lblTransno.Text + "'and pcode like '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadCart();
                }
                else
                {
                    MessageBox.Show("Remaining quantity in cart is '" + i + "!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            }
        

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            frmDiscount fm = new frmDiscount(this);
            fm.lblID.Text = id;
            fm.txtPrice.Text = price;
            fm.Show();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Load Discount 
            int i = dataGridView1.CurrentRow.Index;
            id = dataGridView1[1, i].Value.ToString();
            price = dataGridView1[7, i].Value.ToString();

        }

        private void btnSettlePayment_Click(object sender, EventArgs e)
        {
            frmSettle frm = new frmSettle(this);
            frm.txtSales.Text = lblDisplayTotal.Text;
            frm.Show();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            frmSoldItems frm = new frmSoldItems();
            frm.dt1.Enabled = false;
            frm.dt2.Enabled = false;
            frm.suser = lblUser.Text;
            frm.cboCashier.Enabled = false;
            frm.cboCashier.Text = lblUser.Text;
            
            frm.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count > 0)
            {
                MessageBox.Show("unable to logout. Please cancel the transaction.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(MessageBox.Show("Logout Application?","Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
            {
                this.Hide();
                frmSecurity frm = new frmSecurity();
                frm.Show();
            }
        }
        private void frmPOS_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnPassword_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(this);
            frm.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Remove all items from the cart ?","Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
            {
                cn.Open();
                cm = new SqlCommand("delete from tblCart where transno like '" + lblTransno.Text + "'", cn);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("All items has been successfully remove!", "Remove items", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCart();
            }
        }
       
    }
}
