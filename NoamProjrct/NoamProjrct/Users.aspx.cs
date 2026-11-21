using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    // דף ניהול (למנהל): הצגת המשתמשים וסינון לפי רמת Elite
    public partial class Users : System.Web.UI.Page
    {
        // ===== Page_Load =====
        // מה עושה: ממלא את רשימת רמות ה-Elite לסינון.
        // מאיפה המידע: טבלת MyEliteTier במסד.
        // מטרה: שהמנהל יוכל לבחור רמה ולראות רק את המשתמשים שלה.
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OleDbConnection Con1 = new OleDbConnection();

                Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
                Con1.Open();

                string sqlstring = "SELECT MyEliteTier FROM MyEliteTier ORDER BY MyEliteTier ASC;";
                OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);
                OleDbDataReader Dr = cmd.ExecuteReader();

                ShowTier.DataSource = Dr;
                ShowTier.DataTextField = "MyEliteTier";
                ShowTier.DataValueField = "MyEliteTier";
                ShowTier.DataBind();

                Dr.Close();
                Con1.Close();
            }
        }

        // ===== Button1_Click =====
        // מה עושה: מציגה בטבלה את כל המשתמשים.
        // מאיפה המידע: טבלת Users במסד.
        // מטרה: כפתור "רענן נתונים" - לראות את כל המשתמשים בלי סינון.
        protected void Button1_Click(object sender, EventArgs e)
        {
            OleDbConnection Con1 = new OleDbConnection();

            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            string sqlstring = "SELECT * FROM Users;";
            OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);
            OleDbDataReader Dr = cmd.ExecuteReader();

            GridView1.DataSource = Dr;
            GridView1.DataBind();

            Dr.Close();
            Con1.Close();
        }

        // ===== ShowCities_OnSelectedIndexChanged =====
        // מה עושה: מציגה רק את המשתמשים ברמת ה-Elite שנבחרה ברשימה.
        // מאיפה המידע: הרמה שנבחרה (ShowTier.SelectedValue) וטבלת Users.
        // מטרה: סינון משתמשים לפי רמה.
        protected void ShowCities_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbConnection Con1 = new OleDbConnection();

            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            string sqlstring = "SELECT * FROM Users WHERE MyEliteTier = '" + ShowTier.SelectedValue + "';";
            OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);

            OleDbDataReader Dr = cmd.ExecuteReader();

            GridView1.DataSource = Dr;
            GridView1.DataBind();

            Dr.Close();
            Con1.Close();
        }
    }
}