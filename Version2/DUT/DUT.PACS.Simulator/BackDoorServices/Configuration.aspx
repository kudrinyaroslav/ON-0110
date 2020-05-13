<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="DUT.PACS.Simulator.BackDoorServices.Configuration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" 
    style="font-family: 'Courier New', Courier, monospace; font-size: small">
    <div>
    
        Configuration file:
        <asp:FileUpload ID="fileSelector" runat="server" Width="221px" />
    
    </div>
    <asp:Button ID="btnConfigure" runat="server" onclick="btnConfigure_Click" 
        Text="Configure" />
    <p>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </p>
    </form>
</body>
</html>
