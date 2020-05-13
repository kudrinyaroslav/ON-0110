<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadScript.aspx.cs" Inherits="CameraWebService.LoadScript" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    </head>
<body>
    <form id="form1" runat="server">
    <div style="height: 71px">

        <br />
        <asp:FileUpload ID="fileUpload" runat="server" />
        <br />
        <asp:Button ID="btnLoad" runat="server" onclick="ButtonLoad_Click" Text="Load" />
    
        <br />
        <asp:Button ID="btnReset" runat="server" onclick="ButtonReset_Click" Text="Reset" />


    </div>
    </form>
</body>
</html>
