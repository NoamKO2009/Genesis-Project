using System;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LoggedUser"] != null)
                {
                    litWelcome.Text = "שלום, <b>" + Session["LoggedUser"].ToString() + "</b>.";
                    BtnLogin.Visible = false;
                    BtnRegister.Visible = false;
                    BtnLogout.Visible = true;
                    BtnUpdate.Visible = true;

                    // מציג את כפתור החיפוש ומסתיר את הודעת הדרישה להתחברות
                    BtnSearch.Visible = true;
                    pnlLoginRequired.Visible = false;

                    // הצגת כפתורי הניהול רק אם המשתמש הוא מנהל
                    BtnAdminShowUsers.Visible = IsUserAdmin();
                    BtnAdminPanel.Visible = IsUserAdmin();
                    BtnBookings.Visible = IsUserAdmin();

                    LoadUserStats();
                }
                else
                {
                    litWelcome.Text = "שלום אורח! אנא התחבר או הירשם למערכת.";
                    BtnLogin.Visible = true;
                    BtnRegister.Visible = true;
                    BtnLogout.Visible = false;
                    BtnUpdate.Visible = false;
                    BtnAdminPanel.Visible = false;
                    BtnAdminShowUsers.Visible = false;
                    BtnBookings.Visible = false;
                    pnlUserStats.Visible = false;

                    // מסתיר את כפתור החיפוש ומציג הודעה שעליך להתחבר
                    BtnSearch.Visible = false;
                    pnlLoginRequired.Visible = true;
                }

                LoadDropdowns();
            }
        }

        // שליפת רמת Elite ונקודות של המשתמש המחובר
        private void LoadUserStats()
        {
            string username = Session["LoggedUser"].ToString();

            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            string sql = "SELECT MyEliteTier, MyElitePoints FROM Users WHERE Username = '" + username + "';";
            OleDbCommand cmd = new OleDbCommand(sql, Con1);
            OleDbDataReader Dr = cmd.ExecuteReader();

            if (Dr.Read())
            {
                litTier.Text = Dr["MyEliteTier"].ToString();
                litPoints.Text = Dr["MyElitePoints"].ToString();
            }

            Dr.Close();
            Con1.Close();

            pnlUserStats.Visible = true;
        }

        public bool IsUserAdmin()
        {
            if (Session["LoggedUser"] == null)
                return false;

            string username = Session["LoggedUser"].ToString();
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";

            OleDbConnection Con1 = new OleDbConnection(connString);
            Con1.Open();

            string sqlAdmin = "SELECT MyIsAdmin FROM Users WHERE Username = '" + username + "';";
            OleDbCommand cmdAdmin = new OleDbCommand(sqlAdmin, Con1);

            object result = cmdAdmin.ExecuteScalar();
            Con1.Close();

            if (result != null && result != DBNull.Value)
            {
                return Convert.ToBoolean(result);
            }

            return false;
        }

        private void LoadDropdowns()
        {
            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            // מוצא
            string sqlstring1 = "SELECT DISTINCT Origin FROM OriginDestination;";
            OleDbCommand cmd = new OleDbCommand(sqlstring1, Con1);
            OleDbDataReader Dr = cmd.ExecuteReader();

            ddlOrigin.DataSource = Dr;
            ddlOrigin.DataTextField = "Origin";
            ddlOrigin.DataValueField = "Origin";
            ddlOrigin.DataBind();
            Dr.Close();
            ddlOrigin.Items.Insert(0, new ListItem("בחר מוצא...", "0"));

            // יעד
            string sqlstring2 = "SELECT DISTINCT Destination FROM OriginDestination;";
            OleDbCommand cmd1 = new OleDbCommand(sqlstring2, Con1);
            OleDbDataReader Dr1 = cmd1.ExecuteReader();

            ddlDestination.DataSource = Dr1;
            ddlDestination.DataTextField = "Destination";
            ddlDestination.DataValueField = "Destination";
            ddlDestination.DataBind();
            Dr1.Close();

            Con1.Close();

            ddlDestination.Items.Insert(0, new ListItem("בחר יעד...", "0"));
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            string origin = ddlOrigin.SelectedValue;
            string destination = ddlDestination.SelectedValue;
            string depDate = txtDepartureDate.Text;
            string retDate = txtReturnDate.Text;
            string passengers = txtPassengers.Text;

            // מעבר לדף הטיסות עם כל הפרמטרים
            Response.Redirect("Flights.aspx?origin=" + origin + "&destination=" + destination + "&depDate=" + depDate + "&retDate=" + retDate + "&passengers=" + passengers);
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void BtnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Insert.aspx");
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            Response.Redirect("Update.aspx");
        }

        protected void BtnAdminPanel_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddFlight.aspx");
        }

        protected void BtnAdminShowUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("Users.aspx");
        }
        protected void BtnBookings_Click(object sender, EventArgs e)
        {
            Response.Redirect("Bookings.aspx");
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("HomePage.aspx");
        }
    }
}