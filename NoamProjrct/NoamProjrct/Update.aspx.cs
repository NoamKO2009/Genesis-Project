using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoamProjrct
{
    // דף עדכון פרטים: שינוי שם משתמש, סיסמה ואימייל
    public partial class Update : System.Web.UI.Page
    {
        // ===== Page_Load =====
        // מה עושה: ממלא את שדות "הפרטים הקיימים" (שם משתמש, סיסמה, אימייל) של המשתמש המחובר.
        // מאיפה המידע: שם המשתמש מ-Session["LoggedUser"], והפרטים מטבלת Users.
        // מטרה: שהמשתמש יראה את הפרטים הנוכחיים שלו לפני שהוא משנה אותם.
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

                    // שולפים את המשתמש הנוכחי שמחובר לפי השם ששמרנו ב-Session
                    string sqlstring = "SELECT * FROM Users WHERE Username = '" + loggedUser + "'";
                    OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);

                    OleDbDataReader Dr = cmd.ExecuteReader();

                    if (Dr.Read()) // אם נמצאה רשומה למשתמש
                    {
                        // מכניסים את הערכים הקיימים לתיבות הטקסט המיועדות לזיהוי / הצגה
                        up4.Text = Dr["Username"].ToString();
                        up5.Text = Dr["MyPassword"].ToString();

                        // האימייל הקיים מוצג בתיבה של הפרטים הקיימים (העמודה במסד נקראת MyEmail)
                        upOldEmail.Text = Dr["MyEmail"].ToString();
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

        // ===== Updatebutton =====
        // מה עושה: מעדכנת בטבלת Users את שם המשתמש, הסיסמה והאימייל. שדה חדש שנשאר ריק - נשאר הערך הקיים.
        // מאיפה המידע: שדות הפרטים הקיימים (לזיהוי) ושדות הפרטים החדשים בטופס.
        // מטרה: לשמור את הפרטים החדשים, ולעדכן את Session["LoggedUser"] אם שם המשתמש השתנה.
        protected void Updatebutton(object sender, EventArgs e)
        {
            OleDbConnection Con1 = new OleDbConnection();
            Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
            Con1.Open();

            // שדה חדש שנשאר ריק - משאירים את הערך הקיים
            string newUsername = up1.Text;
            if (newUsername == "")
            {
                newUsername = up4.Text;
            }

            string newPassword = up2.Text;
            if (newPassword == "")
            {
                newPassword = up5.Text;
            }

            string newEmail = upEmail.Text;
            if (newEmail == "")
            {
                newEmail = upOldEmail.Text;
            }

            // עדכון שם משתמש, אימייל וסיסמה במסד הנתונים
            string sqlstring = "UPDATE Users SET UserName = '" + newUsername + "', MyEmail = '" + newEmail + "', MyPassword = '" + newPassword + "' WHERE UserName = '" + up4.Text + "' AND MyPassword = '" + up5.Text + "';";

            OleDbCommand cmd = new OleDbCommand(sqlstring, Con1);
            int y = cmd.ExecuteNonQuery();

            Con1.Close();

            if (y > 0)
            {
                // אם העדכון הצליח, נעדכן גם את ה-Session לשם החדש
                Session["LoggedUser"] = newUsername;

                // הודעת הצלחה ומעבר לדף הבית
                Response.Write("<script>alert('הפרטים (שם משתמש, אימייל וסיסמה) עודכנו בהצלחה!'); window.location='HomePage.aspx';</script>");
            }
            else
            {
                Response.Write("<script>alert('שגיאה: פרטי הזיהוי הנוכחיים שגויים או שהעדכון נכשל');</script>");
            }
        }
    }
}