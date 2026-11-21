using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    public partial class BookFlight : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string flightNum = Request.QueryString["flight"];

                if (string.IsNullOrEmpty(flightNum))
                {
                    lblError.Text = "שגיאה: לא נבחרה טיסה להזמנה.";
                    lblError.Visible = true;
                    BtnConfirmBooking.Enabled = false;
                    return;
                }

                // שליפת פרטי הטיסה ממסד הנתונים כדי להציג למשתמש
                OleDbConnection Con1 = new OleDbConnection();
                Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
                Con1.Open();

                string query = "SELECT * FROM Flights WHERE FlightNumber = '" + flightNum + "'";
                OleDbCommand cmd = new OleDbCommand(query, Con1);
                OleDbDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string origin = dr["Origin"].ToString();
                    string dest = dr["Destination"].ToString();
                    string date = dr["FlightDate"].ToString();
                    string time = dr["FlightTime"].ToString();
                    string price = dr["Price"].ToString();

                    lblFlightDetails.Text = "מספר טיסה: <b>" + flightNum + "</b> | מוצא: <b>" + origin + "</b> | יעד: <b>" + dest + "</b> | תאריך: <b>" + date + "</b> (" + time + ") | מחיר: <b>$" + price + "</b>";
                }
                else
                {
                    lblError.Text = "הטיסה המבוקשת לא נמצאה במערכת.";
                    lblError.Visible = true;
                    BtnConfirmBooking.Enabled = false;
                }

                dr.Close();
                Con1.Close();
            }
        }

        protected void BtnConfirmBooking_Click(object sender, EventArgs e)
        {
            string flightNum = Request.QueryString["flight"];
            string flightClass = Request.QueryString["class"];
            string username = txtUsername.Text;
            string passengerName = txtPassengerName.Text;
            string passengerPhone = txtPassengerPhone.Text;
            string bookingDate = DateTime.Now.ToString("dd/MM/yyyy");

            // פתיחת חיבור למסד הנתונים
            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            // בניית שאילתת ההוספה עם משתנים ישירים
            string insertQuery = "INSERT INTO Bookings (MyUsername, FlightNumber, PassengerName, PassengerPhone, BookingDate, FlightClass) VALUES ('" + username + "', '" + flightNum + "', '" + passengerName + "', '" + passengerPhone + "', '" + bookingDate + "', '" + flightClass + "')";

            OleDbCommand cmdInsert = new OleDbCommand(insertQuery, Con1);

            // הרצת הפקודה שמכניסה את הנתונים למסד הנתונים
            cmdInsert.ExecuteNonQuery();

            // סגירת החיבור
            Con1.Close();

            // הצגת הודעת הצלחה
            lblSuccess.Text = "ההזמנה בוצעה בהצלחה!";
            Response.Redirect("HomePage.aspx");
            lblSuccess.Visible = true;
            BtnConfirmBooking.Enabled = false;
        }
    }
}