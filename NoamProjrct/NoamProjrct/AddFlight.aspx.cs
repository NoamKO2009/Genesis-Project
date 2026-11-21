using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    public partial class AddFlight : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OleDbConnection Con1 = new OleDbConnection();
                Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
                Con1.Open();

                // טעינת מוצא (Origin)
                string sqlOrigin = "SELECT DISTINCT Origin FROM OriginDestination;";
                OleDbCommand cmdOrigin = new OleDbCommand(sqlOrigin, Con1);
                OleDbDataReader drOrigin = cmdOrigin.ExecuteReader();
                ddlOrigin.DataSource = drOrigin;
                ddlOrigin.DataTextField = "Origin";
                ddlOrigin.DataValueField = "Origin";
                ddlOrigin.DataBind();
                ddlOrigin.Items.Insert(0, new ListItem("בחר מוצא...", "0"));
                drOrigin.Close();

                // טעינת יעד (Destination)
                string sqlDest = "SELECT DISTINCT Destination FROM OriginDestination;";
                OleDbCommand cmdDest = new OleDbCommand(sqlDest, Con1);
                OleDbDataReader drDest = cmdDest.ExecuteReader();
                ddlDestination.DataSource = drDest;
                ddlDestination.DataTextField = "Destination";
                ddlDestination.DataValueField = "Destination";
                ddlDestination.DataBind();
                ddlDestination.Items.Insert(0, new ListItem("בחר יעד...", "0"));
                drDest.Close();

                // טעינת סוג מטוס (Aircraft)
                string sqlAircraft = "SELECT Aircraft FROM AircraftType;";
                OleDbCommand cmdAircraft = new OleDbCommand(sqlAircraft, Con1);
                OleDbDataReader drAircraft = cmdAircraft.ExecuteReader();
                txtAirplaneModel.DataSource = drAircraft;
                txtAirplaneModel.DataTextField = "Aircraft";
                txtAirplaneModel.DataValueField = "Aircraft";
                txtAirplaneModel.DataBind();
                txtAirplaneModel.Items.Insert(0, new ListItem("בחר מטוס...", "0"));
                drAircraft.Close();

                Con1.Close();
            }
        }

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            // בדיקה שכל השדות נבחרו ומולאו כראוי לפני שליחה למסד הנתונים
            if (ddlOrigin.SelectedValue == "0" || ddlDestination.SelectedValue == "0" || txtAirplaneModel.SelectedValue == "0" ||
                string.IsNullOrEmpty(txtFlightNumber.Text) || string.IsNullOrEmpty(txtTotalSeats.Text) || string.IsNullOrEmpty(txtPrice.Text))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "שגיאה: אנא מלא את כל השדות ובחר ערכים תקינים מהרשימות!";
                return;
            }

            // המרת התאריך לפורמט ישראלי (DD/MM/YYYY)
            string formattedDate = txtFlightDate.Text;
            if (DateTime.TryParse(txtFlightDate.Text, out DateTime parsedDate))
            {
                formattedDate = parsedDate.ToString("dd/MM/yyyy");
            }

            try
            {
                OleDbConnection Con1 = new OleDbConnection();
                Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
                Con1.Open();

                string query = "INSERT INTO Flights (FlightNumber, Origin, Destination, FlightDate, FlightTime, AirplaneModel, TotalSeats, AvailableSeats, Price) VALUES ('" +
                    txtFlightNumber.Text + "', '" +
                    ddlOrigin.SelectedValue + "', '" +
                    ddlDestination.SelectedValue + "', '" +
                    formattedDate + "', '" +
                    txtFlightTime.Text + "', '" +
                    txtAirplaneModel.SelectedValue + "', " +
                    txtTotalSeats.Text + ", " +
                    txtTotalSeats.Text + ", " +
                    txtPrice.Text + ")";

                OleDbCommand cmd = new OleDbCommand(query, Con1);
                cmd.ExecuteNonQuery();
                Con1.Close();

                lblMsg.ForeColor = System.Drawing.Color.FromArgb(74, 222, 128);
                lblMsg.Text = "הטיסה נוספה בהצלחה למסד הנתונים!";
            }
            catch (Exception ex)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "שגיאה בהוספת הטיסה: " + ex.Message;
            }
        }
    }
}