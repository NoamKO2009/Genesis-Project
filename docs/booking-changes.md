# הסבר תהליך הזמנת הטיסה

## מה האתר עושה
1. משתמש מחובר ממלא חיפוש: מוצא, יעד, תאריך יציאה, תאריך חזרה ומספר נוסעים. אי אפשר לחפש בלי למלא הכול.
2. בוחר טיסת הלוך וטיסת חזור.
3. בוחר מחלקה.
4. ממלא שם וטלפון ומאשר.
5. אחרי האישור:
   - המקומות הפנויים (`AvailableSeats`) בשתי הטיסות יורדים לפי מספר הנוסעים.
   - נשמרות שתי הזמנות בטבלת `Bookings` (אחת לכל טיסה), כולל מספר נוסעים ונקודות.
   - הנקודות מתווספות למשתמש בטבלת `Users` ומופיעות בדף הבית.

## איך המידע עובר בין הדפים: Session
כל הפרטים נשמרים ב-**Session** – "זיכרון" של האתר לכל משתמש, שנשאר בזמן שהוא עובר בין דפים.

| דף | מה הוא שומר ב-Session |
|---|---|
| `HomePage` (חיפוש) | `Origin`, `Destination`, `DepDate`, `RetDate`, `Passengers` |
| `Flights` (בחירת טיסות) | `FlightId`, `FlightNumber`, `ReturnFlightId`, `ReturnFlightNumber` |
| `ChooseClass` (בחירת מחלקה) | `FlightClass` |
| `BookFlight` (אישור) | קורא את כל מה שלמעלה, ובסוף מוחק את הטיסות שנבחרו |

```
HomePage.aspx ──> Flights.aspx ──> ChooseClass.aspx ──> BookFlight.aspx ──> HomePage.aspx
   (חיפוש)       (הלוך + חזור)       (מחלקה)             (אישור)
```

כפתור "חזרה לרשימת הטיסות" פשוט מוביל ל-`Flights.aspx`, והדף טוען שוב את החיפוש מה-Session.

---

## מסד הנתונים
נוספו שתי עמודות לטבלת `Bookings`:

| עמודה | סוג | משמעות |
|---|---|---|
| `Passengers` | מספר | כמה נוסעים הוזמנו |
| `Points` | מספר | כמה נקודות נצברו בהזמנה הזו |

---

## HomePage.aspx.cs – בדיקת החיפוש
הפונקציה `CheckSearch` בודקת את השדות אחד אחרי השני. בבעיה הראשונה היא מחזירה הודעת שגיאה. אם הכול תקין – היא מחזירה טקסט ריק.

```csharp
if (origin == "0")      return "אנא בחר מוצא.";
if (destination == "0") return "אנא בחר יעד.";
if (origin == destination) return "המוצא והיעד לא יכולים להיות אותה עיר.";
if (depDate == "")      return "אנא בחר תאריך יציאה.";
if (retDate == "")      return "אנא בחר תאריך חזרה.";
if (Convert.ToDateTime(retDate) < Convert.ToDateTime(depDate)) return "תאריך החזרה חייב להיות אחרי תאריך היציאה.";
if (!IsNumber(passengers)) return "אנא הזן מספר נוסעים (מספר בלבד).";
...
return "";
```

הפונקציה `IsNumber` עוברת בלולאה על כל תו ובודקת שהוא ספרה:
```csharp
for (int i = 0; i < text.Length; i++)
{
    if (text[i] < '0' || text[i] > '9')
    {
        return false;
    }
}
return true;
```

אם הכול תקין – שומרים ב-Session ועוברים לדף הטיסות:
```csharp
Session["Origin"] = origin;
Session["DepDate"] = Convert.ToDateTime(depDate).ToString("dd/MM/yyyy");
...
Response.Redirect("Flights.aspx");
```

---

## Flights.aspx.cs – בחירת טיסות
- אם המשתמש לא מחובר → `Login.aspx`. אם לא היה חיפוש → `HomePage.aspx`.
- מציגים שתי טבלאות: הלוך (מוצא → יעד) וחזור (יעד → מוצא).
- אם אין טיסה בתאריך שנבחר – מציגים את כל הטיסות באותו קו.

בכל שורה בטבלה יש כפתור "בחר" (`CommandName="Select"`). לחיצה עליו מפעילה את `SelectedIndexChanged`, ואת השורה שנבחרה מקבלים עם `GridView1.SelectedRow`:

```csharp
protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
{
    GridViewRow row = GridView1.SelectedRow;
    Session["FlightId"] = GetFlightId(row);
    Session["FlightNumber"] = row.Cells[0].Text;
}
```

**למה צריך `GetFlightId`?** מספר טיסה (למשל `LY-209`) מופיע בכמה תאריכים. לכן מחפשים במסד את ה-`מזהה` לפי כל הפרטים שבשורה – מספר טיסה, מוצא, יעד, תאריך ושעה:
```csharp
string sql = "SELECT [מזהה] FROM Flights WHERE FlightNumber = '" + flightNumber + "' AND Origin = '" + origin +
    "' AND Destination = '" + destination + "' AND FlightDate = '" + flightDate + "' AND FlightTime = '" + flightTime + "'";
```

בטבלת החזור, אם עוד לא נבחרה טיסת הלוך – מציגים הודעה. אחרת שומרים את טיסת החזור ועוברים ל-`ChooseClass.aspx`.

---

## ChooseClass.aspx.cs – בחירת מחלקה
לכל כפתור יש פונקציה משלו, וכולן קוראות לאותה פונקציה עם שם המחלקה:
```csharp
protected void BtnBusiness_Click(object sender, EventArgs e)
{
    SelectClass("Business");
}

private void SelectClass(string flightClass)
{
    if (Session["LoggedUser"] == null) { Response.Redirect("Login.aspx"); return; }
    Session["FlightClass"] = flightClass;
    Response.Redirect("BookFlight.aspx");
}
```

---

## BookFlight.aspx.cs – אישור ההזמנה

| פונקציה | מה היא עושה |
|---|---|
| `GetFlightDetails` | מחזירה טקסט עם פרטי הטיסה להצגה בדף |
| `GetPassengers` | מחזירה את מספר הנוסעים מה-Session |
| `HasEnoughSeats` | בודקת אם בטיסה יש מספיק מקומות פנויים |
| `GetClassBonus` | מחזירה את בונוס הנקודות של המחלקה |
| `CalculatePoints` | מחשבת כמה נקודות שווה ההזמנה |
| `BookOneFlight` | מזמינה טיסה אחת: מורידה מקומות ושומרת הזמנה. מחזירה את הנקודות |
| `AddPointsToUser` | מוסיפה את הנקודות לחשבון המשתמש |

### סדר הפעולות בלחיצה על "אישור"
1. בדיקה שמולאו שם נוסע וטלפון.
2. `HasEnoughSeats` לטיסת ההלוך ולטיסת החזור. אם אין מקום – הודעה ויציאה. **הבדיקה של שתי הטיסות נעשית לפני שמזמינים משהו**, כך שלא יקרה שבהלוך ירדו מקומות ובחזור לא.
3. `BookOneFlight` להלוך ולחזור, וסוכמים את הנקודות.
4. `AddPointsToUser` עם סך הנקודות.
5. מוחקים מה-Session את הטיסות שנבחרו (כדי שלא יוזמנו פעמיים) ועוברים לדף הבית.

### חישוב נקודות
**כל דולר ששולם = 5 נקודות, ועוד בונוס לפי המחלקה.**

| מחלקה | בונוס |
|---|---|
| Economy | 2,000 |
| Premium Economy | 3,000 |
| Business | 5,000 |
| First Class | 8,000 |

```csharp
int totalPrice = price * passengers;   // כמה שולם
int pricePoints = totalPrice * 5;      // 5 נקודות לכל דולר
int classBonus = GetClassBonus(flightClass);
return pricePoints + classBonus;
```

**דוגמה:** LY-001 ב-350$, 2 נוסעים, Business: 350 × 2 × 5 = 3,500 + 5,000 = **8,500 נקודות**.

### הורדת מקומות
```csharp
string sqlSeats = "UPDATE Flights SET AvailableSeats = AvailableSeats - " + passengers + " WHERE [מזהה] = " + flightId;
```

---

## שאלות שעלולים לשאול אותך

**למה משתמשים ב-Session ולא בכתובת הדף?**
Session שומר את המידע בצד השרת לכל משתמש. כך לא צריך להעביר פרטים בכתובת, והמשתמש לא יכול לשנות אותם ידנית.

**למה משתמשים ב-`מזהה` ולא ב-`FlightNumber`?**
מספר טיסה לא ייחודי – אותו `LY-209` מופיע בכמה תאריכים. `מזהה` הוא מספר אוטומטי שונה לכל שורה, ולכן העדכון פוגע רק בטיסה שנבחרה.

**איך יודעים שאין מספיק מקומות?**
`HasEnoughSeats` שולפת את `AvailableSeats` ומשווה למספר הנוסעים.

**מה קורה אם לוחצים "אישור" פעמיים?**
אחרי הזמנה מוחקים את הטיסות מה-Session, אז בכניסה חוזרת לדף מופיעה הודעה "לא נבחרה טיסה להזמנה".

**איפה רואים את הנקודות?**
בטבלת `Bookings` (עמודה `Points`) ובדף הבית ליד "Elite Points".

## בדיקה שבוצעה
| שלב | תוצאה |
|---|---|
| חיפוש עם "abc" בנוסעים | "אנא הזן מספר נוסעים (מספר בלבד)." |
| חיפוש תל אביב ↔ לונדון, 2 נוסעים | שתי טבלאות: LY-001 הלוך, LY-031 חזור |
| חזרה לרשימת הטיסות מדף המחלקות | חוזר לאותן תוצאות |
| הזמנת Business | מקומות: 146→144, 236→234. נקודות: 8,500 + 10,000 = 18,500 |
| כניסה שוב לדף ההזמנה אחרי ההזמנה | "שגיאה: לא נבחרה טיסה להזמנה." |
