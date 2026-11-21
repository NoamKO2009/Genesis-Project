<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="NoamProjrct.Users" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genesis Airways - ניהול משתמשים</title>
    <!-- ייבוא פונטים יוקרתיים מ-Google Fonts -->
    <link href="styles/ShowUsers.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">

            <!-- כותרת וכפתור חזרה -->
            <div class="page-header">
                <h1 class="page-title">ניהול משתמשים</h1>
                <a href="HomePage.aspx" class="btn-back">← חזרה לדף הבית</a>
            </div>

            <!-- אזור הסינון והכפתורים -->
            <div class="controls-card">
                <div class="field-group">
                    <span class="field-label">סינון לפי רמת Elite:</span>
                    <asp:DropDownList ID="ShowTier" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ShowCities_OnSelectedIndexChanged" CssClass="styled-dropdown">
                    </asp:DropDownList>
                </div>
                <div class="field-group">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="רענן נתונים" CssClass="styled-button" />
                </div>
            </div>

            <!-- טבלת הצגת הנתונים (תופסת כעת 100% מרוחב המסך) -->
            <div class="grid-container">
                <asp:GridView ID="GridView1" runat="server" CssClass="styled-grid" GridLines="None">
                </asp:GridView>
            </div>

        </div>
    </form>
</body>
</html>
