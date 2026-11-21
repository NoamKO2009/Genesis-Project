using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    public partial class Insert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Insertbutton(object sender, EventArgs e)
        {
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            string currentDate = DateTime.Now.ToString("dd/MM/yyyy");

            string sqlstring = "INSERT INTO users ([MyFirstName], [MyLastName], [Username], [MyEmail], [MyPassword], [MyPhone], [MyBirthDate], [MyRegistrationDate], [MyEliteTier], [MyElitePoints], [MyIsAdmin]) VALUES ('" +
                txtFirstName.Text + "', '" +
                txtLastName.Text + "', '" +
                txtUsername.Text + "', '" +
                txtEmail.Text + "', '" +
                txtPassword.Text + "', '" +
                txtPhone.Text + "', '" +
                txtBirthDate.Text + "', '" +
                currentDate + "', 'Bronze', 0, 0);";

            OleDbConnection Con1 = new OleDbConnection(connString);
            Con1.Open();

            OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);
            cmd.ExecuteNonQuery();
            Con1.Close();

            lblMsg.ForeColor = System.Drawing.Color.FromArgb(74, 222, 128);
            lblMsg.Text = "המשתמש נרשם בהצלחה!";
        }

        protected void ReturnToHompage_Click(object sender, EventArgs e)
        {
            Response.Redirect("HomePage.aspx");
        }
    }
}