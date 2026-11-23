# הכנה לבוחן – חלק 1: פקדים, עיצוב ו-CSS

> בכל שאלה: **ש** = השאלה שהבוחן יכול לשאול, **ת** = מה לענות.
> חלק 2 (קוד C#, Session ומסד נתונים) נמצא בקובץ `exam-qa-2-code.md`.

---

## א. מבנה דף ב-ASP.NET

**ש: מאילו קבצים בנוי כל דף באתר?**
ת: משלושה קבצים:
- `HomePage.aspx` – העיצוב: HTML ופקדי ASP.NET (מה שרואים).
- `HomePage.aspx.cs` – הקוד (Code Behind) ב-C#: מה קורה כשנכנסים לדף או לוחצים על כפתור.
- `HomePage.aspx.designer.cs` – קובץ ש-Visual Studio יוצר לבד. הוא מגדיר את הפקדים כמשתנים, כדי שאפשר יהיה לכתוב בקוד `lblMsg.Text = ...`.

**ש: מה השורה הראשונה בכל דף aspx?**
ת: `<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="NoamProjrct.HomePage" %>`
היא אומרת: הדף כתוב ב-C#, קובץ הקוד שלו הוא `HomePage.aspx.cs`, והמחלקה שלו היא `HomePage`.

**ש: מה זה `runat="server"`?**
ת: פקד עם `runat="server"` מנוהל על ידי השרת. אפשר לגשת אליו מהקוד ב-C# לפי ה-`ID` שלו (למשל `txtUsername.Text`). בלי זה הוא HTML רגיל שהקוד לא רואה.

**ש: למה כל הדף נמצא בתוך `<form id="form1" runat="server">`?**
ת: ב-ASP.NET Web Forms כל הפקדים חייבים להיות בתוך form אחד של השרת. כשלוחצים על כפתור, הטופס נשלח לשרת (PostBack) והקוד של הכפתור רץ.

**ש: מה ההבדל בין `<asp:Button>` לבין `<a href>`?**
ת: `asp:Button` שולח את הטופס לשרת ומריץ פונקציה ב-C# (למשל `OnClick="BtnSearch_Click"`). `<a href="HomePage.aspx">` הוא קישור רגיל – רק עובר לדף אחר בלי להריץ קוד. בכפתורי "חזרה" השתמשתי בקישור כי לא צריך קוד.

---

## ב. הפקדים (Controls) באתר

| פקד | איפה באתר | למה |
|---|---|---|
| `asp:TextBox` | כל הטפסים | שדה להקלדה (שם, סיסמה, טלפון...) |
| `asp:DropDownList` | מוצא / יעד / דגם מטוס / סינון | רשימה נפתחת |
| `asp:Button` | חיפוש, התחברות, הזמנה... | כפתור שמריץ קוד בשרת |
| `asp:Label` | הודעות שגיאה והצלחה | טקסט שהקוד משנה |
| `asp:Literal` | ברכה ונקודות בדף הבית | טקסט בלי תגית עוטפת |
| `asp:Panel` | אזור טיסות החזור, נתוני המשתמש | קבוצת פקדים שאפשר להסתיר/להציג יחד |
| `asp:GridView` | טיסות, משתמשים, הזמנות | טבלה שמציגה נתונים מהמסד |

**ש: מה האובייקט של הטבלה בדף הטיסות?**
ת: `asp:GridView`. בדף הטיסות יש שניים: `GridView1` לטיסות הלוך ו-`GridView2` לטיסות חזור.

**ש: איך GridView מקבל את הנתונים?**
ת: בקוד:
```csharp
GridView1.DataSource = Dr;   // Dr = התוצאות של שאילתת SELECT
GridView1.DataBind();        // מצייר את הטבלה
```

**ש: מה זה `AutoGenerateColumns="False"`?**
ת: אני מגדיר בעצמי אילו עמודות יופיעו ובאיזה סדר, עם `BoundField`. בדף הטיסות הגדרתי: מספר טיסה, מוצא, יעד, תאריך, שעה, מחיר, מקומות פנויים.
בדפי הניהול (Users, Bookings) לא כתבתי את זה, אז ה-GridView מציג אוטומטית את **כל** העמודות מהטבלה.

**ש: מה זה `BoundField`?**
ת: עמודה בטבלה שמחוברת לעמודה במסד.
`<asp:BoundField DataField="FlightNumber" HeaderText="מספר טיסה" />`
- `DataField` – שם העמודה במסד.
- `HeaderText` – הכותרת שמופיעה בטבלה.

**ש: מה זה `ButtonField` ולמה `CommandName="Select"`?**
ת: עמודה עם כפתור בכל שורה (הכפתור "בחר"). `CommandName="Select"` הוא שם מיוחד: לחיצה עליו בוחרת את השורה ומפעילה את האירוע `OnSelectedIndexChanged`. בקוד מקבלים את השורה עם `GridView1.SelectedRow`.

**ש: איך קוראים ערך מתא בשורה?**
ת: `row.Cells[0].Text` – התא הראשון (מספר טיסה). המספור מתחיל מ-0: `Cells[1]` מוצא, `Cells[2]` יעד, `Cells[3]` תאריך, `Cells[4]` שעה.

**ש: מה זה `GridLines="None"`?**
ת: מבטל את קווי הרשת המובנים של ה-GridView, כדי שהקווים יבואו רק מה-CSS שלי.

**ש: מה זה `TextMode="Password"` / `TextMode="Date"`?**
ת: `Password` – מציג נקודות במקום אותיות. `Date` – הדפדפן מציג לוח שנה לבחירת תאריך (בדף הבית ובהוספת טיסה).

**ש: מה זה `ReadOnly="true"`?**
ת: שדה שאפשר לראות ואי אפשר לשנות. השתמשתי בזה ב"אימייל קיים" בדף העדכון ובשם המשתמש בדף ההזמנה.

**ש: מה זה `Visible="false"`?**
ת: הפקד לא נשלח לדפדפן בכלל. בקוד משנים את זה, למשל `BtnSearch.Visible = true;` כשהמשתמש מחובר.

**ש: מה זה `AutoPostBack="True"` ברשימה בדפי הניהול?**
ת: ברגע שבוחרים ערך ברשימה, הדף נשלח לשרת מיד (בלי ללחוץ על כפתור), ורצה הפונקציה של `OnSelectedIndexChanged` – למשל סינון המשתמשים לפי רמה.

**ש: מה ההבדל בין `CssClass` ל-`class`?**
ת: בפקדי `asp:` כותבים `CssClass="btn-search"`, ו-ASP.NET הופך את זה ל-`class="btn-search"` ב-HTML. בתגיות HTML רגילות כותבים `class` ישירות.

---

## ג. CSS – איפה הוא נמצא

**ש: איפה כתוב העיצוב?**
ת: בשני סוגים:
- **קבצי CSS חיצוניים** בתיקייה `Styles`: `HomePage.css` (דף הבית), `ShowUsers.css` (ניהול משתמשים), `AddFlightCss.css` (הוספת טיסה). מחברים עם:
  `<link href="styles/HomePage.css" rel="stylesheet" type="text/css" />`
- **CSS פנימי** בתוך תגית `<style>` בראש הדף – בשאר הדפים (Login, Flights, BookFlight, ChooseClass...).

**ש: מאיפה הפונטים?**
ת: מ-Google Fonts, עם `<link>` בראש כל דף. שלושה פונטים:
- `Cinzel` – הכותרות והכפתורים (GENESIS AIRWAYS).
- `Playfair Display` – הודעת הברכה ותת-כותרת.
- `Assistant` – כל הטקסט הרגיל (תומך בעברית).

**ש: איך האתר מוצג מימין לשמאל?**
ת: ב-CSS של `body`: `direction: rtl;` ו-`text-align: right;`.

**ש: מה עושה `* { box-sizing: border-box; }`?**
ת: גורם לכך שה-padding וה-border נכללים ברוחב של האלמנט. כך `width: 100%` לא יוצא מהמסגרת.

---

## ד. הטבלה המעוגלת (GridView עם פינות עגולות)

**ש: GridView הוא טבלה רגילה – איך הפינות שלו מעוגלות?**
ת: GridView הופך ב-HTML ל-`<table>`. עיגלתי אותו ב-CSS:
```css
.grid-view-style {
    border-radius: 12px;     /* מעגל את הפינות */
    overflow: hidden;        /* חותך את מה שיוצא מהפינות העגולות */
    border-collapse: collapse;
    border: 1px solid #334155;
    background-color: #1e293b;
}
```
ובפקד: `<asp:GridView ... CssClass="grid-view-style" GridLines="None">`

**ש: למה צריך `overflow: hidden`?**
ת: `border-radius` מעגל רק את המסגרת. התאים שבפינות (למשל צבע הרקע של הכותרת) "בולטים" החוצה מהפינה. `overflow: hidden` חותך אותם כך שהכול נשאר בתוך הפינה העגולה.

**ש: ובדף ניהול המשתמשים?**
ת: שם עטפתי את ה-GridView ב-`<div class="grid-container">`, והעיגול על ה-div:
```css
.grid-container {
    border-radius: 16px;
    overflow: hidden;
    border: 2px solid rgba(212, 175, 55, 0.4);
}
```

**ש: איך מעוצבות הכותרות והשורות של הטבלה?**
ת:
```css
.grid-view-style th {               /* תא כותרת */
    background-color: #0f172a;
    color: #d4af37;                  /* זהב */
    border-bottom: 2px solid #d4af37;
    font-family: 'Cinzel', serif;
}
.grid-view-style td {               /* תא רגיל */
    padding: 16px 20px;
    border-bottom: 1px solid #334155;
}
.grid-view-style tr:hover {         /* שורה כשהעכבר מעליה */
    background-color: rgba(212, 175, 55, 0.05);
}
```

**ש: למה הטבלה בתוך `<div style="overflow-x: auto;">`?**
ת: אם המסך צר והטבלה רחבה, יופיע פס גלילה אופקי במקום שהטבלה תצא מהדף.

---

## ה. עיצובים נוספים שכדאי להכיר

**ש: איך עשית את הצבע הזהב בכפתורים?**
ת: מעבר צבעים (gradient):
`background: linear-gradient(135deg, #d4af37 0%, #aa7c11 100%);`
מזהב בהיר (`#d4af37`) לזהב כהה (`#aa7c11`) בזווית של 135 מעלות.

**ש: מה זה `rgba(212, 175, 55, 0.8)`?**
ת: צבע זהב עם שקיפות. שלושת המספרים הראשונים הם אדום, ירוק, כחול (0-255), והרביעי הוא שקיפות (0 = שקוף, 1 = אטום).

**ש: איך הכפתור "קופץ" כשהעכבר עליו?**
ת:
```css
.btn-search:hover { transform: translateY(-2px); opacity: 0.95; }
.btn-search { transition: transform 0.2s, opacity 0.2s; }
```
`:hover` – כשהעכבר מעל. `translateY(-2px)` – מזיז 2 פיקסלים למעלה. `transition` – עושה את זה בהדרגה ולא בבת אחת.

**ש: מה זה `box-shadow`?**
ת: צל מסביב לאלמנט. למשל `box-shadow: 0 30px 70px rgba(0,0,0,0.95);` – צל שחור גדול מתחת לכרטיס, נותן תחושה שהוא "מרחף".

**ש: איך הכותרת GENESIS AIRWAYS נמצאת מעל התמונה בדף הבית?**
ת: ב-`.hero-banner` יש `position: relative`. התמונה `position: absolute` עם `z-index: 1`, והטקסט `position: relative` עם `z-index: 2` – כלומר הטקסט בשכבה מעל התמונה. `filter: brightness(0.45)` מכהה את התמונה כדי שהטקסט יבלוט.

**ש: איך כרטיס החיפוש "עולה" על התמונה?**
ת: `.booking-card { margin: -70px auto 0 auto; z-index: 10; }` – margin שלילי מושך אותו 70 פיקסלים למעלה, ו-`z-index` שם אותו מעל הבאנר.

**ש: איך השדות בטופס החיפוש מסודרים בשורה?**
ת: עם CSS Grid:
```css
.search-grid { display: grid; grid-template-columns: 1fr 1fr 1fr auto; gap: 18px; }
```
וב-`@media (max-width: 1000px)` הם עוברים לעמודה אחת – כך זה עובד גם במסך קטן.

**ש: מה ההבדל בין Flex ל-Grid?**
ת: `display: flex` מסדר אלמנטים בשורה אחת או בעמודה אחת (למשל כפתורי ההתחברות בפס העליון). `display: grid` מסדר בטבלה של שורות ועמודות (טופס החיפוש, כרטיסי המחלקות).

**ש: איך כרטיסי המחלקות מסתדרים לבד לפי רוחב המסך?**
ת: `grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));` – כמה כרטיסים שנכנסים, כל אחד לפחות 280 פיקסלים.

**ש: מה זה `.class-card.featured`?**
ת: כרטיס עם שתי מחלקות CSS. Premium Economy מקבל גם `featured` – מסגרת זהב ותגית "מומלץ" מעליו.

**ש: איך התגית "מומלץ" יושבת בדיוק באמצע למעלה?**
ת: `position: absolute; top: -15px; left: 50%; transform: translateX(-50%);` – זזה לאמצע, ואז אחורה בחצי מהרוחב שלה.

**ש: איך עוצבו הרשימות הנפתחות (DropDownList)?**
ת: עם `appearance: base-select` – אפשרות חדשה בדפדפנים שמאפשרת לעצב גם את הרשימה שנפתחת (`::picker(select)`), את החץ (`::picker-icon`) ואת האפשרויות (`option:hover`, `option:checked`). החץ מסתובב ב-180 מעלות כשהרשימה פתוחה: `.search-box-select:open::picker-icon { rotate: 180deg; }`.
אם הדפדפן לא תומך – מוצגת רשימה רגילה (בגלל `@supports`).

**ש: איך עובד כפתור העין בדף ההתחברות?**
ת: פונקציית JavaScript `togglePassword()`:
- מוצאת את שדה הסיסמה עם `document.getElementById('<%= txtPassword.ClientID %>')`.
- אם ה-`type` הוא `password` – משנה ל-`text` (רואים את הסיסמה), ואחרת מחזירה ל-`password`.
`<%= txtPassword.ClientID %>` נותן את ה-ID האמיתי שהשדה קיבל ב-HTML.
הכפתור הוא `type="button"` כדי שלא ישלח את הטופס.

**ש: איך הלוגו מוביל לדף "מי אנחנו"?**
ת: הוא עטוף בקישור: `<a href="Fleet.aspx"><img src="images/Logo.png" class="brand-logo" /></a>`.

**ש: מה זה `object-fit: cover`?**
ת: התמונה ממלאת את כל השטח בלי להימתח – חותכת את מה שמיותר. משמש בתמונות הרקע.

---

## ו. צבעי האתר

| צבע | קוד | שימוש |
|---|---|---|
| כחול-שחור | `#0b0f19` | רקע הדף |
| כחול כהה | `#0f172a`, `#1e293b` | כרטיסים, שדות, כותרות טבלה |
| זהב | `#d4af37` | כותרות, מסגרות, כפתורים |
| אפור בהיר | `#cbd5e1`, `#e2e8f0` | טקסט רגיל |
| אדום | `#fca5a5`, `#ef4444` | הודעות שגיאה, כפתור התנתקות |
| ירוק | `#4ade80` | הודעות הצלחה |
