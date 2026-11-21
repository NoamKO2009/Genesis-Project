using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    public partial class Flights : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OleDbConnection Con1 = new OleDbConnection();
                Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
                Con1.Open();

                // קליטת הנתונים שהועברו מדף הבית
                string originQuery = Request.QueryString["origin"];
                string destQuery = Request.QueryString["destination"];
                string depDateQuery = Request.QueryString["depDate"];
                string retDateQuery = Request.QueryString["retDate"]; // תאריך חזרה
                string passengersQuery = Request.QueryString["passengers"];

                // 1. טיפול בטיסות הלוך (Departure Flights)
                if (!string.IsNullOrEmpty(originQuery) || !string.IsNullOrEmpty(destQuery) || !string.IsNullOrEmpty(depDateQuery))
                {
                    string query = "SELECT * FROM Flights WHERE 1=1";

                    if (!string.IsNullOrEmpty(originQuery) && originQuery != "0")
                    {
                        query += " AND Origin = '" + originQuery + "'";
                    }
                    if (!string.IsNullOrEmpty(destQuery) && destQuery != "0")
                    {
                        query += " AND Destination = '" + destQuery + "'";
                    }
                    if (!string.IsNullOrEmpty(depDateQuery))
                    {
                        if (DateTime.TryParse(depDateQuery, out DateTime parsedDate))
                        {
                            string formattedDate = parsedDate.ToString("dd/MM/yyyy");
                            query += " AND FlightDate = '" + formattedDate + "'";
                        }
                    }
                    if (!string.IsNullOrEmpty(passengersQuery) && int.TryParse(passengersQuery, out int passengersCount))
                    {
                        query += " AND AvailableSeats >= " + passengersCount;
                    }

                    OleDbCommand cmdSearch = new OleDbCommand(query, Con1);
                    OleDbDataReader drSearch = cmdSearch.ExecuteReader();

                    bool hasRows = drSearch.HasRows;

                    GridView1.DataSource = drSearch;
                    GridView1.DataBind();
                    drSearch.Close();

                    if (!hasRows)
                    {
                        lblError.Text = "אין טיסות הלוך בתאריכים אלו. להלן הטיסות הזמינות לפי המוצא והיעד שבחרת:";
                        lblError.Visible = true;

                        string fallbackQuery = "SELECT * FROM Flights WHERE 1=1";
                        if (!string.IsNullOrEmpty(originQuery) && originQuery != "0")
                        {
                            fallbackQuery += " AND Origin = '" + originQuery + "'";
                        }
                        if (!string.IsNullOrEmpty(destQuery) && destQuery != "0")
                        {
                            fallbackQuery += " AND Destination = '" + destQuery + "'";
                        }

                        OleDbCommand cmdFallback = new OleDbCommand(fallbackQuery, Con1);
                        OleDbDataReader drFallback = cmdFallback.ExecuteReader();

                        GridView1.DataSource = drFallback;
                        GridView1.DataBind();
                        drFallback.Close();
                    }
                }
                else
                {
                    string allQuery = "SELECT * FROM Flights;";
                    OleDbCommand cmdAll = new OleDbCommand(allQuery, Con1);
                    OleDbDataReader drAll = cmdAll.ExecuteReader();
                    GridView1.DataSource = drAll;
                    GridView1.DataBind();
                    drAll.Close();
                }

                // 2. טיפול בטיסות חזור (Return Flights) - היפוך מוצא ויעד + גיבוי אוטומטי
                if (!string.IsNullOrEmpty(retDateQuery))
                {
                    pnlReturnFlights.Visible = true;

                    string returnQuery = "SELECT * FROM Flights WHERE 1=1";

                    // בטיסת חזור, המוצא והיעד מתהפכים (היעד הופך למוצא, והמוצא הופך ליעד)
                    if (!string.IsNullOrEmpty(destQuery) && destQuery != "0")
                    {
                        returnQuery += " AND Origin = '" + destQuery + "'";
                    }
                    if (!string.IsNullOrEmpty(originQuery) && originQuery != "0")
                    {
                        returnQuery += " AND Destination = '" + originQuery + "'";
                    }
                    if (DateTime.TryParse(retDateQuery, out DateTime parsedRetDate))
                    {
                        string formattedRetDate = parsedRetDate.ToString("dd/MM/yyyy");
                        returnQuery += " AND FlightDate = '" + formattedRetDate + "'";
                    }
                    if (!string.IsNullOrEmpty(passengersQuery) && int.TryParse(passengersQuery, out int passCount))
                    {
                        returnQuery += " AND AvailableSeats >= " + passCount;
                    }

                    OleDbCommand cmdReturn = new OleDbCommand(returnQuery, Con1);
                    OleDbDataReader drReturn = cmdReturn.ExecuteReader();

                    bool hasReturnRows = drReturn.HasRows;

                    GridView2.DataSource = drReturn;
                    GridView2.DataBind();
                    drReturn.Close();

                    // אם לא נמצאה טיסה בדיוק בתאריך החזרה המבוקש, נציג את כל הטיסות בקו החזרה הזה כגיבוי
                    if (!hasReturnRows)
                    {
                        lblReturnError.Text = "אין טיסות חזור בתאריך זה. להלן כל הטיסות הזמינות בקו החזרה:";
                        lblReturnError.Visible = true;

                        string returnFallbackQuery = "SELECT * FROM Flights WHERE 1=1";
                        if (!string.IsNullOrEmpty(destQuery) && destQuery != "0")
                        {
                            returnFallbackQuery += " AND Origin = '" + destQuery + "'";
                        }
                        if (!string.IsNullOrEmpty(originQuery) && originQuery != "0")
                        {
                            returnFallbackQuery += " AND Destination = '" + originQuery + "'";
                        }

                        OleDbCommand cmdRetFallback = new OleDbCommand(returnFallbackQuery, Con1);
                        OleDbDataReader drRetFallback = cmdRetFallback.ExecuteReader();

                        GridView2.DataSource = drRetFallback;
                        GridView2.DataBind();
                        drRetFallback.Close();
                    }
                }

                Con1.Close();
            }
        }

        // כפתור להצגת כל הטיסות
        protected void Button1_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblReturnError.Visible = false;
            pnlReturnFlights.Visible = false;

            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            string sqlstring = "SELECT * FROM Flights;";
            OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);
            OleDbDataReader Dr = cmd.ExecuteReader();

            GridView1.DataSource = Dr;
            GridView1.DataBind();

            Dr.Close();
            Con1.Close();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "BookFlight")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView1.Rows[index];
                string flightNumber = row.Cells[0].Text;

                string retDateQuery = Request.QueryString["retDate"];

                // אם יש תאריך חזרה בחיפוש, זו הזמנת הלוך-חזור ויש לבחור גם חזור
                if (!string.IsNullOrEmpty(retDateQuery))
                {
                    Session["SelectedOutbound"] = flightNumber;
                    lblError.Text = "טיסת הלוך (" + flightNumber + ") נבחרה בהצלחה! כעת בחר טיסת חזור מטבלת טיסות החזור למטה.";
                    lblError.Visible = true;
                }
                else
                {
                    // חיפוש חד-כיווני רגיל - עובר ישר לבחירת מחלקה
                    Response.Redirect("ChooseClass.aspx?flight=" + flightNumber);
                }
            }
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "BookFlight")
            {
                // וידוא שהמשתמש בחר קודם טיסת הלוך
                if (Session["SelectedOutbound"] == null)
                {
                    lblReturnError.Text = "אנא בחר קודם טיסת הלוך מהטבלה העליונה לפני בחירת טיסת החזור!";
                    lblReturnError.Visible = true;
                    return;
                }

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView2.Rows[index];
                string returnFlightNumber = row.Cells[0].Text;
                string outboundFlightNumber = Session["SelectedOutbound"].ToString();

                // מעבר לדף בחירת המחלקה עם שתי הטיסות יחד (הלוך וחזור)
                Response.Redirect("ChooseClass.aspx?flight=" + outboundFlightNumber + "&returnFlight=" + returnFlightNumber);
            }
        }
    }
}