# הסבר השינויים: הזמנת טיסה, מקומות פנויים, נוסעים ונקודות

## מה המטרה
כשמשתמש מחובר מחפש טיסה ומזמין אותה:
1. מספר המקומות הפנויים (`AvailableSeats`) בטבלת `Flights` יורד במספר הנוסעים שהזמין.
2. בהזמנה נשמרים מספר הנוסעים (`Passengers`) וכמות הנקודות שנצברו (`Points`).
3. הנקודות מתווספות למשתמש בטבלת `Users` (עמודה `MyElitePoints`), ומופיעות בדף הבית.
4. אם אין מספיק מקומות – ההזמנה נדחית ושום דבר לא נשמר.

## המסלול של ההזמנה באתר
```
HomePage.aspx  (חיפוש: מוצא, יעד, תאריכים, מספר נוסעים)
     |  Flights.aspx?origin=...&passengers=3
     v
Flights.aspx   (בחירת טיסה; שולח את המזהה של הטיסה + מספר נוסעים)
     |  ChooseClass.aspx?flight=LY-001&flightId=1&passengers=3
     v
ChooseClass.aspx (בחירת מחלקה; בודק שהמשתמש מחובר ומעביר הלאה את הכול)
     |  BookFlight.aspx?flight=LY-001&flightId=1&passengers=3&class=Business
     v
BookFlight.aspx  (פרטי נוסע -> אישור: בדיקת מקומות, הורדת מקומות, שמירת הזמנה, הוספת נקודות)
```
בהזמנת הלוך-חזור נוספים לכתובת גם `returnFlight` ו-`returnFlightId`.

---

## שינוי 1: מסד הנתונים
נוספו שתי עמודות לטבלת `Bookings`:

| עמודה | סוג | משמעות |
|---|---|---|
| `Passengers` | מספר | כמה נוסעים הוזמנו |
| `Points` | מספר | כמה נקודות המשתמש צבר בהזמנה הזו |

בהזמנות הישנות שהיו לפני השינוי: `Passengers = 1`, `Points = 0`.

---

## שינוי 2: `Flights.aspx` – מזהה ייחודי של טיסה

**הבעיה:** מספר טיסה (למשל `LY-209`) מופיע בכמה שורות שונות (תאריכים וקווים שונים).
אם היינו מעדכנים לפי `FlightNumber`, המקומות היו יורדים בכל השורות האלה.

**הפתרון:** לטבלה (GridView) הוספנו `DataKeyNames="מזהה"` – כך כל שורה "זוכרת" את המזהה הייחודי שלה מהמסד:
```aspx
<asp:GridView ID="GridView1" runat="server" ... OnRowCommand="GridView1_RowCommand" DataKeyNames="מזהה">
```

כשלוחצים "הזמן" (`Flights.aspx.cs`):
```csharp
int index = Convert.ToInt32(e.CommandArgument);              // מספר השורה שנלחצה
GridViewRow row = GridView1.Rows[index];
string flightNumber = row.Cells[0].Text;                      // מספר הטיסה (להצגה)
string flightId = GridView1.DataKeys[index].Value.ToString(); // המזהה הייחודי

Response.Redirect("ChooseClass.aspx?flight=" + Server.UrlEncode(flightNumber) +
                  "&flightId=" + flightId + "&passengers=" + GetPassengers());
```

---

## שינוי 3: `ChooseClass.aspx.cs` – תיקון בדיקת התחברות

**באג שתוקן:** הדף בדק `Session["Username"]`, אבל דף ההתחברות שומר `Session["LoggedUser"]`.
לכן כל משתמש נזרק לדף ההתחברות ואף אחד לא הצליח להזמין.

```csharp
if (Session["LoggedUser"] == null)
{
    Response.Redirect("Login.aspx");
    return;
}
```
בנוסף הדף מעביר הלאה לדף ההזמנה את מזהה הטיסה, טיסת החזור ומספר הנוסעים.

---

## שינוי 4: `BookFlight.aspx.cs` – הלב של השינוי

הקוד מחולק לפונקציות קטנות, כל אחת עושה דבר אחד:

| פונקציה | מה היא עושה |
|---|---|
| `GetPassengers()` | קוראת את מספר הנוסעים מהכתובת (אם אין – 1) |
| `GetFlightDetails(...)` | מחזירה טקסט עם פרטי הטיסה להצגה בדף |
| `HasEnoughSeats(...)` | בודקת אם בטיסה יש מספיק מקומות פנויים |
| `GetClassBonus(...)` | מחזירה את בונוס הנקודות של המחלקה |
| `CalculatePoints(...)` | מחשבת כמה נקודות שווה ההזמנה |
| `BookOneFlight(...)` | מזמינה טיסה אחת: מורידה מקומות ושומרת הזמנה. מחזירה את הנקודות |
| `AddPointsToUser(...)` | מוסיפה את הנקודות לחשבון המשתמש |

### בדיקת מקומות
```csharp
private bool HasEnoughSeats(OleDbConnection Con1, string flightId, int passengers)
{
    string sql = "SELECT AvailableSeats FROM Flights WHERE [מזהה] = " + flightId;
    OleDbCommand cmd = new OleDbCommand(sql, Con1);
    int availableSeats = Convert.ToInt32(cmd.ExecuteScalar());

    if (availableSeats >= passengers)
    {
        return true;
    }
    return false;
}
```

### חישוב נקודות
**כל דולר ששולם = 5 נקודות, ועוד בונוס לפי המחלקה.**

```csharp
private int GetClassBonus(string flightClass)
{
    if (flightClass == "Economy")          return 2000;
    if (flightClass == "Premium Economy")  return 3000;
    if (flightClass == "Business")         return 5000;
    if (flightClass == "First Class")      return 8000;
    return 0;
}

private int CalculatePoints(int price, int passengers, string flightClass)
{
    int totalPrice = price * passengers;       // כמה שולם בסך הכול
    int pricePoints = totalPrice * 5;          // 5 נקודות לכל דולר
    int classBonus = GetClassBonus(flightClass);
    return pricePoints + classBonus;
}
```

**דוגמה:** טיסה ב-350$, 2 נוסעים, Business:
350 × 2 = 700$ → 700 × 5 = 3,500 נקודות + 5,000 בונוס = **8,500 נקודות**.

### הזמנת טיסה אחת
```csharp
private int BookOneFlight(OleDbConnection Con1, string flightId, int passengers, string flightClass)
{
    // 1. שליפת מספר הטיסה והמחיר
    // 2. הורדת המקומות:
    string sqlSeats = "UPDATE Flights SET AvailableSeats = AvailableSeats - " + passengers + " WHERE [מזהה] = " + flightId;
    // 3. חישוב נקודות: CalculatePoints(price, passengers, flightClass)
    // 4. שמירת ההזמנה בטבלת Bookings (כולל Passengers ו-Points)
    // 5. החזרת כמות הנקודות
}
```

### הוספת נקודות למשתמש
```csharp
private void AddPointsToUser(OleDbConnection Con1, int points)
{
    // שליפת הנקודות הנוכחיות -> חיבור הנקודות החדשות -> שמירה
    int newPoints = currentPoints + points;
    string sqlUpdate = "UPDATE Users SET MyElitePoints = '" + newPoints + "' WHERE Username = '" + username + "'";
}
```

### לחיצה על "אישור וביצוע הזמנה"
סדר הפעולות:
1. בדיקה שמולאו שם נוסע וטלפון.
2. בדיקה שיש מספיק מקומות בטיסת ההלוך (ובטיסת החזור אם יש). אם אין – הודעת שגיאה ויציאה.
3. `BookOneFlight` להלוך (ולחזור אם יש) – סוכמים את הנקודות.
4. `AddPointsToUser` – מוסיפים את סך הנקודות למשתמש.
5. מעבר לדף הבית.

הבדיקה של **שתי** הטיסות נעשית **לפני** שמזמינים משהו – כך שאם בחזור אין מקום, לא יורדים מקומות גם בהלוך.

---

## שאלות שעלולים לשאול אותך

**למה משתמשים ב-`מזהה` ולא ב-`FlightNumber`?**
כי מספר טיסה לא ייחודי – אותו `LY-209` מופיע בכמה תאריכים. `מזהה` הוא מספר אוטומטי (AutoNumber) שונה לכל שורה, ולכן העדכון פוגע רק בטיסה שנבחרה.

**מאיפה מגיע מספר הנוסעים?**
מהשדה "נוסעים" בדף הבית. הוא עובר בכתובת (QueryString) מדף לדף: `Flights` → `ChooseClass` → `BookFlight`. אם הוא ריק – ברירת המחדל היא 1.

**איך מחושבות הנקודות?**
מחיר × נוסעים × 5, ועוד בונוס המחלקה (2,000 / 3,000 / 5,000 / 8,000 – כמו שכתוב בדף בחירת המחלקה). בונוס המחלקה ניתן פעם אחת לכל טיסה.

**איפה רואים את הנקודות?**
בטבלת `Bookings` (עמודה `Points` – כמה נצבר בכל הזמנה), ובדף הבית ליד "Elite Points" (סך הכול של המשתמש מטבלת `Users`).

**מה קורה בהלוך-חזור?**
נשמרות שתי הזמנות (שורה לכל טיסה), בשתיהן יורדים מקומות, וכל טיסה נותנת נקודות משלה.

**למה הורדנו את ה-try/catch בדף AddFlight?**
כדי שהקוד יהיה פשוט ואחיד בכל הפרויקט. במקום לתפוס שגיאות, הדף בודק מראש עם `if` שכל השדות מולאו.

## בדיקות שבוצעו
| בדיקה | תוצאה |
|---|---|
| הזמנת 500 נוסעים | נדחתה: "אין מספיק מקומות פנויים בטיסת ההלוך" – שום דבר לא השתנה |
| הלוך-חזור LY-001 (350$) + LY-031 (500$), 2 נוסעים, Business | מקומות: 146→144 ו-236→234 |
| | נקודות בהזמנות: 8,500 ו-10,000 |
| | נקודות המשתמש: 18,500 |

## קבצים ששונו
- `UsersDB.accdb` – עמודות חדשות `Passengers` ו-`Points` בטבלת `Bookings`
- `Flights.aspx`, `Flights.aspx.cs`
- `ChooseClass.aspx.cs`
- `BookFlight.aspx.cs`
- `AddFlight.aspx.cs` – הוסר try/catch
