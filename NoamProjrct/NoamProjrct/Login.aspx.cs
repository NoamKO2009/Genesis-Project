using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            string query = "SELECT COUNT(*) FROM Users WHERE Username = '" + txtUsername.Text + "' AND MyPassword = '" + txtPassword.Text + "'";

            OleDbConnection Con1 = new OleDbConnection(connString);
            Con1.Open();

            OleDbCommand cmd = new OleDbCommand(query, Con1);
            int count = (int)cmd.ExecuteScalar();

            Con1.Close();

            if (count > 0)
            {
                Session["LoggedUser"] = txtUsername.Text;
                Response.Redirect("HomePage.aspx");
            }
            else
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "שם משתמש או סיסמה שגויים!";
            }
        }

        protected void ReturnToHompage_Click(object sender, EventArgs e)
        {
            Response.Redirect("HomePage.aspx");
        }
    }
}