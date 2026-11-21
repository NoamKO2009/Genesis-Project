<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bookings.aspx.cs" Inherits="NoamProjrct.Bookings" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="he" dir="rtl">
<head runat="server">
    <meta charset="utf-8" />
    <title>Genesis Airways - ניהול והצגת הזמנות טיסה</title>
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
            padding: 40px 20px;
        }

        .flights-container {
            max-width: 1700px;
            width: 100%;
            margin: 0 auto;
            background: rgba(15, 23, 42, 0.97);
            backdrop-filter: blur(16px);
            border: 2.5px solid rgba(212, 175, 55, 0.8);
            padding: 45px 50px;
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
            text-shadow: 0 3px 12px rgba(0,0,0,0.8);
        }

        .flights-title {
            color: #d4af37;
            font-family: 'Cinzel', serif;
            font-size: 22px;
            text-align: center;
            margin-top: 0;
            margin-bottom: 35px;
            font-weight: bold;
            letter-spacing: 1.5px;
        }

        .controls-row {
            display: flex;
            gap: 20px;
            align-items: flex-end;
            margin-bottom: 35px;
            justify-content: center;
            flex-wrap: wrap;
        }

        .field-group {
            display: flex;
            flex-direction: column;
            gap: 8px;
            position: relative;
            z-index: 20;
        }

        .field-label {
            font-size: 14px;
            color: #cbd5e1;
            font-weight: 600;
            letter-spacing: 0.5px;
        }

        .btn-action {
            background: linear-gradient(135deg, #d4af37 0%, #aa7c11 100%);
            color: #0b0f19;
            padding: 12px 28px;
            border: none;
            border-radius: 12px;
            cursor: pointer;
            font-size: 16px;
            font-weight: bold;
            font-family: 'Cinzel', serif;
            letter-spacing: 1px;
            box-shadow: 0 5px 18px rgba(212, 175, 55, 0.4);
            transition: transform 0.2s, opacity 0.2s;
            height: 50px;
        }

        .btn-action:hover {
            opacity: 0.95;
            transform: translateY(-2px);
        }

        /* ===== עיצוב רשימות נגללות (Base-Select) יוקרתיות מלאות ===== */
        @supports (appearance: base-select) {
            select,
            select::picker(select) {
                appearance: base-select;
            }
        }

        .search-box-select {
            padding: 12px 18px;
            border-radius: 12px;
            font-size: 16px;
            font-family: 'Assistant', sans-serif;
            outline: none;
            min-width: 260px;
            height: 50px;
            cursor: pointer;
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: linear-gradient(145deg, #1e293b 0%, #162032 100%) !important;
            border: 2px solid #334155;
            color: white;
            transition: border-color 0.3s, box-shadow 0.3s;
        }

        .search-box-select:hover {
            border-color: rgba(212, 175, 55, 0.7);
        }

        .search-box-select:focus,
        .search-box-select:open {
            border-color: #d4af37 !important;
            box-shadow: 0 0 0 3px rgba(212, 175, 55, 0.18);
        }

        .search-box-select::picker-icon {
            color: #d4af37;
            transition: rotate 0.3s;
        }

        .search-box-select:open::picker-icon {
            rotate: 180deg;
        }

        .search-box-select::picker(select) {
            direction: rtl;
            margin-top: 8px;
            padding: 6px;
            max-height: 240px;
            background: #0f172a;
            border: 2px solid rgba(212, 175, 55, 0.7);
            border-radius: 14px;
            box-shadow: 0 20px 45px rgba(0, 0, 0, 0.85);
            scrollbar-width: thin;
            scrollbar-color: #d4af37 transparent;
            opacity: 0;
            transform: translateY(4px);
            transition: opacity 0.25s, transform 0.25s, display 0.25s allow-discrete, overlay 0.25s allow-discrete;
        }

        .search-box-select:open::picker(select) {
            opacity: 1;
            transform: translateY(0);
        }

        @starting-style {
            .search-box-select:open::picker(select) {
                opacity: 0;
                transform: translateY(4px);
            }
        }

        .search-box-select option {
            padding: 11px 14px;
            border-radius: 9px;
            background: transparent;
            color: #cbd5e1;
            font-size: 15px;
            cursor: pointer;
            transition: background 0.2s, color 0.2s;
        }

        .search-box-select option::checkmark {
            display: none;
        }

        .search-box-select option:hover,
        .search-box-select option:focus {
            background: linear-gradient(90deg, rgba(212,175,55,0.25), rgba(212,175,55,0.08));
            color: #d4af37;
        }

        .search-box-select option:checked {
            background: rgba(212, 175, 55, 0.18);
            color: #d4af37;
            font-weight: 700;
        }

        /* עיצוב טבלת ההזמנות */
        .grid-view-style {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
            background-color: #1e293b;
            border-radius: 12px;
            overflow: hidden;
            border: 1px solid #334155;
            font-family: 'Assistant', sans-serif;
            direction: rtl;
            text-align: right;
        }

        .grid-view-style th {
            background-color: #0f172a;
            color: #d4af37;
            padding: 16px 20px;
            font-size: 16px;
            font-family: 'Cinzel', serif;
            letter-spacing: 1px;
            border-bottom: 2px solid #d4af37;
            text-align: right;
            white-space: nowrap;
        }

        .grid-view-style td {
            padding: 16px 20px;
            color: #e2e8f0;
            border-bottom: 1px solid #334155;
            font-size: 15px;
            text-align: right;
        }

        .grid-view-style tr:hover {
            background-color: rgba(212, 175, 55, 0.05);
        }

        .btn-top {
            background: transparent;
            border: 1.5px solid rgba(203, 213, 225, 0.6);
            color: #cbd5e1;
            padding: 10px 24px;
            border-radius: 10px;
            font-weight: bold;
            font-family: 'Cinzel', serif;
            letter-spacing: 1px;
            cursor: pointer;
            transition: all 0.3s ease;
            text-decoration: none;
            font-size: 14px;
            display: inline-block;
        }

        .btn-top:hover {
            border-color: #ffffff;
            color: #ffffff;
            background-color: rgba(203, 213, 225, 0.1);
        }

        .back-container {
            margin-top: 40px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="flights-container">
            <div class="brand-title">GENESIS AIRWAYS</div>
            <h2 class="flights-title">ניהול והצגת הזמנות טיסה</h2>

            <div class="controls-row">
                <asp:Button ID="BtnShowAll" runat="server" OnClick="BtnShowAll_Click" Text="הצג את כל ההזמנות" CssClass="btn-action" />
                
                <div class="field-group">
                    <span class="field-label">סינון לפי שם משתמש (Username)</span>
                    <asp:DropDownList ID="ShowUsers" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ShowUsers_OnSelectedIndexChanged" CssClass="search-box-select">
                    </asp:DropDownList>
                </div>
            </div>

            <div style="overflow-x: auto; width: 100%;" dir="rtl">
                <asp:GridView ID="GridView1" runat="server" CssClass="grid-view-style" GridLines="None" Width="100%">
                </asp:GridView>
            </div>

            <div class="back-container">
                <a href="HomePage.aspx" class="btn-top">← חזרה לדף הבית</a>
            </div>
        </div>
    </form>
</body>
</html>