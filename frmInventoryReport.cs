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
    public partial class frmInventoryReport : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
       

        public frmInventoryReport()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void frmInventoryReport_Load(object sender, EventArgs e)
        {

            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadReport()
        {
            ReportDataSource rptDS;
            try
            {
            reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptInventory.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
            AtteyDataSet3 ds = new AtteyDataSet3();
            SqlDataAdapter da =  new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("select p.pcode, p.barcode, p.pdescription, b.brand, c.category, p.price, p.qty, p.reorder from tblProduct as p inner join tblBrand as b on p.bid = b.id inner join tblCategory as c on p.cid=c.id", cn);
                da.Fill(ds.Tables["dtInventory"]);

                cn.Close();
                rptDS = new ReportDataSource("AtteyDataSet3", ds.Tables["dtInventory"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent= 100;
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadTopSelling(string sql, string param, string header)
        {
            try
            {
                ReportDataSource rptD;
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptTopSelling.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                AtteyDataSet3 ds = new AtteyDataSet3();
                SqlDataAdapter da = new SqlDataAdapter();
                cn.Open();
                da.SelectCommand = new SqlCommand(sql, cn);
                da.Fill(ds.Tables["dtTopSelling"]);
                cn.Close();
                ReportParameter pHeader = new ReportParameter("pHeader", header);
                ReportParameter pDate = new ReportParameter("pDate", param);
                reportViewer1.LocalReport.SetParameters(pDate);

                reportViewer1.LocalReport.SetParameters(pHeader);
                rptD = new ReportDataSource("AtteyDataSet3", ds.Tables["dtTopSelling"]);
                reportViewer1.LocalReport.DataSources.Add(rptD);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadSoldItems(string sql, string param)
        {
            try
            {
                ReportDataSource rptD;
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptSoldItem.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                AtteyDataSet3 ds = new AtteyDataSet3();
                SqlDataAdapter da = new SqlDataAdapter();
                cn.Open();
                da.SelectCommand = new SqlCommand(sql, cn);
                da.Fill(ds.Tables["dtSoldItem"]);
                cn.Close();
                ReportParameter pDate = new ReportParameter("pDate", param);
                reportViewer1.LocalReport.SetParameters(pDate);

                rptD = new ReportDataSource("AtteyDataSet3", ds.Tables["dtSoldItem"]);
                reportViewer1.LocalReport.DataSources.Add(rptD);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadSCancelledOrder(string psql, string param)
        {
            ReportDataSource rptDS;
            try
            {
                reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptCancelled.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                AtteyDataSet3 ds = new AtteyDataSet3();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand(psql, cn);
                da.Fill(ds.Tables["dtCancelled"]);
                cn.Close();
                ReportParameter pDate = new ReportParameter("pDate", param);
                reportViewer1.LocalReport.SetParameters(pDate);
                rptDS = new ReportDataSource("AtteyDataSet3", ds.Tables["dtCancelled"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadStockIn(string psql, string param)
        {
            ReportDataSource rptDS;
            try
            {
                reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptStockIn.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                AtteyDataSet3 ds = new AtteyDataSet3();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand(psql, cn);
                da.Fill(ds.Tables["dtStockIn"]);
                cn.Close();
                ReportParameter pDate = new ReportParameter("pDate", param);
                reportViewer1.LocalReport.SetParameters(pDate);
                rptDS = new ReportDataSource("AtteyDataSet3", ds.Tables["dtStockIn"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

}
