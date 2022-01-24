<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dtest.aspx.cs" Inherits="dtest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Session ist:
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>

        <br />
        <asp:Button ID="Button1" runat="server" Text="Session laden" OnClick="Button1_Click" />
        <br /><br />
        <asp:Button ID="Button2" runat="server" Text="Gehe zu Datatrans" OnClick="Button2_Click" />
    </div>
    </form>
</body>
</html>
