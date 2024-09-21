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

    public partial class frmSecurity : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        public string _pass,  _username="";
        public bool _isactive = false;
        public frmSecurity()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           if(MessageBox.Show("EXIT THE APPLICATION ?","CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
           {
               Application.Exit();
           }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string _role="", _name="";
            try
            {
                bool found = false;
                cn.Open();
                cm = new SqlCommand("select * from tblUser where username=@username and password=@password", cn);
                cm.Parameters.AddWithValue("@username", txtUsername.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Text); 
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    found = true;
                    _username = dr["username"].ToString();
                     _role = dr["role"].ToString();
                     _name = dr["name"].ToString();
                     _pass = dr["password"].ToString();
                     _isactive = bool.Parse(dr["isactive"].ToString());

                }else
                {
                    found = false;
                }
                dr.Close();
                cn.Close();
                if(found == true)
                {
                    if (_isactive == false)
                    {
                        MessageBox.Show("Account is inactive, Unable to login", "Inactive Account ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                        if (_role == "Cashier")
                        {
                            MessageBox.Show(" Welcome" + _name + "!", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtUsername.Clear();
                            txtPassword.Clear();
                            this.Hide();
                            frmPOS fm = new frmPOS(this);
                            fm.lblUser.Text = _username;
                            fm.lblName.Text = _name + "|" + _role;
                            fm.ShowDialog();
                       

                    }else
                    {
                        MessageBox.Show(" Welcome" + _name + "!", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtUsername.Clear();
                        txtPassword.Clear();
                        this.Hide();
                       Form1 fm = new Form1();
                       fm.lblName.Text = _name;
                       fm.lblUser.Text = _username;
                       fm.lblRole.Text = _role;
                       fm._pass = _pass;
                       fm._user = _username;
                       fm.LoadDashboard();
                       fm.ShowDialog();
                    }
                }else
                {
                    MessageBox.Show("Invalid username or password!", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
               
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
