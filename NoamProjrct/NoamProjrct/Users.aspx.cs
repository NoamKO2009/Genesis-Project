using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    public partial class Users : System.Web.UI.Page
    {
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