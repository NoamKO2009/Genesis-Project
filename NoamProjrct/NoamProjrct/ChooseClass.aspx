<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseClass.aspx.cs" Inherits="NoamProjrct.ChooseClass" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genesis Airways - בחירת מחלקת טיסה</title>
    <link href="https://fonts.googleapis.com/css2?family=Cinzel:wght@500;700;900&family=Playfair+Display:wght@500;700&family=Assistant:wght@300;400;600;700&display=swap" rel="stylesheet" />
    <style>
        * { box-sizing: border-box; }
        body {
            font-family: 'Assistant', sans-serif;
            background-color: #0b0f19;
            color: #ffffff;
            direction: rtl;
            text-align: right;
            margin: 0;
            padding: 40px 20px;
        }
        .container {
            max-width: 1400px;
            width: 100%;
            margin: 0 auto;
            background: rgba(15, 23, 42, 0.97);
            backdrop-filter: blur(16px);
            border: 2.5px solid rgba(212, 175, 55, 0.8);
            padding: 45px 40px;
            border-radius: 24px;
            box-shadow: 0 30px 70px rgba(0, 0, 0, 0.95);
        }
        .brand-title {
            text-align: center;
            color: #ffffff;
            font-family: 'Cinzel', serif;
            font-size: 28px;
            font-weight: 900;
            letter-spacing: 4px;
            margin-bottom: 8px;
        }
        .page-title {
            color: #d4af37;
            font-family: 'Cinzel', serif;
            font-size: 22px;
            text-align: center;
            margin-top: 0;
            margin-bottom: 10px;
            font-weight: bold;
        }
        .flight-info {
            text-align: center;
            color: #cbd5e1;
            font-size: 16px;
            margin-bottom: 40px;
        }
        /* מבנה כרטיסיות המחלקות (Grid) */
        .classes-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
            gap: 25px;
            margin-bottom: 40px;
        }
        .class-card {
            background: linear-gradient(145deg, #1e293b 0%, #162032 100%);
            border: 2px solid #334155;
            border-radius: 18px;
            padding: 30px 25px;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            position: relative;
            transition: transform 0.3s, border-color 0.3s, box-shadow 0.3s;
        }
        .class-card:hover {
            border-color: #d4af37;
            transform: translateY(-5px);
            box-shadow: 0 15px 35px rgba(212, 175, 55, 0.2);
        }
        .class-card.featured {
            border-color: #d4af37;
            background: linear-gradient(145deg, #243049 0%, #1a2538 100%);
            box-shadow: 0 10px 30px rgba(212, 175, 55, 0.15);
        }
        .badge-popular {
            position: absolute;
            top: -15px;
            left: 50%;
            transform: translateX(-50%);
            background: linear-gradient(135deg, #d4af37 0%, #aa7c11 100%);
            color: #0b0f19;
            font-family: 'Cinzel', serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px 15px;
            border-radius: 20px;
            letter-spacing: 1px;
        }
        .class-name {
            font-family: 'Cinzel', serif;
            font-size: 20px;
            color: #d4af37;
            text-align: center;
            margin-bottom: 5px;
            font-weight: 700;
        }
        .class-subtitle {
            text-align: center;
            font-size: 13px;
            color: #94a3b8;
            margin-bottom: 25px;
        }
        .features-list {
            list-style: none;
            padding: 0;
            margin: 0 0 30px 0;
            display: flex;
            flex-direction: column;
            gap: 12px;
            font-size: 14px;
            color: #cbd5e1;
            flex-grow: 1;
        }
        .features-list li {
            display: flex;
            align-items: center;
            gap: 10px;
        }
        .features-list li::before {
            content: "✔";
            color: #d4af37;
            font-weight: bold;
        }
        .features-list li.disabled::before {
            content: "✕";
            color: #64748b;
        }
        .features-list li.disabled {
            color: #64748b;
            text-decoration: line-through;
        }
        .btn-select-class {
            background: linear-gradient(135deg, #d4af37 0%, #aa7c11 100%);
            color: #0b0f19;
            padding: 12px 20px;
            border: none;
            border-radius: 12px;
            cursor: pointer;
            font-size: 16px;
            font-weight: bold;
            font-family: 'Cinzel', serif;
            letter-spacing: 1px;
            width: 100%;
            text-align: center;
            transition: opacity 0.2s;
        }
        .btn-select-class:hover {
            opacity: 0.9;
        }
        .back-link {
            text-align: center;
            margin-top: 20px;
        }
        .back-link a {
            color: #cbd5e1;
            text-decoration: none;
            font-family: 'Cinzel', serif;
            font-size: 14px;
        }
        .back-link a:hover {
            color: #d4af37;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="brand-title">GENESIS AIRWAYS</div>
            <h2 class="page-title">בחירת מחלקת טיסה</h2>
            <div class="flight-info">
                מספר טיסה נבחר: <asp:Label ID="lblFlightNum" runat="server" Font-Bold="true" ForeColor="#d4af37"></asp:Label>
            </div>

            <!-- אזור כרטיסי המחלקות -->
            <div class="classes-grid">
                
                <!-- מחלקה 1: Economy -->
                <div class="class-card">
                    <div>
                        <div class="class-name">Economy</div>
                        <div class="class-subtitle">מחלקת תיירים סטנדרטית</div>
                        <ul class="features-list">
                            <li>תיק אישי קטן</li>
                            <li>כבודה קלה (טרולי) לתא הנוסעים</li>
                            <li>חטיף או ארוחה קלה</li>
                            <li class="disabled">בחירת מושב מראש</li>
                            <li class="disabled">שינוי או ביטול כרטיס</li>
                            <li>צבּיר 2,000 נקודות מועדון</li>
                        </ul>
                    </div>
                    <asp:Button ID="BtnEconomy" runat="server" Text="בחר Economy" CssClass="btn-select-class" OnClick="BtnSelectClass_Click" CommandArgument="Economy" />
                </div>

                <!-- מחלקה 2: Premium Economy (מומלץ) -->
                <div class="class-card featured">
                    <div class="badge-popular">מומלץ</div>
                    <div>
                        <div class="class-name">Premium Economy</div>
                        <div class="class-subtitle">תיירים פרימיום מרווחת</div>
                        <ul class="features-list">
                            <li>תיק אישי קטן</li>
                            <li>2 יחידות כבודה לתא הנוסעים</li>
                            <li>ארוחה חמה עשירה</li>
                            <li>בחירת מושב רגיל לבחירתך</li>
                            <li>שינוי כרטיס תמורת דמי טיפול</li>
                            <li>צבּיר 3,000 נקודות מועדון</li>
                        </ul>
                    </div>
                    <asp:Button ID="BtnPremium" runat="server" Text="בחר Premium Economy" CssClass="btn-select-class" OnClick="BtnSelectClass_Click" CommandArgument="Premium Economy" />
                </div>

                <!-- מחלקה 3: Business -->
                <div class="class-card">
                    <div>
                        <div class="class-name">Business</div>
                        <div class="class-subtitle">מחלקת עסקים יוקרתית</div>
                        <ul class="features-list">
                            <li>קדימות בצ'ק-אין ועלייה למטוס</li>
                            <li>2 מזוודות לבטן המטוס + 2 טרולי</li>
                            <li>ארוחות גורמה יוקרתיות ותפריט יינות</li>
                            <li>מושב מיטה נפתח מלא</li>
                            <li>כניסה חופשית לטרקליני החברה</li>
                            <li>צבּיר 5,000 נקודות מועדון</li>
                        </ul>
                    </div>
                    <asp:Button ID="BtnBusiness" runat="server" Text="בחר Business" CssClass="btn-select-class" OnClick="BtnSelectClass_Click" CommandArgument="Business" />
                </div>

                <!-- מחלקה 4: First Class -->
                <div class="class-card">
                    <div>
                        <div class="class-name">First Class</div>
                        <div class="class-subtitle">מחלקה ראשונה אקסקלוסיבית</div>
                        <ul class="features-list">
                            <li>שירות אישי VIP וקדימות מרבית</li>
                            <li>3 מזוודות לבטן המטוס + כבודה מלאה</li>
                            <li>ארוחות שף אישיות לפי דרישה</li>
                            <li>סוויטה פרטית ופרטיות מוחלטת</li>
                            <li>כניסה לטרקליני VIP והסעות פרטיות</li>
                            <li>צבּיר 8,000 נקודות מועדון</li>
                        </ul>
                    </div>
                    <asp:Button ID="BtnFirst" runat="server" Text="בחר First Class" CssClass="btn-select-class" OnClick="BtnSelectClass_Click" CommandArgument="First Class" />
                </div>

            </div>

            <div class="back-link">
                <a href="Flights.aspx">← חזרה לרשימת הטיסות</a>
            </div>
        </div>
    </form>
</body>
</html>