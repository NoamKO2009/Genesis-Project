<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="NoamProjrct.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genesis Airways - התחברות</title>
    <!-- ייבוא פונטים יוקרתיים ואלגנטיים מ-Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Cinzel:wght@500;700;900&family=Playfair+Display:wght@500;700&family=Assistant:wght@300;400;600;700&display=swap" rel="stylesheet" />
    <style>
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
            box-sizing: border-box;
        }

        .login-container {
            width: 100%;
            max-width: 450px;
            background: rgba(15, 23, 42, 0.98);
            backdrop-filter: blur(12px);
            border: 2px solid rgba(212, 175, 55, 0.6);
            padding: 40px;
            border-radius: 22px;
            box-shadow: 0 25px 60px rgba(0, 0, 0, 0.9);
            box-sizing: border-box;
        }

        .brand-title {
            text-align: center;
            color: #ffffff;
            font-family: 'Cinzel', serif;
            font-size: 26px;
            font-weight: 900;
            letter-spacing: 3px;
            margin-bottom: 5px;
        }

        .login-title {
            color: #d4af37;
            font-family: 'Cinzel', serif;
            font-size: 22px;
            text-align: center;
            margin-top: 0;
            margin-bottom: 30px;
            font-weight: bold;
            letter-spacing: 1px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        label {
            display: block;
            font-size: 14px;
            color: #e2e8f0;
            margin-bottom: 6px;
            font-weight: 600;
            letter-spacing: 0.5px;
        }

        input[type="text"], input[type="password"] {
            width: 100%;
            padding: 12px 14px;
            background-color: #1e293b;
            border: 2px solid #334155;
            color: white;
            border-radius: 10px;
            font-size: 15px;
            font-family: 'Assistant', sans-serif;
            box-sizing: border-box;
            outline: none;
            transition: border-color 0.3s;
        }

            input[type="text"]:focus, input[type="password"]:focus {
                border-color: #d4af37;
            }

        /* שדה סיסמה עם כפתור הצגה/הסתרה */
        .password-wrapper {
            position: relative;
        }

            .password-wrapper input {
                padding-left: 48px; /* מקום לכפתור בצד שמאל */
            }

        .btn-eye {
            position: absolute;
            left: 10px;
            top: 50%;
            transform: translateY(-50%);
            background: transparent;
            border: none;
            cursor: pointer;
            font-size: 18px;
            color: #cbd5e1;
            padding: 4px 6px;
            line-height: 1;
            transition: color 0.2s;
        }

            .btn-eye:hover {
                color: #d4af37;
            }

        .btn-submit {
            background: linear-gradient(135deg, #d4af37 0%, #aa7c11 100%);
            color: #0b0f19;
            width: 100%;
            padding: 14px;
            border: none;
            border-radius: 10px;
            cursor: pointer;
            font-size: 17px;
            font-weight: bold;
            font-family: 'Cinzel', serif;
            letter-spacing: 1.5px;
            transition: transform 0.2s, opacity 0.2s;
            box-shadow: 0 5px 15px rgba(212, 175, 55, 0.4);
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
    </style>

    <script type="text/javascript">
        function togglePassword() {
            var txt = document.getElementById('<%= txtPassword.ClientID %>');
            var btn = document.getElementById('btnEye');

            if (txt.type === 'password') {
                txt.type = 'text';
                btn.innerHTML = '🙈';
                btn.title = 'הסתר סיסמה';
            } else {
                txt.type = 'password';
                btn.innerHTML = '👁️';
                btn.title = 'הצג סיסמה';
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="brand-title">GENESIS AIRWAYS</div>
            <h2 class="login-title">🔐 התחברות למערכת</h2>

            <div class="form-group">
                <label>שם משתמש:</label>
                <asp:TextBox ID="txtUsername" runat="server" placeholder="הכנס שם משתמש"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>סיסמה:</label>
                <div class="password-wrapper">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="הכנס סיסמה"></asp:TextBox>
                    <button type="button" id="btnEye" class="btn-eye" onclick="togglePassword()" title="הצג סיסמה">👁️</button>
                </div>
            </div>

            <asp:Button ID="BtnSubmitLogin" runat="server" Text="התחבר" CssClass="btn-submit" OnClick="BtnLogin_Click" />

            <div class="msg-container">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>

            <div style="margin-top: 40px; text-align: center;">
                <asp:Button ID="ReturnToHompage" runat="server" Text="← חזרה לדף הבית" CssClass="btn-top" OnClick="ReturnToHompage_Click" />
            </div>
        </div>
    </form>
</body>
</html>
