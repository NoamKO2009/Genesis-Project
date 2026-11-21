<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookFlight.aspx.cs" Inherits="NoamProjrct.BookFlight" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genesis Airways - טופס הזמנת טיסה</title>
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
        .booking-container {
            max-width: 800px;
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
        }
        .booking-title {
            color: #d4af37;
            font-family: 'Cinzel', serif;
            font-size: 22px;
            text-align: center;
            margin-top: 0;
            margin-bottom: 35px;
            font-weight: bold;
        }
        .flight-info-box {
            background: rgba(30, 41, 59, 0.7);
            border: 1px solid rgba(212, 175, 55, 0.4);
            padding: 20px;
            border-radius: 14px;
            margin-bottom: 30px;
            font-size: 16px;
            color: #cbd5e1;
        }
        .form-group {
            display: flex;
            flex-direction: column;
            gap: 8px;
            margin-bottom: 20px;
        }
        .field-label {
            font-size: 15px;
            color: #cbd5e1;
            font-weight: 600;
        }
        .form-input {
            padding: 12px 18px;
            border-radius: 12px;
            font-size: 16px;
            font-family: 'Assistant', sans-serif;
            background: linear-gradient(145deg, #1e293b 0%, #162032 100%);
            border: 2px solid #334155;
            color: white;
            outline: none;
            height: 50px;
        }
        .form-input:focus {
            border-color: #d4af37;
            box-shadow: 0 0 0 3px rgba(212, 175, 55, 0.18);
        }
        .btn-submit {
            background: linear-gradient(135deg, #d4af37 0%, #aa7c11 100%);
            color: #0b0f19;
            padding: 14px 28px;
            border: none;
            border-radius: 12px;
            cursor: pointer;
            font-size: 18px;
            font-weight: bold;
            font-family: 'Cinzel', serif;
            letter-spacing: 1px;
            width: 100%;
            margin-top: 20px;
            box-shadow: 0 5px 18px rgba(212, 175, 55, 0.4);
            transition: transform 0.2s;
        }
        .btn-submit:hover {
            transform: translateY(-2px);
        }
        .success-msg {
            background: rgba(34, 197, 94, 0.15);
            border: 1.5px solid rgba(34, 197, 94, 0.6);
            color: #4ade80;
            padding: 15px;
            border-radius: 12px;
            text-align: center;
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 25px;
        }
        .error-msg {
            background: rgba(239, 68, 68, 0.1);
            border: 1.5px solid rgba(248, 113, 113, 0.6);
            color: #fca5a5;
            padding: 15px;
            border-radius: 12px;
            text-align: center;
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 25px;
        }
        .back-link {
            text-align: center;
            margin-top: 30px;
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
        <div class="booking-container">
            <div class="brand-title">GENESIS AIRWAYS</div>
            <h2 class="booking-title">טופס אישור הזמנה</h2>

            <!-- הודעות הצלחה / שגיאה -->
            <asp:Label ID="lblSuccess" runat="server" CssClass="success-msg" Visible="false"></asp:Label>
            <asp:Label ID="lblError" runat="server" CssClass="error-msg" Visible="false"></asp:Label>

            <!-- הצגת פרטי הטיסה הנבחרת -->
            <div class="flight-info-box">
                <span style="color: #d4af37; font-weight: bold; font-family: 'Cinzel', serif; display: block; margin-bottom: 5px;">פרטי הטיסה שנבחרה:</span>
                <asp:Label ID="lblFlightDetails" runat="server" Text="טוען פרטי טיסה..."></asp:Label>
            </div>

            <!-- שדות מילוי פרטי הנוסע (תואמים לטבלת Bookings במסד הנתונים) -->
            <div class="form-group">
                <span class="field-label">שם משתמש (Username)</span>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" placeholder="הכנס שם משתמש"></asp:TextBox>
            </div>

            <div class="form-group">
                <span class="field-label">שם הנוסע (Passenger Name)</span>
                <asp:TextBox ID="txtPassengerName" runat="server" CssClass="form-input" placeholder="הכנס שם מלא"></asp:TextBox>
            </div>

            <div class="form-group">
                <span class="field-label">מספר טלפון (Phone)</span>
                <asp:TextBox ID="txtPassengerPhone" runat="server" CssClass="form-input" placeholder="050-0000000"></asp:TextBox>
            </div>

            <asp:Button ID="BtnConfirmBooking" runat="server" Text="אישור וביצוע הזמנה" CssClass="btn-submit" OnClick="BtnConfirmBooking_Click" />

            <div class="back-link">
                <a href="Flights.aspx">← חזרה לרשימת הטיסות</a>
            </div>
        </div>
    </form>
</body>
</html>