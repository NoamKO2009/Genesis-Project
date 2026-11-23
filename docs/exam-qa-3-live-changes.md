# הכנה לבוחן – חלק 3: שינויים "חיים" שהבוחן יכול לבקש

> בבחינה הבוחן לפעמים אומר: "תוסיף/תשנה עכשיו X". כאן יש את המשימות הנפוצות, איפה עושים אותן, ואת הקוד – באותו סגנון בסיסי של הפרויקט.
> חלקים 1 ו-2: `exam-qa-1-design.md`, `exam-qa-2-code.md`.

---

## 0. "איפה זה בקוד?" – טבלה מהירה

| הבוחן שואל איפה... | קובץ | פונקציה |
|---|---|---|
| בודקים שם וסיסמה | `Login.aspx.cs` | פונקציית הכפתור – `SELECT COUNT(*)` |
| נרשם משתמש חדש | `Insert.aspx.cs` | `INSERT INTO Users` |
| בודקים את טופס החיפוש | `HomePage.aspx.cs` | `CheckSearch`, `IsNumber` |
| נשמר החיפוש | `HomePage.aspx.cs` | `BtnSearch_Click` (Session) |
| מוצגות הטיסות | `Flights.aspx.cs` | `Page_Load` |
| יודעים איזו טיסה נבחרה | `Flights.aspx.cs` | `GridView1_SelectedIndexChanged`, `GetFlightId` |
| נבחרת מחלקה | `ChooseClass.aspx.cs` | `SelectClass` |
| יורדים מקומות | `BookFlight.aspx.cs` | `BookOneFlight` – `UPDATE Flights` |
| מחושבות הנקודות | `BookFlight.aspx.cs` | `CalculatePoints`, `GetClassBonus` |
| הנקודות נוספות למשתמש | `BookFlight.aspx.cs` | `AddPointsToUser` |
| בודקים אם מנהל | `HomePage.aspx.cs` | `IsUserAdmin` |
| הטבלה מעוגלת | קובץ ה-aspx של הדף (בתוך `<style>`) | `.grid-view-style` – `border-radius` |

**טיפ:** ב-Visual Studio, `Ctrl+F` מחפש בקובץ, ו-`Ctrl+Shift+F` מחפש בכל הפרויקט.

---

## 1. "תשנה ש-1$ ייתן 10 נקודות במקום 5"
**איפה:** `BookFlight.aspx.cs` → `CalculatePoints`
```csharp
int pricePoints = totalPrice * 5;    // לפני
int pricePoints = totalPrice * 10;   // אחרי
```
וגם לשנות את הטקסט ב-`ChooseClass.aspx` אם כתוב שם "5 נקודות לדולר".

## 2. "תשנה את הבונוס של מחלקת עסקים"
**איפה:** `BookFlight.aspx.cs` → `GetClassBonus` – לשנות את המספר שליד `"Business"`.

---

## 3. "תוודא שהטלפון בהזמנה הוא 10 ספרות"
**איפה:** `BookFlight.aspx.cs` → `BtnConfirmBooking_Click`, אחרי הבדיקה שהשדות לא ריקים.
```csharp
if (txtPassengerPhone.Text.Length != 10)
{
    lblError.Text = "מספר הטלפון חייב להיות 10 ספרות.";
    lblError.Visible = true;
    return;
}
```
**אם רוצים גם לבדוק שרק ספרות** – להעתיק את `IsNumber` מ-`HomePage.aspx.cs` לדף, ולהוסיף:
```csharp
if (!IsNumber(txtPassengerPhone.Text))
{
    lblError.Text = "הטלפון חייב להכיל ספרות בלבד.";
    lblError.Visible = true;
    return;
}
```

## 4. "שלא יהיה אפשר לחפש תאריך שכבר עבר"
**איפה:** `HomePage.aspx.cs` → `CheckSearch`, אחרי הבדיקה שתאריך היציאה לא ריק.
```csharp
if (Convert.ToDateTime(depDate) < DateTime.Today)
{
    return "אי אפשר לחפש טיסה בתאריך שעבר.";
}
```
`DateTime.Today` = היום בלי שעה.

## 5. "שלא יהיה אפשר להזמין יותר מ-9 נוסעים"
**איפה:** `HomePage.aspx.cs` → `CheckSearch`, בסוף לפני `return "";`
```csharp
if (Convert.ToInt32(passengers) > 9)
{
    return "אפשר להזמין עד 9 נוסעים.";
}
```

---

## 6. "דפי הניהול פתוחים לכל מי שמקליד את הכתובת – תסגור"
**איפה:** `Page_Load` של `AddFlight.aspx.cs`, `Users.aspx.cs`, `Bookings.aspx.cs` – בשורה הראשונה.
```csharp
if (Session["LoggedUser"] == null)
{
    Response.Redirect("Login.aspx");
}

OleDbConnection Con1 = new OleDbConnection();
Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
Con1.Open();

string sql = "SELECT MyIsAdmin FROM Users WHERE Username = '" + Session["LoggedUser"].ToString() + "';";
OleDbCommand cmd = new OleDbCommand(sql, Con1);
string isAdmin = cmd.ExecuteScalar().ToString();
Con1.Close();

if (isAdmin != "True")
{
    Response.Redirect("HomePage.aspx");
}
```
**להסביר:** אותו רעיון כמו `IsUserAdmin` בדף הבית – רק שכאן אם לא מנהל, זורקים אותו לדף הבית.

---

## 7. "תוסיף לדף ההזמנות כמה הזמנות יש בסך הכול"
**שלב 1 – בדף `Bookings.aspx`:** להוסיף תווית ליד הכפתור:
```html
<asp:Label ID="lblCount" runat="server"></asp:Label>
```
**שלב 2 – בקוד `Bookings.aspx.cs`, בתוך `BtnShowAll_Click`,** לפני `Con1.Close();` (אחרי `Dr.Close();`):
```csharp
string sqlCount = "SELECT COUNT(*) FROM Bookings;";
OleDbCommand cmdCount = new OleDbCommand(sqlCount, Con1);
lblCount.Text = "סה\"כ הזמנות: " + cmdCount.ExecuteScalar().ToString();
```
**אותו רעיון:** סכום נקודות → `SELECT SUM(Points) FROM Bookings`, סכום נוסעים → `SELECT SUM(Passengers) FROM Bookings`.

## 8. "תוסיף לדף המשתמשים כמה משתמשים יש"
אותו דבר כמו 7, ב-`Users.aspx`, עם `SELECT COUNT(*) FROM Users;`

---

## 9. "תוסיף כפתור שמוחק טיסה" (מנהל, בדף AddFlight)
**שלב 1 – בדף:**
```html
<asp:TextBox ID="txtDeleteId" runat="server" placeholder="FlightID למחיקה"></asp:TextBox>
<asp:Button ID="BtnDelete" runat="server" Text="מחק טיסה" OnClick="BtnDelete_Click" />
```
**שלב 2 – בקוד:**
```csharp
// ===== BtnDelete_Click =====
// מה עושה: מוחקת טיסה לפי FlightID.
// מאיפה המידע: מהתיבה txtDeleteId.
// מטרה: לאפשר למנהל למחוק טיסה.
protected void BtnDelete_Click(object sender, EventArgs e)
{
    if (txtDeleteId.Text == "")
    {
        lblMsg.Text = "אנא הזן מספר טיסה למחיקה.";
        return;
    }

    OleDbConnection Con1 = new OleDbConnection();
    Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
    Con1.Open();

    string sql = "DELETE FROM Flights WHERE FlightID = " + txtDeleteId.Text + ";";
    OleDbCommand cmd = new OleDbCommand(sql, Con1);
    int rows = cmd.ExecuteNonQuery();
    Con1.Close();

    if (rows > 0)
    {
        lblMsg.Text = "הטיסה נמחקה.";
    }
    else
    {
        lblMsg.Text = "לא נמצאה טיסה עם המספר הזה.";
    }
}
```
**לשים לב:** `FlightID` הוא מספר – לכן **בלי** גרשיים `'` ב-SQL.

---

## 10. "תוסיף שדה חדש בהרשמה" (למשל עיר)
1. **במסד (Access):** לפתוח את טבלת `Users` ב-Design View, להוסיף עמודה `MyCity` מסוג Short Text, לשמור, **ולסגור את Access**.
2. **בדף `Insert.aspx`:** להוסיף תיבה, באותו סגנון כמו השדות האחרים:
   ```html
   <asp:TextBox ID="txtCity" runat="server" placeholder="עיר"></asp:TextBox>
   ```
3. **בקוד `Insert.aspx.cs`:** להוסיף את העמודה ל-INSERT – גם ברשימת העמודות וגם ב-VALUES, **באותו מקום**:
   ```csharp
   "INSERT INTO Users (..., MyCity) VALUES (..., '" + txtCity.Text + "')"
   ```

## 11. "תציג למשתמש בדף הבית כמה הזמנות הוא עשה"
**איפה:** `HomePage.aspx.cs` → `LoadUserStats`, ובדף `HomePage.aspx` ליד `litPoints` להוסיף `<asp:Literal ID="litBookings" runat="server" />`.
בקוד, לפני `Con1.Close();` (אחרי `Dr.Close();`):
```csharp
string sql2 = "SELECT COUNT(*) FROM Bookings WHERE MyUsername = '" + username + "';";
OleDbCommand cmd2 = new OleDbCommand(sql2, Con1);
litBookings.Text = cmd2.ExecuteScalar().ToString();
```

## 12. "תעלה משתמש לרמת Silver כשיש לו 10,000 נקודות"
**איפה:** `BookFlight.aspx.cs` → `AddPointsToUser`, אחרי שמחשבים את סך הנקודות החדש:
```csharp
string tier = "Bronze";
if (newPoints >= 10000)
{
    tier = "Silver";
}
if (newPoints >= 30000)
{
    tier = "Gold";
}
```
ולשנות את ה-UPDATE כך שיעדכן גם את הרמה:
```csharp
string sqlUpdate = "UPDATE Users SET MyElitePoints = '" + newPoints + "', MyEliteTier = '" + tier + "' WHERE Username = '" + username + "'";
```

---

## 13. שינויי עיצוב (CSS)

| הבוחן מבקש | מה משנים |
|---|---|
| "תעשה את הטבלה פחות מעוגלת" | `.grid-view-style { border-radius: 12px; }` → מספר קטן יותר, `0` = פינות ישרות |
| "תשנה את צבע הכותרות בטבלה" | `.grid-view-style th { background: ...; }` |
| "תשנה צבע כשעוברים עם העכבר על שורה" | `.grid-view-style tr:hover { background: ...; }` |
| "תגדיל את הכפתור" | `padding` ו-`font-size` של הכפתור |
| "תשנה את צבע ההודעה" | בקוד: `lblMsg.ForeColor = System.Drawing.Color.Red;` |
| "תסתיר כפתור" | בקוד: `BtnX.Visible = false;` |

**להסביר למה הטבלה מעוגלת:** `border-radius` מעגל את הפינות, ו-`overflow: hidden` חותך את התאים שבולטים מהפינות – אחרת הרקע של התאים היה מכסה את העיגול.

---

## 14. אם משהו נשבר באמצע הבחינה

| שגיאה | מה זה אומר | מה עושים |
|---|---|---|
| `The name 'lblCount' does not exist in the current context` | הקוד לא מכיר את הפקד החדש | לשמור את ה-aspx (Ctrl+S) / לבדוק שה-ID זהה בדיוק (אותיות גדולות!) / לבדוק שיש `runat="server"` |
| `Syntax error in INSERT INTO statement` | טעות ב-SQL | לבדוק גרשיים `'` סביב טקסט, פסיקים, ושמספר העמודות = מספר הערכים |
| `No value given for one or more required parameters` | שם עמודה לא נכון ב-SQL | לבדוק את השם בדיוק כמו במסד |
| `Data type mismatch` | טקסט במקום מספר או להפך | מספר → בלי גרשיים, טקסט → עם גרשיים |
| `Object reference not set to an instance of an object` | ניסיון לקרוא Session ריק | לבדוק `if (Session["..."] == null)` לפני |
| `The process cannot access the file ... accdb` / `could not use ... already in use` | Access פתוח | לסגור את Access |
| `Input string was not in a correct format` | `Convert.ToInt32` על טקסט שאינו מספר | לבדוק קודם עם `IsNumber` |

**דיבאג:** `F9` שם נקודת עצירה (breakpoint) על שורה, `F5` מריץ, וכשהקוד נעצר – מעבירים את העכבר על משתנה ורואים את הערך שלו. `F10` – שורה הבאה.

---

## 15. תרגול מומלץ לפני הבחינה
1. לעשות לבד את משימות 3, 6, 7 ו-9 – הן הכי נפוצות (בדיקת קלט, הרשאות, COUNT, DELETE).
2. לפני שמתחילים: **לגבות את `UsersDB.accdb`** (להעתיק לתיקייה אחרת), ובסוף להחזיר.
3. אחרי כל תרגול – לבטל את השינוי (`Ctrl+Z`, או ב-Git: **Git Changes → Undo Changes**), כדי שהפרויקט יחזור למצב הנקי.
