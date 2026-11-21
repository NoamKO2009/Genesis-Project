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

        // בדיקה האם המשתמש המחובר הוא מנהל
        public bool IsUserAdmin()
        {
            if (Session["LoggedUser"] == null)
            {
                return false;
            }

            string username = Session["LoggedUser"].ToString();

            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            string sql = "SELECT MyIsAdmin FROM Users WHERE Username = '" + username + "';";
            OleDbCommand cmd = new OleDbCommand(sql, Con1);
            OleDbDataReader Dr = cmd.ExecuteReader();

            // העמודה היא טקסט: מנהל מסומן "True". משתמש חדש נשמר עם "0" ולכן הוא לא מנהל
            bool isAdmin = false;
            if (Dr.Read())
            {
                if (Dr["MyIsAdmin"].ToString() == "True")
                {
                    isAdmin = true;
                }
            }

            Dr.Close();
            Con1.Close();

            return isAdmin;
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

            // בדיקה שכל פרטי החיפוש מולאו - אם לא, מציגים הודעה ולא מחפשים
            string error = CheckSearch(origin, destination, depDate, retDate, passengers);
            if (error != "")
            {
                lblSearchError.Text = error;
                lblSearchError.Visible = true;
                return;
            }

            // שמירת פרטי החיפוש ב-Session (התאריכים נשמרים בפורמט ישראלי כמו במסד הנתונים)
            Session["Origin"] = origin;
            Session["Destination"] = destination;
            Session["DepDate"] = Convert.ToDateTime(depDate).ToString("dd/MM/yyyy");
            Session["RetDate"] = Convert.ToDateTime(retDate).ToString("dd/MM/yyyy");
            Session["Passengers"] = Convert.ToInt32(passengers);

            // חיפוש חדש - מוחקים טיסות שנבחרו בחיפוש קודם
            Session["FlightId"] = null;
            Session["FlightNumber"] = null;
            Session["ReturnFlightId"] = null;
            Session["ReturnFlightNumber"] = null;

            Response.Redirect("Flights.aspx");
        }

        // בדיקת פרטי החיפוש. מחזירה הודעת שגיאה, או טקסט ריק אם הכול תקין
        private string CheckSearch(string origin, string destination, string depDate, string retDate, string passengers)
        {
            if (origin == "0")
            {
                return "אנא בחר מוצא.";
            }
            if (destination == "0")
            {
                return "אנא בחר יעד.";
            }
            if (origin == destination)
            {
                return "המוצא והיעד לא יכולים להיות אותה עיר.";
            }
            if (depDate == "")
            {
                return "אנא בחר תאריך יציאה.";
            }
            if (retDate == "")
            {
                return "אנא בחר תאריך חזרה.";
            }
            if (Convert.ToDateTime(retDate) < Convert.ToDateTime(depDate))
            {
                return "תאריך החזרה חייב להיות אחרי תאריך היציאה.";
            }
            if (!IsNumber(passengers))
            {
                return "אנא הזן מספר נוסעים (מספר בלבד).";
            }
            if (passengers.Length > 3)
            {
                return "מספר הנוסעים גדול מדי.";
            }
            if (Convert.ToInt32(passengers) < 1)
            {
                return "מספר הנוסעים חייב להיות לפחות 1.";
            }

            return "";
        }

        // בדיקה שהטקסט מכיל רק ספרות
        private bool IsNumber(string text)
        {
            if (text == "")
            {
                return false;
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] < '0' || text[i] > '9')
                {
                    return false;
                }
            }

            return true;
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
