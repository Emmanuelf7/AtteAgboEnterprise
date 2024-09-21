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
    public partial class frmSearchProductStockin : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        string stitle = "Puchasing system";
        frmStockInList f;
        public frmSearchProductStockin(frmStockInList flist)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = flist;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

       public void LoadProduct()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select pcode, pdescription, qty from tblProduct where pdescription like '%"+txtPSearch.Text+"'", cn);
            dr = cm.ExecuteReader();
           while(dr.Read())
           {
               i++;
               dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
           }
           dr.Close();
            cn.Close();
           
        }

       private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
       {
           string colName = dataGridView1.Columns[e.ColumnIndex].Name;
           if (colName == "colName")
           {
               if(f.txtRefNo.Text == string.Empty)
               {
                   MessageBox.Show("Please enter reference no","", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   f.txtRefNo.Focus();
                   return;
               }
                   if(f.cboVendor.Text==String.Empty)
                   {
                       MessageBox.Show("Vendor Information is required?", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                       f.cboVendor.Focus();
                       return;
                   }
                  if(MessageBox.Show("Please Add this item?",stitle,MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                  {
                      cn.Open();
                      cm = new SqlCommand("insert into tblStockIn (refno, pcode, sdate, stockinby, vendorid)values(@refno, @pcode, @sdate, @stockinby,@vendorid)", cn);
                      cm.Parameters.AddWithValue("@refno", f.txtRefNo.Text);
                      cm.Parameters.AddWithValue("@pcode", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                      cm.Parameters.AddWithValue("@sdate", SqlDbType.VarChar).Value = f.dtStockInDate.Value.ToString("dd-MM-yyyy");
                      cm.Parameters.AddWithValue("@stockinby", f.txtStockInBy.Text);
                      cm.Parameters.AddWithValue("@vendorid", f.lblVendor.Text);
                      cm.ExecuteNonQuery();
                      cn.Close();
                      MessageBox.Show("Record has been successfully saved!", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                      f.LoadStockIn();
                  }
                  
               }
           }
       

       private void txtPSearch_TextChanged(object sender, EventArgs e)
       {
           LoadProduct();
       }

       
    }
}
