using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    // דף תוצאות החיפוש: מציג טיסות הלוך וטיסות חזור, והמשתמש בוחר אחת מכל טבלה
    public partial class Flights : System.Web.UI.Page
    {
        // ===== Page_Load =====
        // מה עושה: טוען את טיסות ההלוך לטבלה העליונה ואת טיסות החזור לטבלה התחתונה.
        // מאיפה המידע: פרטי החיפוש מה-Session (נשמרו בדף הבית), והטיסות מטבלת Flights במסד.
        // מטרה: להציג למשתמש רק טיסות שמתאימות לחיפוש שלו.
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // רק משתמש מחובר יכול לחפש טיסות
                if (Session["LoggedUser"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                // אי אפשר להגיע לדף בלי חיפוש מדף הבית
                if (Session["Origin"] == null)
                {
                    Response.Redirect("HomePage.aspx");
                    return;
                }

                // פרטי החיפוש שנשמרו בדף הבית
                string origin = Session["Origin"].ToString();
                string destination = Session["Destination"].ToString();
                string depDate = Session["DepDate"].ToString();
                string retDate = Session["RetDate"].ToString();
                int passengers = Convert.ToInt32(Session["Passengers"]);

                OleDbConnection Con1 = new OleDbConnection();
                Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
                Con1.Open();

                // 1. טיסות הלוך: מהמוצא ליעד בתאריך היציאה, עם מספיק מקומות
                string sql = "SELECT * FROM Flights WHERE Origin = '" + origin + "' AND Destination = '" + destination +
                    "' AND FlightDate = '" + depDate + "' AND AvailableSeats >= " + passengers;
                OleDbCommand cmd = new OleDbCommand(sql, Con1);
                OleDbDataReader Dr = cmd.ExecuteReader();
                bool hasRows = Dr.HasRows;
                GridView1.DataSource = Dr;
                GridView1.DataBind();
                Dr.Close();

                // אם אין טיסה בתאריך הזה - מציגים את כל הטיסות בקו
                if (!hasRows)
                {
                    lblError.Text = "אין טיסות הלוך בתאריך זה. להלן כל הטיסות בקו:";
                    lblError.Visible = true;

                    string sqlAll = "SELECT * FROM Flights WHERE Origin = '" + origin + "' AND Destination = '" + destination + "'";
                    OleDbCommand cmdAll = new OleDbCommand(sqlAll, Con1);
                    OleDbDataReader DrAll = cmdAll.ExecuteReader();
                    GridView1.DataSource = DrAll;
                    GridView1.DataBind();
                    DrAll.Close();
                }

                // 2. טיסות חזור: המוצא והיעד מתהפכים, בתאריך החזרה
                pnlReturnFlights.Visible = true;

                string sqlReturn = "SELECT * FROM Flights WHERE Origin = '" + destination + "' AND Destination = '" + origin +
                    "' AND FlightDate = '" + retDate + "' AND AvailableSeats >= " + passengers;
                OleDbCommand cmdReturn = new OleDbCommand(sqlReturn, Con1);
                OleDbDataReader DrReturn = cmdReturn.ExecuteReader();
                bool hasReturnRows = DrReturn.HasRows;
                GridView2.DataSource = DrReturn;
                GridView2.DataBind();
                DrReturn.Close();

                // אם אין טיסת חזור בתאריך הזה - מציגים את כל הטיסות בקו החזרה
                if (!hasReturnRows)
                {
                    lblReturnError.Text = "אין טיסות חזור בתאריך זה. להלן כל הטיסות בקו החזרה:";
                    lblReturnError.Visible = true;

                    string sqlReturnAll = "SELECT * FROM Flights WHERE Origin = '" + destination + "' AND Destination = '" + origin + "'";
                    OleDbCommand cmdReturnAll = new OleDbCommand(sqlReturnAll, Con1);
                    OleDbDataReader DrReturnAll = cmdReturnAll.ExecuteReader();
                    GridView2.DataSource = DrReturnAll;
                    GridView2.DataBind();
                    DrReturnAll.Close();
                }

                Con1.Close();
            }
        }

        // ===== GetFlightId =====
        // מה עושה: מוצאת את המספר הייחודי (FlightID) של הטיסה שבשורה שנבחרה בטבלה.
        // מאיפה המידע: מהתאים של השורה (מספר טיסה, מוצא, יעד, תאריך, שעה) ומטבלת Flights במסד.
        // מטרה: מספר טיסה לבד לא מספיק (אותו מספר מופיע בכמה תאריכים), וצריך מזהה ייחודי
        //        כדי שבהזמנה יורדו מקומות רק בטיסה הנכונה.
        private string GetFlightId(GridViewRow row)
        {
            string flightNumber = row.Cells[0].Text;
            string origin = row.Cells[1].Text;
            string destination = row.Cells[2].Text;
            string flightDate = row.Cells[3].Text;
            string flightTime = row.Cells[4].Text;

            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            string sql = "SELECT FlightID FROM Flights WHERE FlightNumber = '" + flightNumber + "' AND Origin = '" + origin +
                "' AND Destination = '" + destination + "' AND FlightDate = '" + flightDate + "' AND FlightTime = '" + flightTime + "'";
            OleDbCommand cmd = new OleDbCommand(sql, Con1);
            string flightId = cmd.ExecuteScalar().ToString();

            Con1.Close();
            return flightId;
        }

        // ===== GridView1_SelectedIndexChanged =====
        // מה עושה: רצה כשהמשתמש לוחץ "בחר" בטבלת טיסות ההלוך.
        // מאיפה המידע: מהשורה שנבחרה בטבלה העליונה (GridView1.SelectedRow).
        // מטרה: לשמור ב-Session את טיסת ההלוך (מזהה ומספר טיסה) ולבקש לבחור טיסת חזור.
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;

            Session["FlightId"] = GetFlightId(row);
            Session["FlightNumber"] = row.Cells[0].Text;

            lblError.Text = "טיסת הלוך (" + row.Cells[0].Text + ") נבחרה בהצלחה! כעת בחר טיסת חזור מטבלת טיסות החזור למטה.";
            lblError.Visible = true;
        }

        // ===== GridView2_SelectedIndexChanged =====
        // מה עושה: רצה כשהמשתמש לוחץ "בחר" בטבלת טיסות החזור.
        // מאיפה המידע: מהשורה שנבחרה בטבלה התחתונה (GridView2.SelectedRow), ומה-Session (טיסת ההלוך).
        // מטרה: לוודא שכבר נבחרה טיסת הלוך, לשמור ב-Session את טיסת החזור ולעבור לבחירת מחלקה.
        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // קודם חייבים לבחור טיסת הלוך
            if (Session["FlightId"] == null)
            {
                lblReturnError.Text = "אנא בחר קודם טיסת הלוך מהטבלה העליונה לפני בחירת טיסת החזור!";
                lblReturnError.Visible = true;
                return;
            }

            GridViewRow row = GridView2.SelectedRow;

            Session["ReturnFlightId"] = GetFlightId(row);
            Session["ReturnFlightNumber"] = row.Cells[0].Text;

            // מעבר לבחירת מחלקה
            Response.Redirect("ChooseClass.aspx");
        }
    }
}
