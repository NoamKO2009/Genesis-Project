using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    // דף ההתחברות: בדיקת שם משתמש וסיסמה מול המסד
    public partial class Login : System.Web.UI.Page
    {
        // ===== Page_Load =====
        // מה עושה: כלום - הדף רק מציג טופס התחברות.
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // ===== BtnLogin_Click =====
        // מה עושה: סופרת כמה משתמשים יש עם שם המשתמש והסיסמה שהוקלדו. אם יש לפחות אחד - ההתחברות הצליחה.
        // מאיפה המידע: שדות הטופס (txtUsername, txtPassword) וטבלת Users במסד.
        // מטרה: לשמור את שם המשתמש ב-Session["LoggedUser"]. כל שאר הדפים בודקים את הערך הזה כדי לדעת מי מחובר.
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            string query = "SELECT COUNT(*) FROM Users WHERE Username = '" + txtUsername.Text + "' AND MyPassword = '" + txtPassword.Text + "'";

            OleDbConnection Con1 = new OleDbConnection(connString);
            Con1.Open();

            OleDbCommand cmd = new OleDbCommand(query, Con1);
            int count = Convert.ToInt32(cmd.ExecuteScalar());

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

        // ===== ReturnToHompage_Click =====
        // מה עושה: חזרה לדף הבית בלי להתחבר.
        protected void ReturnToHompage_Click(object sender, EventArgs e)
        {
            Response.Redirect("HomePage.aspx");
        }
    }
}