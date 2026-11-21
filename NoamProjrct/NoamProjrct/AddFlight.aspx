<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddFlight.aspx.cs" Inherits="NoamProjrct.AddFlight" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genesis Airways - הוספת טיסה חדשה</title>
    <link href="https://fonts.googleapis.com/css2?family=Cinzel:wght@500;700;900&family=Playfair+Display:wght@500;700&family=Assistant:wght@300;400;600;700&display=swap" rel="stylesheet" />
    <link href="styles/AddFlightCss.css" rel="stylesheet" type="text/css" />
    <style>
        /* הוספת התאמה ל-input[type="date"] שייראה בדיוק כמו שאר השדות היוקרתיים */
        input[type="text"], input[type="date"], select, .search-box-select {
            width: 100%;
            padding: 13px 15px;
            background-color: #1e293b;
            border: 2px solid #334155;
            color: white;
            border-radius: 12px;
            font-size: 15px;
            font-family: 'Assistant', sans-serif;
            outline: none;
            transition: border-color 0.3s, box-shadow 0.3s;
        }

        input[type="text"]:focus, input[type="date"]:focus, select:focus, .search-box-select:focus {
            border-color: #d4af37;
            box-shadow: 0 0 0 3px rgba(212, 175, 55, 0.18);
        }

        /* התאמת צבע האייקון של לוח השנה בדפדפנים תומכים */
        input[type="date"]::-webkit-calendar-picker-indicator {
            filter: invert(1) brightness(0.8) sepia(1) hue-rotate(5deg) saturate(3);
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="admin-container">
            <div class="brand-title">GENESIS AIRWAYS</div>
            <h2 class="admin-title">🛠️ פאנל ניהול - הוספת טיסה חדשה</h2>

            <div class="form-row">
                <div class="form-group">
                    <label>מספר טיסה (FlightNumber):</label>
                    <asp:TextBox ID="txtFlightNumber" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>דגם מטוס:</label>
                    <asp:DropDownList ID="txtAirplaneModel" runat="server" CssClass="search-box-select"></asp:DropDownList>
                </div>
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label>מוצא (Origin):</label>
                    <asp:DropDownList ID="ddlOrigin" runat="server" CssClass="search-box-select"></asp:DropDownList>
                </div>

                <div class="form-group">
                    <label>יעד (Destination):</label>
                    <asp:DropDownList ID="ddlDestination" runat="server" CssClass="search-box-select"></asp:DropDownList>
                </div>
            </div>

            <div class="form-row">
                <!-- שדה תאריך מעוצב עם לוח שנה מובנה -->
                <div class="form-group">
                    <label>תאריך (FlightDate):</label>
                    <asp:TextBox ID="txtFlightDate" runat="server" TextMode="Date"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>שעה (FlightTime):</label>
                    <asp:TextBox ID="txtFlightTime" runat="server" placeholder="HH:MM"></asp:TextBox>
                </div>
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label>סה"כ מושבים (TotalSeats):</label>
                    <asp:TextBox ID="txtTotalSeats" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>מחיר (Price):</label>
                    <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
                </div>
            </div>

            <asp:Button ID="BtnInsert" runat="server"
                OnClick="BtnInsert_Click"
                Text="הוסף טיסה למסד הנתונים"
                CssClass="btn-submit" />

            <div class="msg-container">
                <asp:Label ID="lblMsg" runat="server" ForeColor="#4ade80"></asp:Label>
            </div>

            <div style="margin-top: 35px; text-align: center;">
                <a href="HomePage.aspx" class="btn-top">← חזרה לדף הבית</a>
            </div>
        </div>
    </form>
</body>
</html>