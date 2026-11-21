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
                string flightNum = Request.QueryString["flight"];
                if (!string.IsNullOrEmpty(flightNum))
                {
                    lblFlightNum.Text = Server.HtmlEncode(flightNum);

                    // בהזמנת הלוך-חזור מציגים גם את טיסת החזור
                    string returnFlightNum = Request.QueryString["returnFlight"];
                    if (!string.IsNullOrEmpty(returnFlightNum))
                    {
                        lblFlightNum.Text += " | טיסת חזור: " + Server.HtmlEncode(returnFlightNum);
                    }
                }
                else
                {
                    lblFlightNum.Text = "No flight selected";
                }
            }
        }

        protected void BtnSelectClass_Click(object sender, EventArgs e)
        {
            // בדיקה האם המשתמש מחובר למערכת (דף ההתחברות שומר את המשתמש ב-LoggedUser)
            if (Session["LoggedUser"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            Button btn = (Button)sender;
            string selectedClass = btn.CommandArgument;

            // מעבירים הלאה את כל פרטי הטיסה (כולל טיסת חזור ומספר נוסעים) לדף ההזמנה
            string url = "BookFlight.aspx?flight=" + Server.UrlEncode(Request.QueryString["flight"]) +
                "&flightId=" + Server.UrlEncode(Request.QueryString["flightId"]) +
                "&passengers=" + Server.UrlEncode(Request.QueryString["passengers"]) +
                "&class=" + Server.UrlEncode(selectedClass);

            if (!string.IsNullOrEmpty(Request.QueryString["returnFlightId"]))
            {
                url += "&returnFlight=" + Server.UrlEncode(Request.QueryString["returnFlight"]) +
                    "&returnFlightId=" + Server.UrlEncode(Request.QueryString["returnFlightId"]);
            }

            Response.Redirect(url);
        }
    }
}