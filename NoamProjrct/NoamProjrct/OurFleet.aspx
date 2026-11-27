<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OurFleet.aspx.cs" Inherits="NoamProjrct.OurFleet" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genesis Airways - צי החברה</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://fonts.googleapis.com/css2?family=Cinzel:wght@500;700;900&family=Assistant:wght@300;400;600;700&display=swap" rel="stylesheet" />
    <style>
        * {
            box-sizing: border-box;
        }

        /* רקע כהה עם מעבר צבע (gradient) מכחול לשחור */
        body {
            font-family: 'Assistant', sans-serif;
            background: linear-gradient(180deg, #13203b 0%, #0b0f19 45%, #070a12 100%);
            background-attachment: fixed;
            color: #ffffff;
            direction: rtl;
            text-align: right;
            margin: 0;
            padding: 0;
            min-height: 100vh;
        }

        .page-container {
            max-width: 1300px;
            margin: 0 auto;
            padding: 40px 24px;
        }

        /* כותרת הדף */
        .header-section {
            text-align: center;
            margin: 30px 0 35px 0;
        }

        .brand-title {
            color: #ffffff;
            font-family: 'Cinzel', serif;
            font-size: 22px;
            letter-spacing: 4px;
            margin: 0 0 8px 0;
        }

        .page-title {
            color: #d4af37;
            font-family: 'Cinzel', serif;
            font-size: 40px;
            font-weight: 900;
            margin: 0 0 10px 0;
            text-shadow: 0 3px 12px rgba(0,0,0,0.8);
        }

        .page-subtitle {
            color: #cbd5e1;
            font-size: 18px;
            margin: 0;
        }

        /* שורת הסינון ומספר הדגמים */
        .controls-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            gap: 16px;
            background: rgba(15, 23, 42, 0.85);
            border: 1.5px solid rgba(212, 175, 55, 0.5);
            border-radius: 14px;
            padding: 16px 22px;
            margin-bottom: 30px;
        }

        .count-text {
            color: #e2e8f0;
            font-size: 17px;
        }

            .count-text b {
                color: #d4af37;
                font-size: 22px;
            }

        .filter-label {
            color: #cbd5e1;
            margin-left: 10px;
            font-weight: 600;
        }

        .filter-ddl {
            background: #0f172a;
            color: #ffffff;
            border: 1.5px solid #d4af37;
            border-radius: 10px;
            padding: 8px 14px;
            font-size: 16px;
            font-family: 'Assistant', sans-serif;
            cursor: pointer;
        }

        /* רשת הכרטיסים - 3 בשורה, ובמסך קטן אחד מתחת לשני */
        .fleet-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 26px;
        }

        @media (max-width: 1000px) {
            .fleet-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 650px) {
            .fleet-grid {
                grid-template-columns: 1fr;
            }

            .page-title {
                font-size: 30px;
            }
        }

        /* כרטיס של מטוס */
        .plane-card {
            background: rgba(15, 23, 42, 0.95);
            border: 1px solid #334155;
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 15px 35px rgba(0,0,0,0.6);
            transition: transform 0.25s ease, border-color 0.25s ease;
        }

            .plane-card:hover {
                transform: translateY(-5px);
                border-color: #d4af37;
            }

        .plane-img {
            width: 100%;
            aspect-ratio: 16 / 10;
            object-fit: cover;
            display: block;
            background-color: #1e293b;
        }

        .plane-info {
            padding: 18px 22px 22px 22px;
        }

        .maker-badge {
            display: inline-block;
            background: rgba(212, 175, 55, 0.15);
            color: #d4af37;
            border: 1px solid rgba(212, 175, 55, 0.6);
            border-radius: 20px;
            padding: 2px 12px;
            font-size: 13px;
            font-weight: 700;
            letter-spacing: 1px;
        }

        .plane-name {
            font-family: 'Cinzel', serif;
            font-size: 26px;
            font-weight: 700;
            color: #ffffff;
            margin: 10px 0 4px 0;
            direction: ltr;
            text-align: right;
        }

        .plane-body {
            color: #94a3b8;
            font-size: 15px;
            margin-bottom: 16px;
        }

        /* נתוני המטוס - מושבים וטווח */
        .plane-specs {
            display: flex;
            gap: 12px;
        }

        .spec-box {
            flex: 1;
            background: #1e293b;
            border-radius: 10px;
            padding: 10px 12px;
            text-align: center;
        }

        .spec-value {
            display: block;
            color: #d4af37;
            font-size: 20px;
            font-weight: 700;
        }

        .spec-label {
            display: block;
            color: #cbd5e1;
            font-size: 13px;
        }

        /* כפתורי ניווט */
        .footer-nav {
            display: flex;
            gap: 14px;
            flex-wrap: wrap;
            margin-top: 40px;
        }

        .btn-back {
            border: 1.5px solid rgba(203, 213, 225, 0.6);
            color: #cbd5e1;
            padding: 10px 24px;
            border-radius: 10px;
            font-weight: bold;
            text-decoration: none;
            display: inline-block;
            transition: all 0.3s ease;
        }

            .btn-back:hover {
                border-color: #ffffff;
                color: #ffffff;
                background-color: rgba(203, 213, 225, 0.1);
            }

        /* קרדיט לצלמים - חובה לפי הרישיון של התמונות */
        .credits {
            margin-top: 40px;
            color: #64748b;
            font-size: 12px;
            line-height: 1.7;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">

            <!-- כותרות -->
            <div class="header-section">
                <p class="brand-title">GENESIS AIRWAYS</p>
                <h1 class="page-title">צי החברה</h1>
                <p class="page-subtitle">כל סוגי המטוסים שמפעילה Genesis Airways</p>
            </div>

            <!-- סינון לפי יצרן ומספר הדגמים -->
            <div class="controls-row">
                <span class="count-text">מציג <b><asp:Literal ID="litCount" runat="server"></asp:Literal></b> דגמי מטוסים</span>
                <div>
                    <span class="filter-label">יצרן:</span>
                    <asp:DropDownList ID="ddlMaker" runat="server" CssClass="filter-ddl" AutoPostBack="True" OnSelectedIndexChanged="ddlMaker_SelectedIndexChanged">
                        <asp:ListItem Text="כל היצרנים" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Airbus" Value="Airbus"></asp:ListItem>
                        <asp:ListItem Text="Boeing" Value="Boeing"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <!-- כאן הקוד שם את כרטיסי המטוסים מטבלת AircraftType -->
            <div class="fleet-grid">
                <asp:Literal ID="litFleet" runat="server"></asp:Literal>
            </div>

            <!-- ניווט -->
            <div class="footer-nav">
                <a href="Fleet.aspx" class="btn-back">← חזרה ל"מי אנחנו"</a>
                <a href="HomePage.aspx" class="btn-back">← חזרה לדף הבית</a>
            </div>

            <!-- קרדיט לתמונות -->
            <div class="credits">
                התמונות מ-Wikimedia Commons:
                A320, A321, B777-200ER, B777-300ER – Acroterion (CC BY-SA 4.0) ·
                A350-900 – Md Shaifuzzaman Ayon (CC BY-SA 4.0) ·
                A350-1000 – Mitchul Hope (CC BY-SA 4.0) ·
                A380-800 – Konstantin von Wedelstaedt (GFDL 1.2) ·
                B787-8 – Lord of the Wings (CC BY-SA 2.0) ·
                B787-9, B737-800MAX – Sg1959 (CC BY-SA 4.0) ·
                B777X – Dan Nevill (CC BY 2.0) ·
                B737-900ER – 4300streetcar (CC BY 4.0)
            </div>

        </div>
    </form>
</body>
</html>
