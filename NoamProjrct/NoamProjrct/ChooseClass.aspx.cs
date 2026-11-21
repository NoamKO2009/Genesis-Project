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
                    lblFlightNum.Text = flightNum;
                }
                else
                {
                    lblFlightNum.Text = "No flight selected";
                }
            }
        }

        protected void BtnSelectClass_Click(object sender, EventArgs e)
        {
            // בדיקה האם המשתמש מחובר למערכת
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx"); // שום מעבר, נשלח יטפל בהתחברות
                return;
            }

            Button btn = (Button)sender;
            string selectedClass = btn.CommandArgument;
            string flightNum = Request.QueryString["flight"];

            Response.Redirect("BookFlight.aspx?flight=" + flightNum + "&class=" + Server.UrlEncode(selectedClass));
        }
    }
}