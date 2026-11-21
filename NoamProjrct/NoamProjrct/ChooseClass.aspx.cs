using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    public partial class ChooseClass : System.Web.UI.Page
    {
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

        // שמירת המחלקה שנבחרה ומעבר לדף ההזמנה
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
