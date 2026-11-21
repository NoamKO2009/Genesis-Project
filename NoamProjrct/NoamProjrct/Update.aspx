<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="NoamProjrct.Update" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genesis Airways - עדכון פרטי משתמש</title>
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
            padding: 40px;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }

        .update-container {
            width: 100%;
            max-width: 600px;
            background: rgba(15, 23, 42, 0.97);
            backdrop-filter: blur(16px);
            border: 2.5px solid rgba(212, 175, 55, 0.8);
            padding: 45px;
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

        .update-title {
            color: #d4af37;
            font-family: 'Cinzel', serif;
            font-size: 22px;
            text-align: center;
            margin-top: 0;
            margin-bottom: 30px;
            font-weight: bold;
            letter-spacing: 1.5px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        label {
            display: block;
            font-size: 14px;
            color: #cbd5e1;
            margin-bottom: 7px;
            font-weight: 600;
            letter-spacing: 0.5px;
        }

        input[type="text"], input[type="password"] {
            width: 100%;
            padding: 13px 15px;
            background-color: #1e293b;
            border: 2px solid #334155;
            color: white;
            border-radius: 12px;
            font-size: 15px;
            font-family: 'Assistant', sans-serif;
            outline: none;
            transition: border-color 0.3s;
        }

        input[type="text"]:focus, input[type="password"]:focus {
            border-color: #d4af37;
        }

        .btn-submit {
            background: linear-gradient(135deg, #d4af37 0%, #aa7c11 100%);
            color: #0b0f19;
            width: 100%;
            padding: 15px;
            border: none;
            border-radius: 12px;
            cursor: pointer;
            font-size: 17px;
            font-weight: bold;
            font-family: 'Cinzel', serif;
            letter-spacing: 1.5px;
            transition: transform 0.2s, opacity 0.2s;
            box-shadow: 0 6px 20px rgba(212, 175, 55, 0.4);
            margin-top: 15px;
        }

        .btn-submit:hover {
            opacity: 0.95;
            transform: translateY(-2px);
        }

        .msg-container {
            margin-top: 15px;
            text-align: center;
            font-size: 15px;
            font-weight: bold;
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
            margin-top: 35px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="update-container">
            <div class="brand-title">GENESIS AIRWAYS</div>
            <h2 class="update-title">✏️ עדכון פרטי משתמש</h2>

            <div class="form-group">
                <label>שם משתמש קיים:</label>
                <asp:TextBox ID="up4" runat="server" placeholder="הכנס שם משתמש נוכחי"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>סיסמה קיימת:</label>
                <asp:TextBox ID="up5" runat="server" TextMode="Password" placeholder="הכנס סיסמה נוכחית"></asp:TextBox>
            </div>

            <hr style="border: 0; border-top: 1px solid #334155; margin: 25px 0;" />

            <div class="form-group">
                <label>שם משתמש חדש:</label>
                <asp:TextBox ID="up1" runat="server" placeholder="הכנס שם משתמש חדש"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>סיסמה חדשה:</label>
                <asp:TextBox ID="up2" runat="server" TextMode="Password" placeholder="הכנס סיסמה חדשה"></asp:TextBox>
            </div>

            <asp:Button ID="Button1" runat="server" Text="עדכן פרטים" CssClass="btn-submit" OnClick="Updatebutton" />

            <div class="msg-container">
                <asp:Label ID="lblMsg" runat="server" ForeColor="#4ade80"></asp:Label>
            </div>

            <div class="back-container">
                <a href="HomePage.aspx" class="btn-top">← חזרה לדף הבית</a>
            </div>
        </div>
    </form>
</body>
</html>