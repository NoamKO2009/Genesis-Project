using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    public partial class Update : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // בודקים שזו הטעינה הראשונה של הדף (ולא לחיצה על כפתור)
            if (!IsPostBack)
            {
                // מוודאים שהמשתמש אכן התחבר (שיש ערך ב-Session)
                if (Session["LoggedUser"] != null)
                {
                    string loggedUser = Session["LoggedUser"].ToString();

                    OleDbConnection Con1 = new OleDbConnection();
                    Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
                    Con1.Open();

                    // שולפים רק את המשתמש הנוכחי שמחובר לפי השם ששמרנו ב-Session
                    string sqlstring = "SELECT * FROM Users WHERE Username = '" + loggedUser + "'";
                    OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);

                    OleDbDataReader Dr = cmd.ExecuteReader();

                    if (Dr.Read()) // אם נמצאה רשומה למשתמש
                    {
                        // מכניסים את הערכים מתוך מסד הנתונים ישירות לתוך תיבות הטקסט בדף
                        up4.Text = Dr["Username"].ToString();
                        up5.Text = Dr["MyPassword"].ToString();

                        // הערה: ודא ששם עמודת הטלפון במסד הנתונים שלך תואם לשם כאן (למשל Phone או Tel)
                        // up6.Text = Dr["Phone"].ToString(); 
                    }

                    Dr.Close();
                    Con1.Close();
                }
                else
                {
                    // אם המשתמש לא עבר דרך הלוגין, נחזיר אותו בחזרה לדף ההתחברות
                    Response.Redirect("Login.aspx");
                }
            }
        }


        protected void Updatebutton(object sender, EventArgs e)
        {
            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            string sqlstring = "UPDATE Users SET UserName = '" + up1.Text + "', MyPassword = '" + up2.Text + "' WHERE UserName = '" + up4.Text + "' AND MyPassword = '" + up5.Text + "';";

            OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);
            int y = cmd.ExecuteNonQuery();

            Con1.Close();

            if (y > 0)
            {
                // אם שם המשתמש עודכן בהצלחה, נעדכן גם את ה-Session שלא ייזרק החוצה
                Session["LoggedUser"] = up1.Text;

                // הודעת הצלחה ומעבר לדף הבית
                Response.Write("<script>alert('הפרטים עודכנו בהצלחה!'); window.location='HomePage.aspx';</script>");
            }
            else
            {
                Response.Write("<שגיאה: פרטי הזיהוי הנוכחיים שגויים או שהעדכון נכשל>");
            }
        }
    }
}