# פרומפטים לתמונות המטוסים – Genesis Airways

## איך משתמשים
1. פותחים כלי ליצירת תמונות (ChatGPT, Gemini וכו').
2. **מצרפים את התמונה של ה-787 שלנו** (המטוס הכחול עם הזנב הזהב) – כדי שהכלי יעתיק את הצבעים והלוגו.
3. מדביקים את **הפרומפט הבסיסי** ומיד אחריו את **השורה של המטוס** מהטבלה.
4. שומרים את התמונה בתיקייה:
   `C:\NoamProjectYudB\WithOrder\NoamProjrct\NoamProjrct\Images\Fleet`
   **בשם המדויק מהטבלה** (למשל `A320.jpg`) – ככה היא מחליפה את התמונה הקיימת והדף מציג אותה אוטומטית.
5. **סיומת `.jpg`** – אם הכלי שומר `.png`, ממירים (למשל פותחים ב-Paint ← שמירה בשם ← JPEG).

**טיפ:** כדי שכל התמונות ייראו כמו סדרה אחת – לייצר את כולן באותה שיחה, אחת אחרי השנייה.

---

## פרומפט בסיסי (מדביקים בכל פעם)

```
Photorealistic aviation photograph of a commercial airliner in the "Genesis Airways" livery, exactly like the attached reference image:
- Fuselage: deep navy blue (almost black-blue), glossy.
- A thin gold cheatline running along the lower side of the fuselage.
- Large gold serif lettering "GENESIS AIRWAYS" on the upper fuselage, with a small gold bird logo before the text.
- Tail fin: solid metallic gold with a navy blue stylized bird (wings spread upward) logo.
- Engine nacelles: navy blue with silver/chrome inlet lip.
- Wings: light grey.
Scene: the aircraft parked on a wet airport apron at night, light rain, bright floodlights reflecting on the wet tarmac, a modern terminal blurred in the background. No other airline logos visible anywhere.
Camera: three-quarter front-side view, the whole aircraft fully visible and centered, landscape 16:10 format, sharp focus, cinematic lighting.
The aircraft must be accurately shaped as the following model:
```

---

## שורה לכל מטוס (מדביקים אחרי הפרומפט הבסיסי)

| שם הקובץ | מה להדביק |
|---|---|
| `A320.jpg` | `Airbus A320neo: short single-aisle narrow-body jet, 2 large underwing engines, upward "sharklet" wingtips, rounded Airbus nose.` |
| `A321.jpg` | `Airbus A321neo: stretched (longer) single-aisle narrow-body jet, 2 large underwing engines, sharklet wingtips, extra emergency doors over and behind the wing.` |
| `A350-900.jpg` | `Airbus A350-900: twin-aisle wide-body jet, 2 very large engines, long curved blended winglets that sweep upward, distinctive black "mask" around the cockpit windows.` |
| `A350-1000.jpg` | `Airbus A350-1000: the longest A350, twin-aisle wide-body jet, 2 very large engines, curved blended winglets, black cockpit window mask, six-wheel main landing gear.` |
| `A380-800.jpg` | `Airbus A380-800: giant double-deck airliner with two full rows of windows along the entire fuselage, 4 engines (two under each wing), very large wings with small wingtip fences.` |
| `B787-8.jpg` | `Boeing 787-8 Dreamliner: shorter twin-aisle wide-body jet, 2 engines with serrated (chevron) rear edges, long flexible wings curving upward at the tips with raked wingtips, smooth pointed nose with 4 cockpit windows.` |
| `B787-9.jpg` | `Boeing 787-9 Dreamliner: stretched twin-aisle wide-body jet, 2 engines with serrated (chevron) rear edges, upward-flexing raked wingtips, smooth pointed nose.` |
| `B777-200ER.jpg` | `Boeing 777-200ER: large twin-aisle wide-body jet, 2 huge round engines, raked wingtips, six-wheel main landing gear, flat "blade" tail cone.` |
| `B777-300ER.jpg` | `Boeing 777-300ER: very long twin-aisle wide-body jet (longer than 777-200ER), 2 huge round engines, long raked wingtips, six-wheel main landing gear.` |
| `B777X.jpg` | `Boeing 777-9 (777X): the longest twin-engine airliner, 2 enormous engines, very long composite wings with folding wingtips (raised up while parked), six-wheel main gear.` |
| `B737-800MAX.jpg` | `Boeing 737 MAX 8: single-aisle narrow-body jet, 2 engines with serrated (chevron) rear edges, split "scimitar" winglets pointing both up and down, pointed tail cone.` |
| `B737-900ER.jpg` | `Boeing 737-900ER: long single-aisle narrow-body jet, 2 engines with slightly flattened bottoms, tall upward blended winglets, classic 737 nose.` |

---

## אחרי שכל התמונות מוכנות
בתחתית `OurFleet.aspx` יש שורת קרדיט לצלמים מ-Wikimedia. כשכל התמונות הוחלפו בתמונות שיצרת – אפשר למחוק אותה (או להחליף ב-"התמונות נוצרו באמצעות בינה מלאכותית").
