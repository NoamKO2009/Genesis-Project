using System;
using System.Data.OleDb;

namespace NoamProjrct
{
    // דף "צי החברה": מציג כרטיס לכל סוג מטוס מטבלת AircraftType, עם תמונה ונתונים
    public partial class OurFleet : System.Web.UI.Page
    {
        // ===== Page_Load =====
        // מה עושה: בכניסה הראשונה לדף טוענת את כל המטוסים.
        // מאיפה המידע: הפונקציה LoadFleet (שקוראת מטבלת AircraftType).
        // מטרה: שהמשתמש יראה את כל הצי כשהוא נכנס לדף.
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFleet();
            }
        }

        // ===== ddlMaker_SelectedIndexChanged =====
        // מה עושה: רצה כשבוחרים יצרן ברשימה (AutoPostBack) וטוענת את המטוסים מחדש.
        // מאיפה המידע: היצרן שנבחר ב-ddlMaker.
        // מטרה: סינון המטוסים לפי יצרן (Airbus / Boeing).
        protected void ddlMaker_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFleet();
        }

        // ===== LoadFleet =====
        // מה עושה: שולפת את סוגי המטוסים מהמסד ובונה כרטיס HTML לכל מטוס: תמונה, יצרן, שם, סוג גוף, מושבים וטווח.
        // מאיפה המידע: טבלת AircraftType (עמודות Aircraft, Manufacturer, BodyType, Seats, RangeKm).
        //              התמונה נמצאת בתיקייה Images/Fleet ושם הקובץ הוא שם הדגם (למשל A320.jpg).
        // מטרה: להציג את כל צי החברה מתוך הטבלה, כך שמטוס חדש בטבלה יופיע בדף אוטומטית.
        private void LoadFleet()
        {
            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            // אם נבחר יצרן - מציגים רק את המטוסים שלו
            string sql = "SELECT Aircraft, Manufacturer, BodyType, Seats, RangeKm FROM AircraftType";
            if (ddlMaker.SelectedValue != "0")
            {
                sql = sql + " WHERE Manufacturer = '" + ddlMaker.SelectedValue + "'";
            }
            sql = sql + " ORDER BY Manufacturer, Aircraft;";

            OleDbCommand cmd = new OleDbCommand(sql, Con1);
            OleDbDataReader Dr = cmd.ExecuteReader();

            string html = "";
            int count = 0;

            // לולאה על כל המטוסים - כל שורה בטבלה הופכת לכרטיס
            while (Dr.Read())
            {
                string model = Dr["Aircraft"].ToString();
                string maker = Dr["Manufacturer"].ToString();
                string body = Dr["BodyType"].ToString();
                string seats = Dr["Seats"].ToString();
                string range = Convert.ToInt32(Dr["RangeKm"]).ToString("N0");

                html = html + "<div class='plane-card'>";
                html = html + "<img src='Images/Fleet/" + model + ".jpg' alt='" + model + "' class='plane-img' />";
                html = html + "<div class='plane-info'>";
                html = html + "<span class='maker-badge'>" + maker + "</span>";
                html = html + "<div class='plane-name'>" + model + "</div>";
                html = html + "<div class='plane-body'>" + body + "</div>";
                html = html + "<div class='plane-specs'>";
                html = html + "<div class='spec-box'><span class='spec-value'>" + seats + "</span><span class='spec-label'>מושבים</span></div>";
                html = html + "<div class='spec-box'><span class='spec-value'>" + range + "</span><span class='spec-label'>טווח (ק\"מ)</span></div>";
                html = html + "</div>";
                html = html + "</div>";
                html = html + "</div>";

                count = count + 1;
            }

            Dr.Close();
            Con1.Close();

            litFleet.Text = html;
            litCount.Text = count.ToString();
        }
    }
}
