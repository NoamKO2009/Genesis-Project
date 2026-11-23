# הכנה לבוחן – חלק 2: קוד C#, Session ומסד נתונים

> **ש** = השאלה, **ת** = מה לענות. חלק 1 (פקדים ועיצוב) בקובץ `exam-qa-1-design.md`.
> המסלול המלא של האתר כתוב גם ב-`project-flow.md`.

---

## א. שאלות כלליות על הפרויקט

**ש: ספר על הפרויקט.**
ת: אתר של חברת תעופה בשם Genesis Airways. משתמש נרשם ומתחבר, מחפש טיסת הלוך-חזור, בוחר טיסות ומחלקה, ומזמין. אחרי ההזמנה יורדים מקומות פנויים בטיסות, ההזמנה נשמרת, והמשתמש צובר נקודות Elite. למנהל יש דפים להוספת טיסות, צפייה במשתמשים וצפייה בהזמנות.

**ש: באילו טכנולוגיות השתמשת?**
ת: ASP.NET Web Forms ב-C# (.NET Framework 4.7.2), HTML ו-CSS לעיצוב, ומסד נתונים Access (`UsersDB.accdb`). עבדתי ב-Visual Studio והקוד שמור ב-GitHub.

**ש: אילו דפים יש באתר?**
| דף | תפקיד | מי רואה |
|---|---|---|
| `HomePage.aspx` | דף הבית + חיפוש טיסות | כולם (חיפוש – רק מחובר) |
| `Insert.aspx` | הרשמה | כולם |
| `Login.aspx` | התחברות | כולם |
| `Update.aspx` | עדכון פרטים | מחובר |
| `Flights.aspx` | תוצאות חיפוש, בחירת הלוך וחזור | מחובר |
| `ChooseClass.aspx` | בחירת מחלקה | מחובר |
| `BookFlight.aspx` | אישור הזמנה | מחובר |
| `Fleet.aspx` | "מי אנחנו" | כולם |
| `AddFlight.aspx` | הוספת טיסה | מנהל |
| `Users.aspx` | ניהול משתמשים | מנהל |
| `Bookings.aspx` | צפייה בהזמנות | מנהל |

---

## ב. מושגי יסוד ב-ASP.NET

**ש: מה זה `Page_Load`?**
ת: פונקציה שרצה אוטומטית בכל פעם שהדף נטען – גם בכניסה הראשונה וגם אחרי לחיצה על כפתור.

**ש: מה זה `IsPostBack` ולמה כתבת `if (!IsPostBack)`?**
ת: `IsPostBack` הוא `true` כשהדף נטען שוב בגלל לחיצה על כפתור, ו-`false` בכניסה הראשונה.
`if (!IsPostBack)` = "רק בכניסה הראשונה". כך למשל הרשימות של מוצא ויעד נטענות פעם אחת, ולא נמחקת הבחירה של המשתמש כשהוא לוחץ "חפש".

**ש: מה זה `object sender, EventArgs e` בפונקציות?**
ת: הפרמטרים ש-ASP.NET שולח לכל פונקציה של אירוע (לחיצה, טעינה). `sender` = הפקד שהפעיל את האירוע. בפרויקט לא השתמשתי בהם, אבל הם חייבים להופיע בחתימה.

**ש: איך כפתור מחובר לפונקציה?**
ת: בדף: `<asp:Button ID="BtnSearch" ... OnClick="BtnSearch_Click" />`
ובקוד: `protected void BtnSearch_Click(object sender, EventArgs e) { ... }`

**ש: מה זה `Response.Redirect("Flights.aspx")`?**
ת: מעביר את המשתמש לדף אחר.

**ש: מה ההבדל בין `public`, `protected` ו-`private`?**
ת: `private` – רק הקוד של אותו דף יכול לקרוא לפונקציה (כמו `CheckSearch`, `GetFlightId`). `protected` – גם הדף ה-aspx יכול להשתמש בה; לכן פונקציות של כפתורים הן `protected`. `public` – כל אחד יכול (`IsUserAdmin`).

**ש: מה זה `void`?**
ת: פונקציה שלא מחזירה ערך. לעומת `bool` (מחזירה true/false), `int` (מספר), `string` (טקסט).

---

## ג. Session – איך המידע עובר בין הדפים

**ש: מה זה Session?**
ת: "זיכרון" בשרת שנשמר לכל משתמש בנפרד, כל עוד הוא באתר. שומרים בו מידע בדף אחד וקוראים אותו בדף אחר.
```csharp
Session["LoggedUser"] = txtUsername.Text;          // שמירה
string user = Session["LoggedUser"].ToString();    // קריאה
if (Session["LoggedUser"] == null) { ... }         // בדיקה אם קיים
Session.Clear();                                   // מחיקת הכול (התנתקות)
```

**ש: למה השתמשת ב-Session ולא שלחת את הנתונים בכתובת הדף?**
ת: ב-Session המידע נשמר בשרת. המשתמש לא רואה אותו ולא יכול לשנות אותו (למשל להקליד מספר טיסה אחר בכתובת). גם הקוד פשוט יותר.

**ש: מה נשמר ב-Session?**
| Session | נשמר ב- | נקרא ב- |
|---|---|---|
| `LoggedUser` | Login | כל הדפים – לבדוק מי מחובר |
| `Origin`, `Destination`, `DepDate`, `RetDate`, `Passengers` | HomePage (חיפוש) | Flights, BookFlight |
| `FlightId`, `FlightNumber` | Flights (בחירת הלוך) | ChooseClass, BookFlight |
| `ReturnFlightId`, `ReturnFlightNumber` | Flights (בחירת חזור) | ChooseClass, BookFlight |
| `FlightClass` | ChooseClass | BookFlight |

**ש: איך כל דף יודע אם המשתמש מחובר?**
ת: בודק `if (Session["LoggedUser"] == null)` – אם ריק, מעביר ל-`Login.aspx`.

**ש: מה קורה ב-Session אחרי הזמנה?**
ת: מוחקים את הטיסות והמחלקה (`Session["FlightId"] = null;`...) כדי שאם נכנסים שוב לדף ההזמנה – לא תתבצע הזמנה כפולה.

---

## ד. עבודה עם מסד הנתונים (OleDb)

**ש: איך מתחברים למסד?**
ת:
```csharp
OleDbConnection Con1 = new OleDbConnection();
Con1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\usersdb.accdb";
Con1.Open();
...
Con1.Close();
```
- `Provider=Microsoft.ACE.OLEDB.12.0` – הדרייבר של Access.
- `Server.MapPath("")` – הנתיב לתיקייה של האתר במחשב, כדי למצוא את קובץ המסד.
- `@` לפני המחרוזת – כדי ש-`\` לא ייחשב כתו מיוחד.

**ש: מה התפקיד של כל אובייקט?**
| אובייקט | תפקיד |
|---|---|
| `OleDbConnection` | החיבור לקובץ המסד |
| `OleDbCommand` | שאילתת SQL שרוצים להריץ |
| `OleDbDataReader` | קורא את התוצאות שורה אחרי שורה |

**ש: מה ההבדל בין שלוש הפעולות של `OleDbCommand`?**
| פעולה | מתי | מה מחזירה | דוגמה בפרויקט |
|---|---|---|---|
| `ExecuteReader()` | SELECT של שורות | `OleDbDataReader` | טיסות לטבלה |
| `ExecuteScalar()` | SELECT של ערך אחד | `object` | `COUNT(*)` בהתחברות, `AvailableSeats` |
| `ExecuteNonQuery()` | INSERT / UPDATE / DELETE | `int` – כמה שורות השתנו | שמירת הזמנה, הורדת מקומות |

**ש: איך קוראים נתונים מ-DataReader?**
ת:
```csharp
OleDbDataReader Dr = cmd.ExecuteReader();
if (Dr.Read())                              // עובר לשורה הבאה (true אם יש)
{
    litTier.Text = Dr["MyEliteTier"].ToString();   // ערך לפי שם עמודה
}
Dr.Close();
```

**ש: מה זה `Dr.HasRows`?**
ת: `true` אם השאילתה החזירה לפחות שורה אחת. בדף הטיסות: אם אין טיסה בתאריך – מציגים את כל הטיסות בקו.

**ש: למה צריך `Con1.Close()` ו-`Dr.Close()`?**
ת: כדי לשחרר את קובץ המסד. Access לא מאפשר הרבה חיבורים פתוחים, ואם לא סוגרים – הדף הבא יכול להיתקע.

**ש: איך הרשימה הנפתחת מתמלאת מהמסד?**
ת:
```csharp
ddlOrigin.DataSource = Dr;
ddlOrigin.DataTextField = "Origin";     // מה שהמשתמש רואה
ddlOrigin.DataValueField = "Origin";    // הערך שהקוד מקבל
ddlOrigin.DataBind();
ddlOrigin.Items.Insert(0, new ListItem("בחר מוצא...", "0"));  // שורה ראשונה עם ערך "0"
```
בבדיקת החיפוש: `if (origin == "0")` = המשתמש לא בחר.

---

## ה. מסד הנתונים – הטבלאות

| טבלה | עמודות חשובות |
|---|---|
| `Users` | `Username`, `MyPassword`, `MyEmail`, `MyFirstName`, `MyLastName`, `MyPhone`, `MyBirthDate`, `MyRegistrationDate`, `MyEliteTier`, `MyElitePoints`, `MyIsAdmin` |
| `Flights` | `FlightID` (מספר אוטומטי), `FlightNumber`, `Origin`, `Destination`, `FlightDate`, `FlightTime`, `AirplaneModel`, `TotalSeats`, `AvailableSeats`, `Price` |
| `Bookings` | `MyUsername`, `FlightNumber`, `PassengerName`, `PassengerPhone`, `BookingDate`, `FlightClass`, `Passengers`, `Points` |
| `OriginDestination` | `Origin`, `Destination` – רשימת הערים |
| `AircraftType` | `Aircraft` – דגמי המטוסים |
| `MyEliteTier` | `MyEliteTier` – Bronze, Silver, Gold, Platinum, Dimond (במסד כתוב בטעות Dimond במקום Diamond) |

**ש: מה זה `FlightID` ולמה צריך אותו?**
ת: מספר אוטומטי (AutoNumber) שונה לכל טיסה. מספר טיסה (`FlightNumber`) לא ייחודי – `LY-209` מופיע בכמה תאריכים. אם הייתי מוריד מקומות לפי `FlightNumber`, הם היו יורדים בכל הטיסות האלה. עם `FlightID` מתעדכנת רק הטיסה שנבחרה.

**ש: מה ההבדל בין `TotalSeats` ל-`AvailableSeats`?**
ת: `TotalSeats` – כמה מקומות יש במטוס (לא משתנה). `AvailableSeats` – כמה נשארו פנויים (יורד בכל הזמנה). בהוספת טיסה שניהם מקבלים אותו מספר.

**ש: מה הקשר בין הטבלאות?**
ת: `Bookings.MyUsername` = `Users.Username` (מי הזמין), `Bookings.FlightNumber` = `Flights.FlightNumber` (איזו טיסה). `Users.MyEliteTier` לוקח ערכים מ-`MyEliteTier`.

---

## ו. שאילתות SQL בפרויקט

**ש: תן דוגמה ל-SELECT.**
ת: בדף הטיסות:
```sql
SELECT * FROM Flights WHERE Origin = 'Tel Aviv' AND Destination = 'London'
  AND FlightDate = '12/12/2026' AND AvailableSeats >= 2
```
`*` = כל העמודות. `WHERE` = תנאי. `AND` = כל התנאים חייבים להתקיים.

**ש: מה זה `SELECT COUNT(*)`?**
ת: סופר כמה שורות עונות לתנאי. בהתחברות: אם יש לפחות משתמש אחד עם השם והסיסמה – ההתחברות הצליחה.

**ש: מה זה `SELECT DISTINCT`?**
ת: מחזיר כל ערך פעם אחת בלבד. ברשימת הערים ובסינון ההזמנות לפי משתמש – כדי שלא יופיעו כפילויות.

**ש: תן דוגמה ל-INSERT.**
ת: שמירת הזמנה:
```sql
INSERT INTO Bookings (MyUsername, FlightNumber, PassengerName, PassengerPhone, BookingDate, FlightClass, Passengers, Points)
VALUES ('noam', 'LY-001', 'Noam', '0500000000', '23/11/2026', 'Business', 2, 8500)
```
טקסט בתוך גרשיים `'...'`, מספרים בלי.

**ש: תן דוגמה ל-UPDATE.**
ת: הורדת מקומות:
```sql
UPDATE Flights SET AvailableSeats = AvailableSeats - 2 WHERE FlightID = 1
```
לוקח את הערך הנוכחי ומוריד ממנו. בלי `WHERE` – זה היה משנה את **כל** הטיסות.

**ש: איך בנית את ה-SQL בקוד?**
ת: חיבור מחרוזות:
```csharp
string sql = "SELECT * FROM Users WHERE Username = '" + username + "'";
```

---

## ז. שאלות על כל דף

### Insert (הרשמה)
**ש: מה קורה כשנרשמים?**
ת: `Insertbutton` מוסיפה שורה ל-`Users` עם הנתונים מהטופס, ותאריך ההרשמה הוא היום (`DateTime.Now.ToString("dd/MM/yyyy")`). משתמש חדש מקבל `Bronze`, `0` נקודות, ו-`MyIsAdmin = 0` (לא מנהל).

### Login (התחברות)
**ש: איך עובדת ההתחברות?**
ת: `SELECT COUNT(*) FROM Users WHERE Username = ... AND MyPassword = ...`. אם התוצאה גדולה מ-0 – שומרים `Session["LoggedUser"]` ועוברים לדף הבית. אחרת – הודעה אדומה "שם משתמש או סיסמה שגויים".

### HomePage (דף הבית)
**ש: מה רואה אורח ומה רואה משתמש מחובר?**
ת: ב-`Page_Load` בודקים `Session["LoggedUser"]`:
- אורח: כפתורי התחברות והרשמה, והודעה שצריך להתחבר כדי לחפש.
- מחובר: ברכה עם השם, רמת Elite ונקודות, כפתור חיפוש, עדכון פרטים והתנתקות.
- מנהל: בנוסף – שלושה כפתורי ניהול.

**ש: איך יודעים שמשתמש הוא מנהל?**
ת: `IsUserAdmin()` שולפת את `MyIsAdmin` מטבלת `Users`. אם הערך הוא `"True"` – מנהל.

**ש: מה בודקת `CheckSearch`?**
ת: לפי הסדר: נבחר מוצא, נבחר יעד, המוצא והיעד שונים, יש תאריך יציאה, יש תאריך חזרה, תאריך החזרה לא לפני היציאה, מספר הנוסעים הוא מספר (`IsNumber`), לא יותר מ-3 ספרות, ולפחות 1. בבעיה הראשונה היא מחזירה הודעה; אם הכול תקין – מחזירה `""`.

**ש: איך `IsNumber` עובדת?**
ת: לולאת `for` על כל תו בטקסט. אם תו קטן מ-`'0'` או גדול מ-`'9'` – זה לא ספרה, מחזירה `false`. אם כל התווים ספרות – `true`.

**ש: למה ממירים את התאריך ל-`dd/MM/yyyy`?**
ת: שדה התאריך בדפדפן מחזיר `2026-12-12`, אבל במסד התאריכים שמורים כטקסט בפורמט ישראלי `12/12/2026`. ממירים כדי שהחיפוש יתאים.

### Flights (תוצאות חיפוש)
**ש: איך מוצאים את טיסות החזור?**
ת: אותה שאילתה, אבל המוצא והיעד הפוכים: `Origin = יעד AND Destination = מוצא`, ובתאריך החזרה.

**ש: מה קורה אם אין טיסה בתאריך?**
ת: `Dr.HasRows` הוא `false`, אז מציגים הודעה ומריצים שאילתה בלי תאריך – כל הטיסות בקו.

**ש: מה עושה `GetFlightId`?**
ת: מקבלת שורה מהטבלה, לוקחת ממנה מספר טיסה, מוצא, יעד, תאריך ושעה, ומחפשת ב-`Flights` את ה-`FlightID` של הטיסה הזו.

**ש: מה קורה אם בוחרים טיסת חזור לפני הלוך?**
ת: `GridView2_SelectedIndexChanged` בודקת `if (Session["FlightId"] == null)` ומציגה הודעה "אנא בחר קודם טיסת הלוך".

### ChooseClass (בחירת מחלקה)
**ש: למה יש ארבע פונקציות לכפתורים ופונקציה אחת `SelectClass`?**
ת: כל כפתור צריך פונקציה משלו כדי שנדע איזה נלחץ, אבל מה שהן עושות זהה – לכן כל אחת קוראת ל-`SelectClass` עם שם המחלקה. כך לא כותבים את אותו קוד ארבע פעמים.

### BookFlight (אישור הזמנה)
**ש: מה הסדר בלחיצה על "אישור"?**
ת:
1. בדיקה שמולאו שם וטלפון.
2. `HasEnoughSeats` להלוך ולחזור – אם אין מקום, הודעה ויציאה.
3. `BookOneFlight` להלוך ולחזור – מורידה מקומות, מחשבת נקודות, שומרת הזמנה.
4. `AddPointsToUser` – מוסיפה את סך הנקודות למשתמש.
5. ניקוי ה-Session ומעבר לדף הבית.

**ש: למה בודקים את שתי הטיסות לפני שמזמינים?**
ת: כדי שלא יקרה שבהלוך ירדו מקומות ונשמרה הזמנה, ובחזור התברר שאין מקום. קודם בודקים הכול, ורק אם הכול תקין – מזמינים.

**ש: איך מחושבות הנקודות?**
ת: `CalculatePoints`: מחיר × נוסעים = כמה שולם, × 5 = נקודות על המחיר, ועוד `GetClassBonus`:
Economy 2,000 · Premium Economy 3,000 · Business 5,000 · First Class 8,000.
**דוגמה:** 350$ × 2 נוסעים = 700$ × 5 = 3,500 + Business 5,000 = **8,500**.

**ש: איך הנקודות נוספות למשתמש?**
ת: `AddPointsToUser` שולפת את `MyElitePoints` הנוכחי, ממירה למספר, מוסיפה את הנקודות החדשות, ושומרת עם `UPDATE Users`. העמודה היא טקסט, לכן `Convert.ToInt32` בקריאה.

### Update (עדכון פרטים)
**ש: איך עובד העדכון?**
ת: למעלה הפרטים הקיימים (לזיהוי), למטה החדשים. אם שדה חדש ריק – לוקחים את הערך הקיים. `UPDATE Users SET ... WHERE UserName = <קיים> AND MyPassword = <קיימת>`. אם `ExecuteNonQuery()` מחזיר יותר מ-0 – הצליח, ומעדכנים גם את `Session["LoggedUser"]`.

**ש: למה שדה הסיסמה הקיימת ריק למרות שהקוד ממלא אותו?**
ת: תיבה עם `TextMode="Password"` ב-ASP.NET לא מציגה ערך מהקוד – מטעמי אבטחה. המשתמש חייב להקליד את הסיסמה הנוכחית.

### AddFlight (מנהל)
**ש: מה קורה בהוספת טיסה?**
ת: בודקים שכל השדות מולאו, ממירים את התאריך ל-`dd/MM/yyyy`, ומוסיפים שורה ל-`Flights`. `AvailableSeats` מקבל את אותו ערך כמו `TotalSeats` – כי בטיסה חדשה כל המקומות פנויים.

### Users / Bookings (מנהל)
**ש: מה הדפים האלה עושים?**
ת: מציגים בטבלה את כל המשתמשים / כל ההזמנות, ומאפשרים לסנן לפי רשימה נפתחת (רמת Elite / שם משתמש). הרשימה עם `AutoPostBack="True"`, אז הסינון קורה מיד בבחירה.

---

## ח. המרות וטיפוסים

**ש: מה עושה `Convert.ToInt32`?**
ת: ממיר טקסט או ערך מהמסד למספר שלם. למשל `Convert.ToInt32(Session["Passengers"])`.

**ש: מה עושה `.ToString()`?**
ת: ממיר ערך לטקסט. למשל `Dr["Price"].ToString()`.

**ש: מה עושה `Convert.ToDateTime(...).ToString("dd/MM/yyyy")`?**
ת: ממיר טקסט לתאריך, ואז מציג אותו בפורמט יום/חודש/שנה.

**ש: מה זה `DateTime.Now`?**
ת: התאריך והשעה של עכשיו. משמש לתאריך ההרשמה ותאריך ההזמנה.

---

## ט. שאלות "קשות" – ותשובות כנות

**ש: האם האתר מאובטח?**
ת: לא לגמרי, וכדאי לדעת למה:
- **SQL Injection:** השאילתות נבנות מחיבור מחרוזות, אז משתמש יכול להקליד בשדה קוד SQL ולשנות את השאילתה (למשל `' OR '1'='1` בהתחברות). הפתרון המקצועי הוא פרמטרים (`OleDbParameter`), אבל בפרויקט בחרתי בדרך הבסיסית שלמדנו.
- **סיסמאות:** נשמרות כטקסט רגיל. באתר אמיתי שומרים אותן מוצפנות (Hash).
- **דפי הניהול:** הכפתורים מוסתרים ממי שאינו מנהל, אבל אם מקלידים את כתובת הדף ישירות – אפשר להיכנס. הפתרון: לבדוק `IsUserAdmin()` גם ב-`Page_Load` של דפי הניהול.

**ש: מה היית משפר?**
ת: פרמטרים ב-SQL, הצפנת סיסמאות, בדיקת מנהל בדפי הניהול, שמירת תאריכים כ-Date ולא כטקסט, ודף "ההזמנות שלי" למשתמש.

**ש: למה התאריכים במסד הם טקסט?**
ת: כך בניתי את הטבלה בהתחלה. החיסרון: תאריכים בפורמטים שונים (`1/2/27` מול `01/02/2027`) לא נמצאים בחיפוש. לכן יש "גיבוי" שמציג את כל הטיסות בקו.

**ש: מה קורה אם שני משתמשים מזמינים את המקום האחרון באותו רגע?**
ת: שניהם יכולים לעבור את הבדיקה של `HasEnoughSeats` לפני שהמקומות ירדו. באתר אמיתי משתמשים בטרנזקציה כדי למנוע את זה. בפרויקט שלי זה לא מטופל.

---

## י. GitHub

**ש: איפה הקוד שמור?**
ת: ב-GitHub: `NoamKO2009/Genesis-Project`. כל שינוי נשמר כ-commit, ובקובץ `CHANGELOG.md` כתוב בעברית מה השתנה בכל עדכון.

**ש: מה זה commit, push ו-pull?**
ת: `commit` – שמירת גרסה במחשב עם הודעה. `push` – העלאה ל-GitHub. `pull` – הורדת השינויים מ-GitHub למחשב.
