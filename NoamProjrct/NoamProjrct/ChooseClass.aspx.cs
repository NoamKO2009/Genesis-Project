using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    // דף בחירת מחלקה: Economy / Premium Economy / Business / First Class
    public partial class ChooseClass : System.Web.UI.Page
    {
        // ===== Page_Load =====
        // מה עושה: מציג בראש הדף את מספרי הטיסות שנבחרו (הלוך וחזור).
        // מאיפה המידע: Session["FlightNumber"] ו-Session["ReturnFlightNumber"] (נשמרו בדף הטיסות).
        // מטרה: שהמשתמש יראה לאילו טיסות הוא בוחר מחלקה.
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // הצגת הטיסות שנבחרו בדף הטיסות
                if (Session["FlightNumber"] != null && Session["ReturnFlightNumber"] != null)
                {
                    lblFlightNum.Text = Session["FlightNumber"].ToString() + " | טיסת חזור: " + Session["ReturnFlightNumber"].ToString();
                }
                else
                {
                    lblFlightNum.Text = "לא נבחרה טיסה";
                }
            }
        }

        // ===== SelectClass =====
        // מה עושה: שומרת את המחלקה שנבחרה ב-Session ועוברת לדף ההזמנה.
        // מאיפה המידע: מקבלת את שם המחלקה מאחד מכפתורי הבחירה (למשל BtnBusiness_Click).
        // מטרה: פונקציה אחת משותפת לכל ארבעת הכפתורים, כדי לא לכתוב את אותו קוד 4 פעמים.
        private void SelectClass(string flightClass)
        {
            // בדיקה האם המשתמש מחובר למערכת
            if (Session["LoggedUser"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            Session["FlightClass"] = flightClass;
            Response.Redirect("BookFlight.aspx");
        }

        // ===== כפתורי המחלקות =====
        // מה עושים: כל כפתור קורא ל-SelectClass עם שם המחלקה שלו.
        // מאיפה המידע: הלחיצה של המשתמש על הכפתור בדף.
        protected void BtnEconomy_Click(object sender, EventArgs e)
        {
            SelectClass("Economy");
        }

        protected void BtnPremium_Click(object sender, EventArgs e)
        {
            SelectClass("Premium Economy");
        }

        protected void BtnBusiness_Click(object sender, EventArgs e)
        {
            SelectClass("Business");
        }

        protected void BtnFirst_Click(object sender, EventArgs e)
        {
            SelectClass("First Class");
        }
    }
}
