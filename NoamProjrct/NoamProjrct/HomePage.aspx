<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="NoamProjrct.HomePage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genesis Airways - דף הבית</title>
    <!-- ייבוא פונטים יוקרתיים מ-Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Cinzel:wght@500;700;900&family=Playfair+Display:wght@500;700&family=Assistant:wght@300;400;600;700&display=swap" rel="stylesheet" />
    <link href="styles/HomePage.css" rel="stylesheet" type="text/css" />
    <style>
        .login-required-msg {
            background: rgba(239, 68, 68, 0.1);
            border: 1.5px solid rgba(248, 113, 113, 0.6);
            color: #fca5a5;
            padding: 15px;
            border-radius: 12px;
            text-align: center;
            font-size: 16px;
            font-weight: 600;
            margin-top: 20px;
        }
        .login-required-msg a {
            color: #d4af37;
            text-decoration: underline;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">
            <!-- פס עליון -->
            <div class="top-bar">
                <div style="display: flex; align-items: center; gap: 25px;">
                    <div class="logo-container">
                        <a href="Fleet.aspx" title="Genesis Airways - Fleet & About Us">
                            <img src="images/Logo.png" alt="Genesis Airways Logo" class="brand-logo" />
                        </a>
                    </div>
                    <div>
                        <div class="welcome-msg">
                            <asp:Literal ID="litWelcome" runat="server"></asp:Literal>
                        </div>
                        <asp:Panel ID="pnlUserStats" runat="server" CssClass="user-stats" Visible="false">
                            <span class="stat-chip">Elite Tier: <b>
                                <asp:Literal ID="litTier" runat="server"></asp:Literal></b></span>
                            <span class="stat-chip">Elite Points: <b>
                                <asp:Literal ID="litPoints" runat="server"></asp:Literal></b></span>
                        </asp:Panel>
                    </div>
                </div>

                <div class="auth-buttons">
                    <asp:Button ID="BtnLogin" runat="server" Text="התחבר" CssClass="btn-auth" OnClick="BtnLogin_Click" />
                    <asp:Button ID="BtnRegister" runat="server" Text="הירשם" CssClass="btn-auth" OnClick="BtnRegister_Click" />
                    <asp:Button ID="BtnUpdate" runat="server" Text="עדכון פרטים" CssClass="btn-auth" OnClick="BtnUpdate_Click" Visible="false" />
                    <asp:Button ID="BtnLogout" runat="server" Text="התנתק" CssClass="btn-logout" OnClick="BtnLogout_Click" />
                </div>
            </div>

            <!-- באנר ראשי -->
            <div class="hero-banner">
                <img src="images/Genesis.jpg" alt="Genesis Airways" class="bg-img" />
                <div class="hero-content">
                    <h1 class="brand-title">GENESIS AIRWAYS</h1>
                    <p class="hero-subtitle">חווה את עולם התעופה ברמה הגבוהה ביותר</p>
                </div>
            </div>

            <!-- קופסת הזמנת טיסה חופפת -->
            <div class="booking-card" style="max-width: 1400px; width: 100%;">
                <div class="booking-title">הזמנת טיסה</div>
                <div class="search-grid" style="grid-template-columns: 1fr 1fr 1fr 1fr 1fr auto;">
                    <div class="field-group">
                        <span class="field-label">מוצא (Origin)</span>
                        <asp:DropDownList ID="ddlOrigin" runat="server" CssClass="search-box-select"></asp:DropDownList>
                    </div>
                    <div class="field-group">
                        <span class="field-label">יעד (Destination)</span>
                        <asp:DropDownList ID="ddlDestination" runat="server" CssClass="search-box-select"></asp:DropDownList>
                    </div>
                    <div class="field-group">
                        <span class="field-label">תאריך מוצא</span>
                        <asp:TextBox ID="txtDepartureDate" runat="server" CssClass="search-box-select" TextMode="Date" Style="height: 52px;"></asp:TextBox>
                    </div>
                    <div class="field-group">
                        <span class="field-label">תאריך חזרה</span>
                        <asp:TextBox ID="txtReturnDate" runat="server" CssClass="search-box-select" TextMode="Date" Style="height: 52px;"></asp:TextBox>
                    </div>
                    <div class="field-group">
                        <span class="field-label">נוסעים (Passengers)</span>
                        <asp:TextBox ID="txtPassengers" runat="server" CssClass="search-box-select" placeholder="מס' נוסעים" Style="height: 52px;"></asp:TextBox>
                    </div>
                    <div class="field-group" style="justify-content: flex-end;">
                        <!-- כפתור החיפוש יוצג רק למשתמש מחובר -->
                        <asp:Button ID="BtnSearch" runat="server" Text="חפש טיסות" CssClass="btn-search" OnClick="BtnSearch_Click" />
                    </div>
                </div>

                <!-- הודעת שגיאה כשלא מולאו כל פרטי החיפוש -->
                <asp:Label ID="lblSearchError" runat="server" CssClass="login-required-msg" Visible="false" Style="display: block;"></asp:Label>

                <!-- הודעה שתוצג לאורחים בלבד -->
                <asp:Panel ID="pnlLoginRequired" runat="server" Visible="false" CssClass="login-required-msg">
                    עליך <a href="Login.aspx">להתחבר למערכת</a> על מנת לחפש ולהזמין טיסות.
                </asp:Panel>
            </div>

            <!-- אזור מנהל -->
            <div class="admin-area" style="display: flex; gap: 15px; justify-content: center;">
                <asp:Button ID="BtnAdminPanel" runat="server" Text="🛠️ הוספת טיסה / פאנל ניהול" CssClass="btn-admin" OnClick="BtnAdminPanel_Click" />
                <asp:Button ID="BtnAdminShowUsers" runat="server" Text="👥 ניהול משתמשים" CssClass="btn-admin" OnClick="BtnAdminShowUsers_Click" />
                <asp:Button ID="BtnBookings" runat="server" Text="הזמנות" CssClass="btn-admin" OnClick="BtnBookings_Click" />
            </div>
        </div>
    </form>
</body>
</html>