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
using System.Windows.Forms.DataVisualization.Charting;
namespace AttehAgboEnterprise
{
    public partial class frmRecord : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        
        public frmRecord()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadTopSelling()
        {
            int i = 0;
        dataGridView1.Rows.Clear();   
        cn.Open();
            if(cboTopSelect.Text == "SORT BY QTY")
            {
                cm = new SqlCommand("select top 10  pcode, pdescription, isnull(sum(qty),0) as qty, isnull(sum(total),0) as total  from vwSoldItems where sdate between '" + dt1.Value.ToString("dd-MM-yyyy") + "'and '" + dt2.Value.ToString("dd-MM-yyyy") + "'and status like 'Sold' group by pcode, pdescription order by qty desc", cn);
            }
            else if (cboTopSelect.Text == "SORT BY TOTAL AMOUNT")
            {
                cm = new SqlCommand("select top 10  pcode, pdescription, isnull(sum(qty),0) as qty, isnull(sum(total),0) as total  from vwSoldItems where sdate between '" + dt1.Value.ToString("dd-MM-yyyy") + "'and '" + dt2.Value.ToString("dd-MM-yyyy") + "'and status like 'Sold' group by pcode, pdescription order by total desc", cn);
            }
       
        dr = cm.ExecuteReader();
        while(dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr["pcode"].ToString(), dr["pdescription"].ToString(), dr["qty"].ToString(), double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
            }
        dr.Close();
        cn.Close();
    }

        public void LoadCancelledOrder()
        {
            int i = 0;
            dataGridView5.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwCancelledOrder where sdate between '" + dt5.Value.ToString("dd-MM-yyyy") + "'and'" + dt6.Value.ToString("dd-MM-yyyy") + "'", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView5.Rows.Add(i, dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdescription"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["total"].ToString(), dr["sdate"].ToString(), dr["voidby"].ToString(), dr["cancelledby"].ToString(), dr["reason"].ToString(), dr["action"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            
        }

        private void btnLoadSold_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select c.pcode, p.pdescription, c.price, sum(c.qty) as tot_qty, sum(c.discount) as tot_discount, sum(c.total) as total from tblCart as c inner join tblProduct as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dt3.Value.ToString("dd-MM-yyyy") + "'and '" + dt4.Value.ToString("dd-MM-yyyy") + "'group by c.pcode, p.pdescription, c.price", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView2.Rows.Add(i, dr["pcode"].ToString(), dr["pdescription"].ToString(), Double.Parse(dr["price"].ToString()).ToString("#,##0.00"), dr["tot_qty"].ToString(), dr["tot_discount"].ToString(), Double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
                }
                dr.Close();
                cn.Close();

                //String x;
                cn.Open();
                cm = new SqlCommand("select isnull(sum(total),0) from tblCart where status like 'Sold' and sdate between '" + dt3.Value.ToString("dd-MM-yyyy") + "'and '" + dt4.Value.ToString("dd-MM-yyyy") + "'", cn);
                lblTotal.Text = Double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");
                cn.Close();
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadInvventory()
        {
            dataGridView4.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new SqlCommand("select p.pcode, p.barcode, p.pdescription, b.brand, c.category, p.price, p.qty, p.reorder from tblProduct as p inner join tblBrand as b on p.bid = b.id inner join tblCategory as c on p.cid=c.id ", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView4.Rows.Add(i, dr["pcode"].ToString(), dr["barcode"].ToString(), dr["pdescription"].ToString(), dr["brand"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["reorder"].ToString());
            }
            cn.Close();
        }
        public void LoadCriticalItems()
        {
            try
            {
                dataGridView3.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select * from vwCriticalItems", cn);
                dr = cm.ExecuteReader();
                while(dr.Read())
                {
                    i++;
                    dataGridView3.Rows.Add(i, dr["pcode"].ToString(), dr["barcode"].ToString(), dr["pdescription"].ToString(), dr["brand"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["reorder"].ToString(), dr["qty"].ToString());
                }
                dr.Close();
                cn.Close();
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LoadCriticalItems();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            frm.LoadReport();
            frm.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }

        public void LoadStockInHistory()
        {
            dataGridView6.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new SqlCommand("select * from vwStockIn where sdate between '" + dt7.Value.ToString("dd-MM-yyyy") + "'and'" + dt8.Value.ToString("dd-MM-yyyy") + "' and status like 'Done'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView6.Rows.Add(i, dr["id"].ToString(), dr["refno"].ToString(), dr["pcode"].ToString(), dr["pdescription"].ToString(), dr["qty"].ToString(), dr["sdate"].ToString(), dr["stockinby"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            if (cboTopSelect.Text == "SORT BY QTY")
            {
                frm.LoadTopSelling("select top 10  pcode, pdescription, isnull(sum(qty),0) as qty  from vwSoldItems where sdate between '" + dt1.Value.ToString("dd-MM-yyyy") + "'and '" + dt2.Value.ToString("dd-MM-yyyy") + "'and status like 'Sold' group by pcode, pdescription order by qty desc", "From-:" + dt1.Value.ToString("dd-MM-yyyy") + "-To:-" + dt2.Value.ToString("dd-MM-yyyy"), "TOP SELLING ITEMS SORT BY QTY");
            }
            else if (cboTopSelect.Text == "SORT BY TOTAL AMOUNT")
            {

                frm.LoadTopSelling("select top 10  pcode, pdescription, isnull(sum(qty),0) as qty, isnull(sum(total),0) as total  from vwSoldItems where sdate between '" + dt1.Value.ToString("dd-MM-yyyy") + "'and '" + dt2.Value.ToString("dd-MM-yyyy") + "'and status like 'Sold' group by pcode, pdescription order by total desc", "From-:" + dt1.Value.ToString("dd-MM-yyyy") + "-To:-" + dt2.Value.ToString("dd-MM-yyyy"), "TOP SELLING ITEMS SORT BY TOTAL AMOUNT");
            }



            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            frm.LoadSoldItems("select c.pcode, p.pdescription, c.price, sum(c.qty) as tot_qty, sum(c.discount) as tot_discount, sum(c.total) as total from tblCart as c inner join tblProduct as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dt3.Value.ToString("dd-MM-yyyy") + "'and '" + dt4.Value.ToString("dd-MM-yyyy") + "'group by c.pcode, p.pdescription, c.price", "From-:" + dt1.Value.ToString("dd-MM-yyyy") + "-To:-" + dt2.Value.ToString("dd-MM-yyyy"));
            frm.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(cboTopSelect.Text == String.Empty)
            {
                MessageBox.Show("Please select from the dropdown list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            LoadTopSelling();
            LoadChartTopSelling();
        }

        public void LoadChartTopSelling()
        {
           SqlDataAdapter da = new SqlDataAdapter();
            cn.Open();
            if (cboTopSelect.Text == "SORT BY QTY")
            {
                 da = new SqlDataAdapter("select top 10  pcode, isnull(sum(qty),0) as qty  from vwSoldItems where sdate between '" + dt1.Value.ToString("dd-MM-yyyy") + "'and '" + dt2.Value.ToString("dd-MM-yyyy") + "'and status like 'Sold' group by pcode order by qty desc", cn);
            }
            else if (cboTopSelect.Text == "SORT BY TOTAL AMOUNT")
            {
               da = new SqlDataAdapter("select top 10  pcode, isnull(sum(total),0) as total  from vwSoldItems where sdate between '" + dt1.Value.ToString("dd-MM-yyyy") + "'and '" + dt2.Value.ToString("dd-MM-yyyy") + "'and status like 'Sold' group by pcode order by total desc", cn);
            }
            DataSet ds = new DataSet();
            da.Fill(ds, "TOPSELLING");
            chart1.DataSource = ds.Tables["TOPSELLING"];
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Doughnut;

            series.Name = "TOP SELLING";
            var cahrt = chart1;
            chart1.Series[0].XValueMember = "pcode";
            if (cboTopSelect.Text == "SORT BY QTY")
            {
                chart1.Series[0].YValueMembers = "qty";
            }
            if (cboTopSelect.Text == "SORT BY TOTAL AMOUNT")
            {
                chart1.Series[0].YValueMembers = "total";
            }
            chart1.Series[0].IsValueShownAsLabel = true;
            if (cboTopSelect.Text == "SORT BY TOTAL AMOUNT")
            {
                chart1.Series[0].LabelFormat = "{#,##0.00}";
            }
            if (cboTopSelect.Text == "SORT BY QTY")
            {
                chart1.Series[0].LabelFormat = "{#,##0}";
            }
            cn.Close();
        }
        private void cboTopSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            string param = "Date Covered:" +dt7.Value.ToString("dd-MM-yyyy") + " - " + dt8.Value.ToString("dd-MM-yyyy");
            frm.LoadStockIn("select * from vwStockIn where sdate between '" + dt7.Value.ToString("dd-MM-yyyy") + "'and'" + dt8.Value.ToString("dd-MM-yyyy") + "' and status like 'Done'", param);
            frm.Show();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadStockInHistory();
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadCancelledOrder();
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport fm = new frmInventoryReport();
            string param = "Date Covered:" + dt5.Value.ToString("dd-MM-yyyy") + " - " + dt6.Value.ToString("dd-MM-yyyy");
            fm.LoadSCancelledOrder("select * from vwCancelledOrder where sdate between '" + dt5.Value.ToString("dd-MM-yyyy") + "'and'" + dt6.Value.ToString("dd-MM-yyyy") + "'", param);
            fm.Show();
        }

        private void dt1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
