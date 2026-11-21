# איך הפרויקט Genesis Airways עובד

## חלק 1: המבנה
| חלק | מה זה |
|---|---|
| **דפי `.aspx`** | העיצוב – מה שהמשתמש רואה (HTML, כפתורים, טבלאות) |
| **קבצי `.aspx.cs`** | הקוד – מה קורה כשנכנסים לדף או לוחצים על כפתור |
| **`UsersDB.accdb`** | מסד הנתונים (Access) – כל המידע נשמר כאן |
| **Session** | "זיכרון" של האתר לכל משתמש – שומר מידע בזמן שהוא עובר בין דפים |

### הטבלאות במסד
| טבלה | מה יש בה |
|---|---|
| `Users` | משתמשים: שם, שם משתמש, סיסמה, אימייל, רמת Elite, נקודות, האם מנהל |
| `Flights` | טיסות: FlightID (מספר ייחודי), מספר טיסה, מוצא, יעד, תאריך, שעה, מטוס, מקומות פנויים, מחיר |
| `Bookings` | הזמנות: מי הזמין, איזו טיסה, שם וטלפון נוסע, מחלקה, מספר נוסעים, נקודות |
| `OriginDestination` | רשימת הערים (למוצא וליעד) |
| `AircraftType` | רשימת סוגי המטוסים |
| `MyEliteTier` | רשימת רמות ה-Elite |

### איך כל דף מדבר עם המסד
בכל דף הקוד עושה אותם 4 צעדים:
```csharp
OleDbConnection Con1 = new OleDbConnection();          // 1. יוצרים חיבור
Con1.ConnectionString = "...usersdb.accdb";            //    לקובץ המסד
Con1.Open();                                           // 2. פותחים
OleDbCommand cmd = new OleDbCommand(sql, Con1);        // 3. מריצים שאילתת SQL
OleDbDataReader Dr = cmd.ExecuteReader();              //    (קריאה / הוספה / עדכון)
Con1.Close();                                          // 4. סוגרים
```
- `ExecuteReader()` – לקריאת שורות (SELECT)
- `ExecuteScalar()` – לקריאת ערך אחד (למשל COUNT או מקומות פנויים)
- `ExecuteNonQuery()` – לשינוי במסד (INSERT / UPDATE)

---

## חלק 2: המסלול – מהכניסה ועד ההזמנה

```
 Insert.aspx      Login.aspx      HomePage.aspx      Flights.aspx      ChooseClass.aspx      BookFlight.aspx      HomePage.aspx
 (הרשמה)    -->   (התחברות)  -->   (חיפוש)     -->   (הלוך+חזור)  -->   (מחלקה)         -->   (אישור)         -->   (נקודות)
```

### שלב 1 – הרשמה (`Insert.aspx`)
- המשתמש ממלא טופס.
- **לאן הולך המידע:** `INSERT INTO Users` – שורה חדשה בטבלת `Users`.
- משתמש חדש מתחיל: רמה `Bronze`, נקודות `0`, מנהל `0`.

### שלב 2 – התחברות (`Login.aspx`)
- המשתמש מקליד שם משתמש וסיסמה.
- **מאיפה המידע:** `SELECT COUNT(*) FROM Users WHERE Username=... AND MyPassword=...`
- אם נמצא משתמש: **`Session["LoggedUser"] = שם המשתמש`** ← מכאן כל האתר יודע מי מחובר.

### שלב 3 – דף הבית וחיפוש (`HomePage.aspx`)
- **בטעינה:**
  - `Session["LoggedUser"]` קיים? → מציגים ברכה, נקודות וכפתור חיפוש.
  - `IsUserAdmin()` – בודקת ב-`Users` אם `MyIsAdmin = "True"` → מציגה כפתורי ניהול.
  - `LoadDropdowns()` – ממלאת מוצא ויעד מטבלת `OriginDestination`.
- **לחיצה על "חפש טיסות":**
  1. `CheckSearch()` בודקת שכל השדות מולאו נכון.
  2. שומרים ב-Session:
     ```
     Session["Origin"]      = "Tel Aviv"
     Session["Destination"] = "London"
     Session["DepDate"]     = "12/12/2026"
     Session["RetDate"]     = "13/12/2026"
     Session["Passengers"]  = 2
     ```
  3. מעבר ל-`Flights.aspx`.

### שלב 4 – בחירת טיסות (`Flights.aspx`)
- **בטעינה:** קורא את החיפוש מה-Session ומריץ 2 שאילתות על `Flights`:
  - הלוך: `Origin = מוצא AND Destination = יעד AND FlightDate = תאריך יציאה`
  - חזור: `Origin = יעד AND Destination = מוצא AND FlightDate = תאריך חזרה` (הפוך!)
  - אם אין טיסה בתאריך – מציגים את כל הטיסות בקו.
- **לחיצה על "בחר" בטבלת ההלוך:**
  - `GetFlightId()` מוצאת את ה-`FlightID` של הטיסה לפי הפרטים בשורה.
  - שומרים: `Session["FlightId"]`, `Session["FlightNumber"]`
- **לחיצה על "בחר" בטבלת החזור:**
  - שומרים: `Session["ReturnFlightId"]`, `Session["ReturnFlightNumber"]`
  - מעבר ל-`ChooseClass.aspx`.

### שלב 5 – בחירת מחלקה (`ChooseClass.aspx`)
- מציג את מספרי הטיסות מה-Session.
- לחיצה על מחלקה → `SelectClass("Business")`:
  - שומרים: `Session["FlightClass"] = "Business"`
  - מעבר ל-`BookFlight.aspx`.

### שלב 6 – אישור הזמנה (`BookFlight.aspx`) – כאן המידע נכנס למסד
- **בטעינה:** מציג את פרטי שתי הטיסות (מ-`Flights` לפי ה-`FlightID` שב-Session).
- **לחיצה על "אישור":**
  1. בדיקה שמולאו שם וטלפון.
  2. `HasEnoughSeats()` – להלוך ולחזור. אם אין מקום → הודעה, ושום דבר לא נשמר.
  3. `BookOneFlight()` להלוך, ואז לחזור. כל קריאה עושה:
     - `UPDATE Flights SET AvailableSeats = AvailableSeats - נוסעים` ← **המקומות יורדים**
     - `CalculatePoints()` ← מחיר × נוסעים × 5 + בונוס מחלקה
     - `INSERT INTO Bookings (...)` ← **ההזמנה נשמרת**
  4. `AddPointsToUser()` – `UPDATE Users SET MyElitePoints = ...` ← **הנקודות נוספות למשתמש**
  5. מוחקים מה-Session את הטיסות והמחלקה (כדי שלא יוזמנו פעמיים).
  6. מעבר לדף הבית – שם `LoadUserStats()` מציגה את הנקודות החדשות.

---

## חלק 3: מעבר הנתונים – טבלת סיכום

### מה נשמר ב-Session ומי משתמש בזה
| Session | נשמר ב- | נקרא ב- |
|---|---|---|
| `LoggedUser` | Login | כל הדפים (בדיקת התחברות), BookFlight (שם המזמין), Update |
| `Origin`, `Destination`, `DepDate`, `RetDate` | HomePage | Flights |
| `Passengers` | HomePage | Flights, BookFlight |
| `FlightId`, `FlightNumber` | Flights (טבלה עליונה) | ChooseClass, BookFlight |
| `ReturnFlightId`, `ReturnFlightNumber` | Flights (טבלה תחתונה) | ChooseClass, BookFlight |
| `FlightClass` | ChooseClass | BookFlight |

### מה כל דף עושה במסד
| דף | קורא מ- | כותב ל- |
|---|---|---|
| Insert | – | `Users` (INSERT) |
| Login | `Users` | – |
| HomePage | `Users`, `OriginDestination` | – |
| Flights | `Flights` | – |
| ChooseClass | – | – |
| BookFlight | `Flights`, `Users` | `Flights` (UPDATE מקומות), `Bookings` (INSERT), `Users` (UPDATE נקודות) |
| Update | `Users` | `Users` (UPDATE) |
| AddFlight (מנהל) | `OriginDestination`, `AircraftType` | `Flights` (INSERT) |
| Users (מנהל) | `Users`, `MyEliteTier` | – |
| Bookings (מנהל) | `Bookings` | – |

### דוגמה מלאה
משתמש מזמין תל אביב ↔ לונדון, 1 נוסע, Economy:

| טבלה | לפני | אחרי |
|---|---|---|
| `Flights` – LY-001 (350$) | 146 מקומות | 145 מקומות |
| `Flights` – LY-031 (500$) | 236 מקומות | 235 מקומות |
| `Bookings` | – | 2 שורות חדשות: LY-001 (3,750 נק'), LY-031 (4,500 נק') |
| `Users` – MyElitePoints | 0 | 8,250 |

חישוב: LY-001: 350 × 1 × 5 = 1,750 + 2,000 = 3,750 · LY-031: 500 × 1 × 5 = 2,500 + 2,000 = 4,500.
