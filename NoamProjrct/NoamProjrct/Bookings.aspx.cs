using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    public partial class Bookings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OleDbConnection Con1 = new OleDbConnection();
                Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
                Con1.Open();

                string sqlstring = "SELECT DISTINCT MyUsername FROM Bookings;";
                OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);
                OleDbDataReader Dr = cmd.ExecuteReader();

                ShowUsers.DataSource = Dr;
                ShowUsers.DataTextField = "MyUsername";
                ShowUsers.DataValueField = "MyUsername";
                ShowUsers.DataBind();

                Dr.Close();
                Con1.Close();
            }
        }

        protected void BtnShowAll_Click(object sender, EventArgs e)
        {
            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            string sqlstring = "SELECT * FROM Bookings;";
            OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);
            OleDbDataReader Dr = cmd.ExecuteReader();

            GridView1.DataSource = Dr;
            GridView1.DataBind();

            Dr.Close();
            Con1.Close();
        }

        protected void ShowUsers_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            string sqlstring = "SELECT * FROM Bookings WHERE MyUsername = '" + ShowUsers.SelectedValue + "';";
            OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);
            OleDbDataReader Dr = cmd.ExecuteReader();

            GridView1.DataSource = Dr;
            GridView1.DataBind();

            Dr.Close();
            Con1.Close();
        }
    }
}