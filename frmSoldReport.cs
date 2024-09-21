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
using Microsoft.Reporting.WinForms;
namespace AttehAgboEnterprise
{
    public partial class frmSoldReport : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        
        frmSoldItems fm;
        public frmSoldReport(frmSoldItems flist)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            fm = flist;
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmSoldReport_Load(object sender, EventArgs e)
        {

            
        }

       
        public void LoadReport()
        {
            try
            {
                ReportDataSource rptDS;
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptSoldReport.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                AtteyDataSet3 ds = new AtteyDataSet3();
                SqlDataAdapter da = new SqlDataAdapter();
                cn.Open();
                if (fm.cboCashier.Text == "All Cashier")
                {
                    da.SelectCommand = new SqlCommand("select c.id, c.transno, c.pcode, p.pdescription, c.price, c.qty, c.discount, c.total from tblCart as c inner join tblProduct as p on c.pcode=p.pcode where status like 'Sold' and sdate between'" + fm.dt1.Value.ToString("dd-MM-yyyy") + "' and'" + fm.dt2.Value.ToString("dd-MM-yyyy") + "'", cn);
                }
                else
                {
                   da.SelectCommand =new SqlCommand("select c.id, c.transno, c.pcode, p.pdescription, c.price, c.qty, c.discount, c.total from tblCart as c inner join tblProduct as p on c.pcode=p.pcode where status like 'Sold' and sdate between'" + fm.dt1.Value.ToString("dd-MM-yyyy") + "' and'" + fm.dt2.Value.ToString("dd-MM-yyyy") + "'and Cashier like '" + fm.cboCashier.Text + "'", cn);
                }
                da.Fill(ds.Tables["dtSoldReport"]);
                cn.Close();
                ReportParameter pDate = new ReportParameter("pDate", "Date From:" + fm.dt1.Value.ToString("dd-MM-yyyy")+ "To:" +fm.dt2.Value.ToString("dd-MM-yyyy"));
                ReportParameter pCashier = new ReportParameter("pCashier", "Cashier:" + fm.cboCashier.Text);
                ReportParameter pHeader = new ReportParameter("pHeader","SALES REPORT");
                reportViewer1.LocalReport.SetParameters(pDate);
                reportViewer1.LocalReport.SetParameters(pCashier);
                reportViewer1.LocalReport.SetParameters(pHeader);
                rptDS = new ReportDataSource("AtteyDataSet3", ds.Tables["dtSoldReport"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }
    }
}
