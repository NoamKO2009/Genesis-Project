<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Fleet.aspx.cs" Inherits="NoamProjrct.Fleet" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genesis Airways - צי החברה</title>
    <!-- ייבוא פונטים יוקרתיים ואלגנטיים מ-Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Cinzel:wght@500;700;900&family=Playfair+Display:wght@500;700&family=Assistant:wght@300;400;600;700&display=swap" rel="stylesheet" />
    <style>
        * {
            box-sizing: border-box;
        }

        body {
            font-family: 'Assistant', sans-serif;
            background-color: #0b0f19;
            color: #ffffff;
            direction: rtl;
            text-align: right;
            margin: 0;
            padding: 0;
            overflow-x: hidden;
        }

        /* תמונת רקע מלאה עם חשיכה מותאמת לקריאות הטקסט */
        .bg-img {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            object-fit: cover;
            filter: brightness(0.4);
            z-index: -1;
        }

        .page-container {
            max-width: 1440px;
            margin: 0 auto;
            padding: 40px 45px;
            position: relative;
            z-index: 2;
            min-height: 100vh;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
        }

        /* כותרות מרכזיות */
        .header-section {
            text-align: center;
            margin-top: 200px;
            margin-bottom: 40px;
        }

        .page-subtitle {
            color: #cbd5e1;
            font-size: 26px;
            font-weight: 700;
            font-family: 'Cinzel', serif; /* פונט יוקרתי ומעודן */
            letter-spacing: 3px;
            margin: 0;
            text-shadow: 0 2px 10px rgba(0,0,0,0.9);
        }

        /* אזור ההסברים שיושב ישירות על התמונה למטה */
        .fleet-description-area {
            background: rgba(11, 15, 25, 0.82);
            backdrop-filter: blur(12px);
            border-right: 4px solid #d4af37;
            padding: 25px 35px;
            border-radius: 12px;
            max-width: 750px;
            margin-bottom: 30px;
            box-shadow: 0 15px 35px rgba(0,0,0,0.8);
        }

            .fleet-description-area p {
                color: #e2e8f0;
                font-size: 16px;
                line-height: 1.8;
                margin: 0 0 10px 0;
            }

                .fleet-description-area p:last-child {
                    margin-bottom: 0;
                }

        /* כפתור חזרה */
        .footer-nav {
            margin-top: 20px;
            margin-bottom: 20px;
        }

        .btn-back {
            background: transparent;
            border: 1.5px solid rgba(203, 213, 225, 0.6);
            color: #cbd5e1;
            padding: 10px 24px;
            border-radius: 10px;
            font-weight: bold;
            font-family: 'Cinzel', serif;
            letter-spacing: 1px;
            cursor: pointer;
            text-decoration: none;
            display: inline-block;
            transition: all 0.3s ease;
        }

            .btn-back:hover {
                border-color: #ffffff;
                color: #ffffff;
                background-color: rgba(203, 213, 225, 0.1);
            }
    </style>
</head>
<body>
    <!-- תמונת הרקע של הצי -->
    <img src="images/Fleet.jpg" alt="Genesis Fleet" class="bg-img" />

    <form id="form1" runat="server">
        <div class="page-container">

            <!-- כותרות -->
            <div class="header-section">
                <h1 class="page-subtitle">מי אנחנו?</h1>
            </div>

            <!-- אזור ההסברים המשתלב ישירות על התמונה -->
            <div class="fleet-description-area">
                <p><strong>חברת Genesis Airways</strong> מפעילה את צי המטוסים המתקדם, היוקרתי והבטחוני ביותר בעולם התעופה המסחרית.</p>
                <p>כל מטוסינו עוצבו בקפידה רבה כדי להעניק לנוסעי היוקרה וה-Elite חוויית טיסה חסרת פשרות, עם מערכות בידור מתקדמות, מחלקות עסקים פרטיות לחלוטין ושקט אבסולוטי בתא הנוסעים.</p>
            </div>

            <!-- כפתור חזרה לדף הבית -->
            <div class="footer-nav">
                <a href="HomePage.aspx" class="btn-back">← חזרה לדף הבית</a>
            </div>

        </div>
    </form>
</body>
</html>
