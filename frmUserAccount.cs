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
    public partial class frmUserAccount : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        Form1 f;
        public frmUserAccount(Form1 fm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            LoadLoginDetails();
            this.f = fm;
        }

        private void frmUserAccount_Resize(object sender, EventArgs e)
        {
            tabControl1.Left = (this.Width - tabControl1.Width) / 2;
            tabControl1.Top = (this.Height - tabControl1.Height) / 2;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();

        }

        public void Clear()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtRetypePassword.Clear();
            cboRole.Text = "";
            txtName.Clear();
            txtUsername.Focus();
        }
        private void frmUserAccount_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               if(txtPassword.Text !=txtRetypePassword.Text)
               {
                   MessageBox.Show("Password did not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   return;
               }
               cn.Open();
               cm = new SqlCommand("insert into tblUser (username, password, role, name)values(@username, @password, @role, @name)", cn);
               cm.Parameters.AddWithValue("@username", txtUsername.Text);
               cm.Parameters.AddWithValue("@password", txtPassword.Text);
               cm.Parameters.AddWithValue("@role", cboRole.Text);
               cm.Parameters.AddWithValue("@name", txtName.Text);
               cm.ExecuteNonQuery();
               cn.Close();
               MessageBox.Show("New account saved!");
               Clear();
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtPOldUsername2.Text != f._pass)
                {
                     MessageBox.Show("Old password did not match!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return;
            
                }
                if (txtNewPassword2.Text != txtRetype2.Text)
                {
                    MessageBox.Show("Confirm new password", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
              
                cn.Open();
                cm = new SqlCommand("update tblUser set password=@password where username=@username", cn);
                cm.Parameters.AddWithValue("@password", txtNewPassword2.Text);
                cm.Parameters.AddWithValue("@username", txtUsername2.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Password has been successfully changed!", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                txtRetype2.Clear();
                txtNewPassword2.Clear();
                txtPOldUsername2.Clear();
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtUsername3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("select * from tblUser where username=@username", cn);
                cm.Parameters.AddWithValue("@username", txtUsername3.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    checkBox1.Checked = bool.Parse(dr["isactive"].ToString());
                }else
                {
                    checkBox1.Checked = false;
                }
                dr.Close();
                cn.Close();
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                bool found = true;

                cn.Open();
                cm = new SqlCommand("select * from tblUser where username=@username", cn);
                cm.Parameters.AddWithValue("@username", txtUsername3.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    found = true;
                }else
                {
                    found = false;
                }
                
                dr.Close();
                cn.Close();

                if (found == true)
                {


                    cn.Open();
                    cm = new SqlCommand("update tblUser set isactive=@isactive where username=@username", cn);
                    cm.Parameters.AddWithValue("@isactive", checkBox1.Checked.ToString());
                    cm.Parameters.AddWithValue("@username", txtUsername3.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Account status has been successfully updated.", "Update status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUsername3.Clear();
                    checkBox1.Checked = false;
                }else
                {
                    MessageBox.Show("Account doesn't exist", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         public void LoadLoginDetails()
        {
            int i = 0;
            cn.Open();
            cm = new SqlCommand("select * from tblUser", cn);
            dr = cm.ExecuteReader();
             while(dr.Read())
             {
                 i++;
                 dataGridView1.Rows.Add(dr["username"].ToString(), dr["password"].ToString(), dr["role"].ToString(), dr["name"].ToString(), dr["isactive"].ToString());
             }
             dr.Close();
            cn.Close();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
