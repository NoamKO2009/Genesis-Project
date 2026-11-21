<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Flights.aspx.cs" Inherits="NoamProjrct.Flights" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genesis Airways - תוצאות חיפוש טיסות</title>
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
            align-items: center;
            margin-bottom: 35px;
            justify-content: space-between;
            flex-wrap: wrap;
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
            transition: all 0.2s ease;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            height: 50px;
        }

            .btn-action:hover {
                opacity: 0.95;
                transform: translateY(-2px);
                box-shadow: 0 8px 22px rgba(212, 175, 55, 0.6);
            }

            /* אפקט לחיצה (Active) - נותן תחושה שהכפתור נלחץ פנימה מיד כשקונטרול עובד */
            .btn-action:active {
                transform: translateY(1px);
                box-shadow: 0 2px 10px rgba(212, 175, 55, 0.3);
                background: linear-gradient(135deg, #aa7c11 0%, #8c630e 100%);
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
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            height: 50px;
        }

            .btn-action:hover {
                opacity: 0.95;
                transform: translateY(-2px);
            }

        /* הודעת שגיאה מעוצבת */
        .error-msg {
            background: rgba(239, 68, 68, 0.1);
            border: 1.5px solid rgba(248, 113, 113, 0.6);
            color: #fca5a5;
            padding: 14px 20px;
            border-radius: 12px;
            text-align: center;
            font-size: 16px;
            font-weight: 600;
            margin-bottom: 25px;
            display: block;
        }

        /* עיצוב טבלת הטיסות */
        .grid-view-style {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="flights-container">
            <div class="brand-title">GENESIS AIRWAYS</div>
            <h2 class="flights-title">תוצאות חיפוש טיסות</h2>

            <div class="controls-row">
                <a href="HomePage.aspx" class="btn-action">← חזור לדף הבית לחיפוש חדש</a>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="הצג את כל הטיסות" CssClass="btn-action" />
            </div>

            <!-- חלק 1: טיסות הלוך -->
            <h3 class="flights-title" style="text-align: right; margin-top: 20px; margin-bottom: 10px; font-size: 19px;">טיסות הלוך</h3>
            <asp:Label ID="lblError" runat="server" CssClass="error-msg" Visible="false"></asp:Label>

            <div style="overflow-x: auto; width: 100%;" dir="rtl">
                <asp:GridView ID="GridView1" runat="server" CssClass="grid-view-style" GridLines="None" Width="100%"
                    AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand" DataKeyNames="מזהה">
                    <Columns>
                        <asp:BoundField DataField="FlightNumber" HeaderText="מספר טיסה" />
                        <asp:BoundField DataField="Origin" HeaderText="מוצא" />
                        <asp:BoundField DataField="Destination" HeaderText="יעד" />
                        <asp:BoundField DataField="FlightDate" HeaderText="תאריך" />
                        <asp:BoundField DataField="FlightTime" HeaderText="שעה" />
                        <asp:BoundField DataField="Price" HeaderText="מחיר ($)" />
                        <asp:BoundField DataField="AvailableSeats" HeaderText="מקומות פנויים" />
                        <asp:ButtonField ButtonType="Button" CommandName="BookFlight" Text="הזמן" ControlStyle-CssClass="btn-action" />
                    </Columns>
                </asp:GridView>
            </div>

            <!-- חלק 2: טיסות חזור (מוצג רק אם נבחר תאריך חזרה) -->
            <asp:Panel ID="pnlReturnFlights" runat="server" Visible="false" Style="margin-top: 45px; border-top: 1px solid rgba(212, 175, 55, 0.3); padding-top: 30px;">
                <h3 class="flights-title" style="text-align: right; margin-bottom: 10px; font-size: 19px;">טיסות חזור</h3>
                <asp:Label ID="lblReturnError" runat="server" CssClass="error-msg" Visible="false"></asp:Label>

                <div style="overflow-x: auto; width: 100%;" dir="rtl">
                    <asp:GridView ID="GridView2" runat="server" CssClass="grid-view-style" GridLines="None" Width="100%"
                        AutoGenerateColumns="False" OnRowCommand="GridView2_RowCommand" DataKeyNames="מזהה">
                        <Columns>
                            <asp:BoundField DataField="FlightNumber" HeaderText="מספר טיסה" />
                            <asp:BoundField DataField="Origin" HeaderText="מוצא" />
                            <asp:BoundField DataField="Destination" HeaderText="יעד" />
                            <asp:BoundField DataField="FlightDate" HeaderText="תאריך" />
                            <asp:BoundField DataField="FlightTime" HeaderText="שעה" />
                            <asp:BoundField DataField="Price" HeaderText="מחיר ($)" />
                            <asp:BoundField DataField="AvailableSeats" HeaderText="מקומות פנויים" />
                            <asp:ButtonField ButtonType="Button" CommandName="BookFlight" Text="הזמן" ControlStyle-CssClass="btn-action" />
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
