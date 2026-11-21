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
            // רק משתמש מחובר יכול להזמין טיסה
            if (Session["LoggedUser"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // שם המשתמש נלקח מההתחברות ולא מוקלד ידנית
                txtUsername.Text = Session["LoggedUser"].ToString();
                txtUsername.ReadOnly = true;

                string flightId = Request.QueryString["flightId"];
                string returnFlightId = Request.QueryString["returnFlightId"];

                if (string.IsNullOrEmpty(flightId))
                {
                    lblError.Text = "שגיאה: לא נבחרה טיסה להזמנה.";
                    lblError.Visible = true;
                    BtnConfirmBooking.Enabled = false;
                    return;
                }

                OleDbConnection Con1 = new OleDbConnection();
                Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
                Con1.Open();

                // פרטי טיסת ההלוך
                string details = GetFlightDetails(Con1, flightId);

                // פרטי טיסת החזור (אם נבחרה)
                if (!string.IsNullOrEmpty(returnFlightId))
                {
                    details = details + "<br />טיסת חזור: " + GetFlightDetails(Con1, returnFlightId);
                }

                Con1.Close();

                lblFlightDetails.Text = details + "<br />מספר נוסעים: <b>" + GetPassengers() + "</b>";
            }
        }

        // מחזיר טקסט עם פרטי הטיסה לפי המזהה שלה
        private string GetFlightDetails(OleDbConnection Con1, string flightId)
        {
            string sql = "SELECT * FROM Flights WHERE [מזהה] = " + flightId;
            OleDbCommand cmd = new OleDbCommand(sql, Con1);
            OleDbDataReader Dr = cmd.ExecuteReader();

            string details = "";
            if (Dr.Read())
            {
                details = "מספר טיסה: <b>" + Dr["FlightNumber"].ToString() + "</b>" +
                    " | מוצא: <b>" + Dr["Origin"].ToString() + "</b>" +
                    " | יעד: <b>" + Dr["Destination"].ToString() + "</b>" +
                    " | תאריך: <b>" + Dr["FlightDate"].ToString() + "</b> (" + Dr["FlightTime"].ToString() + ")" +
                    " | מחיר: <b>$" + Dr["Price"].ToString() + "</b>" +
                    " | מקומות פנויים: <b>" + Dr["AvailableSeats"].ToString() + "</b>";
            }

            Dr.Close();
            return details;
        }

        // מספר הנוסעים מהחיפוש (אם לא הוזן מספר - נוסע אחד)
        private int GetPassengers()
        {
            string passengersText = Request.QueryString["passengers"];
            if (string.IsNullOrEmpty(passengersText))
            {
                return 1;
            }

            int passengers = Convert.ToInt32(passengersText);
            if (passengers < 1)
            {
                return 1;
            }
            return passengers;
        }

        // בדיקה האם יש בטיסה מספיק מקומות פנויים
        private bool HasEnoughSeats(OleDbConnection Con1, string flightId, int passengers)
        {
            string sql = "SELECT AvailableSeats FROM Flights WHERE [מזהה] = " + flightId;
            OleDbCommand cmd = new OleDbCommand(sql, Con1);
            int availableSeats = Convert.ToInt32(cmd.ExecuteScalar());

            if (availableSeats >= passengers)
            {
                return true;
            }
            return false;
        }

        // בונוס נקודות לפי המחלקה (כמו שכתוב בדף בחירת המחלקה)
        private int GetClassBonus(string flightClass)
        {
            if (flightClass == "Economy")
            {
                return 2000;
            }
            if (flightClass == "Premium Economy")
            {
                return 3000;
            }
            if (flightClass == "Business")
            {
                return 5000;
            }
            if (flightClass == "First Class")
            {
                return 8000;
            }
            return 0;
        }

        // חישוב הנקודות להזמנה: כל דולר ששולם = 5 נקודות, ועוד בונוס המחלקה
        private int CalculatePoints(int price, int passengers, string flightClass)
        {
            int totalPrice = price * passengers;
            int pricePoints = totalPrice * 5;
            int classBonus = GetClassBonus(flightClass);
            return pricePoints + classBonus;
        }

        // הזמנת טיסה אחת: הורדת מקומות + שמירת ההזמנה. מחזיר את כמות הנקודות שנצברו
        private int BookOneFlight(OleDbConnection Con1, string flightId, int passengers, string flightClass)
        {
            // שליפת מספר הטיסה והמחיר שלה
            string sqlFlight = "SELECT FlightNumber, Price FROM Flights WHERE [מזהה] = " + flightId;
            OleDbCommand cmdFlight = new OleDbCommand(sqlFlight, Con1);
            OleDbDataReader Dr = cmdFlight.ExecuteReader();
            Dr.Read();
            string flightNumber = Dr["FlightNumber"].ToString();
            int price = Convert.ToInt32(Dr["Price"]);
            Dr.Close();

            // הורדת מספר הנוסעים מהמקומות הפנויים
            string sqlSeats = "UPDATE Flights SET AvailableSeats = AvailableSeats - " + passengers + " WHERE [מזהה] = " + flightId;
            OleDbCommand cmdSeats = new OleDbCommand(sqlSeats, Con1);
            cmdSeats.ExecuteNonQuery();

            // חישוב הנקודות שהמשתמש צבר בהזמנה הזו
            int points = CalculatePoints(price, passengers, flightClass);

            // שמירת ההזמנה (כולל מספר נוסעים ונקודות)
            string username = Session["LoggedUser"].ToString();
            string bookingDate = DateTime.Now.ToString("dd/MM/yyyy");

            string sqlInsert = "INSERT INTO Bookings (MyUsername, FlightNumber, PassengerName, PassengerPhone, BookingDate, FlightClass, Passengers, Points) VALUES ('" +
                username + "', '" +
                flightNumber + "', '" +
                txtPassengerName.Text + "', '" +
                txtPassengerPhone.Text + "', '" +
                bookingDate + "', '" +
                flightClass + "', " +
                passengers + ", " +
                points + ")";
            OleDbCommand cmdInsert = new OleDbCommand(sqlInsert, Con1);
            cmdInsert.ExecuteNonQuery();

            return points;
        }

        // הוספת נקודות למשתמש בטבלת Users
        private void AddPointsToUser(OleDbConnection Con1, int points)
        {
            string username = Session["LoggedUser"].ToString();

            // שליפת הנקודות הנוכחיות של המשתמש
            string sqlGet = "SELECT MyElitePoints FROM Users WHERE Username = '" + username + "'";
            OleDbCommand cmdGet = new OleDbCommand(sqlGet, Con1);
            string currentText = cmdGet.ExecuteScalar().ToString();

            int currentPoints = 0;
            if (currentText != "")
            {
                currentPoints = Convert.ToInt32(currentText);
            }

            int newPoints = currentPoints + points;

            // שמירת הסכום החדש
            string sqlUpdate = "UPDATE Users SET MyElitePoints = '" + newPoints + "' WHERE Username = '" + username + "'";
            OleDbCommand cmdUpdate = new OleDbCommand(sqlUpdate, Con1);
            cmdUpdate.ExecuteNonQuery();
        }

        protected void BtnConfirmBooking_Click(object sender, EventArgs e)
        {
            string flightId = Request.QueryString["flightId"];
            string returnFlightId = Request.QueryString["returnFlightId"];
            string flightClass = Request.QueryString["class"];
            int passengers = GetPassengers();

            // בדיקה שמולאו פרטי הנוסע
            if (txtPassengerName.Text == "" || txtPassengerPhone.Text == "")
            {
                lblError.Text = "אנא מלא שם נוסע ומספר טלפון.";
                lblError.Visible = true;
                return;
            }

            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            // בדיקה שיש מספיק מקומות בטיסת ההלוך
            if (!HasEnoughSeats(Con1, flightId, passengers))
            {
                Con1.Close();
                lblError.Text = "אין מספיק מקומות פנויים בטיסת ההלוך עבור " + passengers + " נוסעים.";
                lblError.Visible = true;
                return;
            }

            // בדיקה שיש מספיק מקומות בטיסת החזור (אם נבחרה)
            if (!string.IsNullOrEmpty(returnFlightId))
            {
                if (!HasEnoughSeats(Con1, returnFlightId, passengers))
                {
                    Con1.Close();
                    lblError.Text = "אין מספיק מקומות פנויים בטיסת החזור עבור " + passengers + " נוסעים.";
                    lblError.Visible = true;
                    return;
                }
            }

            // הזמנת טיסת ההלוך
            int totalPoints = BookOneFlight(Con1, flightId, passengers, flightClass);

            // הזמנת טיסת החזור (אם נבחרה)
            if (!string.IsNullOrEmpty(returnFlightId))
            {
                totalPoints = totalPoints + BookOneFlight(Con1, returnFlightId, passengers, flightClass);
            }

            // הוספת כל הנקודות לחשבון המשתמש
            AddPointsToUser(Con1, totalPoints);

            Con1.Close();

            // ההזמנה בוצעה - חזרה לדף הבית
            Response.Redirect("HomePage.aspx");
        }
    }
}
