<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DUT.CameraWebService.TestSettings" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="DUT.CameraWebService" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        a {
            color: #4682B4;
            text-decoration: none;
        }

        .main {
            background-color: #4682B4;
            color: #ffffff;
        }

        .validation {
            color: Red;
        }

        .form-line {
            background-color: #ffffff;
        }

        .menu-item {
            background-color: #4169E1;
            width: 120px;
            text-align: center;
            background-color: #ffffff;
            color: #4682B4;
        }
        .auto-style1 {
            height: 23px;
        }
        .auto-style2 {
            height: 30px;
        }
    </style>
</head>
<body class="main">
    <form id="form1" runat="server">
        <div>
            m_credentilk<table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <h1>ONVIF DUT Simulator 1.01</h1>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="menu" cellspacing="5px">
                            <tr>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Main" OnCommand="OnMenuClick">
                        <div class="menu-item">Main</div>
                                    </asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="TestSetting" OnCommand="OnMenuClick">
                        <div  class="menu-item">Test Settings</div>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="form-line" style="height: 2px" />
                </tr>
                <tr>
                    <td>
                        <asp:MultiView ID="mvMain" runat="Server" ActiveViewIndex="1">
                            <asp:View ID="TestSetting" runat="Server">
                                <table>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="LabelError" CssClass="validation" runat="server" Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Button ID="ClearAuth" Text="Clear Auth"
                                                OnCommand="OnClearAuth" runat="Server" Width="280px" />
                                            <asp:Button ID="ClearUsersCredentials" Text="Clear users credentials"
                                                OnCommand="OnClearCredentials" runat="Server" Width="280px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <p>
                                                Load test suite file and select test case:
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Test Suite:
                                        </td>
                                        <td width="300px">
                                            <asp:Label ID="LabelTestSuite" runat="server" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td />
                                        <td>
                                            <asp:FileUpload ID="TestSuiteFile" runat="Server" Width="300" />
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="Button4" Text="Load" OnCommand="OnLoadTestSuite" runat="Server" Width="126" />
                                        </td>
                                        <td></td>
                                        <td class="auto-style2">
                                            <asp:Label ID="TestReportCreated" Text="" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td />
                                        <td />
                                        <td align="left">
                                            <asp:Button ID="Button2" Text="Get Test List"
                                                OnCommand="OnGetTestList" runat="Server" Width="126" />
                                        </td>
                                        <td></td>
                                        <td class="auto-style2">
                                            <asp:TextBox ID="TicketID" Text="Input ticket ID" runat="server" />
                                        </td>     


                                    </tr>
                                                                        <tr>
                                        <td colspan="4" class="auto-style1">
                                            <p>
                                                
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style2">Test Case:
                                        </td>
                                        <td class="auto-style2">
                                            <asp:Label ID="LabelTestCase" runat="server" />
                                        </td>
                                        <td align="left" class="auto-style2">
                                            <asp:Button ID="Button5" Text="Reset" OnCommand="OnReset" runat="Server" Width="126" />
                                        </td>
                                        <td class="auto-style2"></td>
                                        <td class="auto-style2">
                                            <asp:Button ID="ButtonSwitchMedia2Service" runat="server" OnClick="OnSwitchMedia2Service" Text="Switch Media2 Service" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td />
                                        <td>
                                            <asp:DropDownList ID="ListTestCases" runat="Server" Width="300" />
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="Button1" Text="Load" OnCommand="OnLoadTestCase" runat="Server" Width="126" />
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="LabelSwitchMedia2Service" runat="server" Text="Old Media2 Service is enabled"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Test Name:
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="LabelTestName" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Test Description:
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="LabelTestDescription" runat="server" />
                                        </td>
                                        <td valign="top">
                                                                                                                                    <asp:Button ID="ButtonGetCurrentResult" Text="Get Current Result"
                                                OnCommand="OnGetCurrentResult" runat="Server" Width="126" />
                                            <asp:Button ID="ClearTestList" Text="Clear"
                                                OnCommand="OnClearTestList" runat="Server" Width="126px" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">Expected Result:
                                        </td>
                                        <td colspan="3" valign="top">
                                            <asp:Label ID="LabelExpectedResult" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="LabelCurrentResult" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="About" runat="Server">
                                <p>
                                    ONVIF DUT Simulator is ASP.NET 2.0 xml web service, emulating behavior of onvif
                                server.<br />
                                    Device management entry point is:
                                <%
                                    string url = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceDevice10/DeviceServiceFake.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url));
                                %>
                                    <br />
                                    <br />
                                    Imaging 20 entry point is:
                                <%
                                    string url2 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceImaging20/ImagingService20.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url2));
                                %>
                                    <br />
                                    DeviceIO 10 entry point is:
                                <%
                                    string url3 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceDeviceIO10/DeviceIOService10.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url3));
                                %>
                                    <br />
                                    Media 10 entry point is:
                                <%
                                    string url4 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceMedia10/MediaService.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url4));
                                %>
                                    <br />
                                    Search 10 entry point is:
                                <%
                                    string url5 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceSearch10/SearchService.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url5));
                                %>
                                    <br />
                                    Recording 10 entry point is:
                                <%
                                    string url6 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceRecording10/RecordingService.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url6));
                                %>
                                    <br />
                                    PACS v03 entry point is:
                                <%
                                    string url7 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServicePACS10/PACSService.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url7));
                                %>
                                    <br />
                                    Door Control v03 entry point is:
                                <%
                                    string url8 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceDoor10/DoorService.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url8));
                                %>
                                    <br />
                                    PACS 10 entry point is:
                                <%
                                    string url9 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServicePACS11/PACSService.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url9));
                                %>
                                    <br />
                                    Door Control 10 entry point is:
                                <%
                                    string url10 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceDoor11/DoorService.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url10));
                                %>
                                    <br />
                                    Advanced Security 10 entry point is:
                                <%
                                    string url11 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceAdvancedSecurity10/AdvancedSecurityService.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url11));
                                    string urlHelp11 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/Help/AdvancedSecurityServiceHelp.txt";
                                    Response.Write(string.Format("  <a href='{0}' style='color:#ffffff;text-decoration:underline;'>[HELP]</a>", urlHelp11));
                                %>
                                    <br />
                                    Pull Point Subscribtion 10 entry point is:
                                <%
                                    string url12 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceEvents10/PullPointSubscribtionService2.asmx";
                                    string urlHelp12 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/Help/EventServiceHelp.txt";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url12));
                                    Response.Write(string.Format("  <a href='{0}' style='color:#ffffff;text-decoration:underline;'>[HELP]</a>", urlHelp12));
                                %>
                                    <br />
                                    Credential 10 entry point is:
                                <%
                                    string url13 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceCredential10/CredentialService.asmx";
                                    string urlHelp13 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/Help/CredentialServiceHelp.txt";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url13));
                                    Response.Write(string.Format("  <a href='{0}' style='color:#ffffff;text-decoration:underline;'>[HELP]</a>", urlHelp13));
                                %>
                                    <br />
                                    Access Rules 10 entry point is:
                                <%
                                    string url14 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceAccessRules10/AccessRulesService.asmx";
                                    string urlHelp14 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/Help/AccessRulesServiceHelp.txt";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url14));
                                    Response.Write(string.Format("  <a href='{0}' style='color:#ffffff;text-decoration:underline;'>[HELP]</a>", urlHelp14));
                                %>
                                    <br />
                                    Schedule 10 entry point is:
                                <%
                                    string url15 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceSchedule10/ServiceSchedule.asmx";
                                    string urlHelp15 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/Help/ScheduleServiceHelp.txt";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url15));
                                    Response.Write(string.Format("  <a href='{0}' style='color:#ffffff;text-decoration:underline;'>[HELP]</a>", urlHelp15));
                                %>
                                    <br />
                                    Media2 10 entry point is:
                                <%
                                    string url16 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceMedia210/Media2Service.asmx";
                                    string urlHelp16 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/Help/Media2ServiceHelp.txt";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url16));
                                    Response.Write(string.Format("  <a href='{0}' style='color:#ffffff;text-decoration:underline;'>[HELP]</a>", urlHelp16));
                                %>
                                    <br />
                                    Media2 SVC entry point is:
                                <%
                                    string url17 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceMedia2SVC/Media2Service.svc";
                                    string urlHelp17 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/Help/Media2SVCServiceHelp.txt";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url17));
                                    Response.Write(string.Format("  <a href='{0}' style='color:#ffffff;text-decoration:underline;'>[HELP]</a>", urlHelp17));
                                %>
                                    <br />
                                    AnalyticsEngine entry point is:
                                <%
                                    string url18 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceAnalytics20/AnalyticsEngineService.asmx";
                                    string urlHelp18 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/Help/AnalyticsServiceHelp.txt";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url18));
                                    Response.Write(string.Format("  <a href='{0}' style='color:#ffffff;text-decoration:underline;'>[HELP]</a>", urlHelp18));
                                %>
                                    <br />
                                    Provisioning entry point is:
                                <%
                                    string url19 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceProvisioning10/ProvisioningService.asmx";
                                    string urlHelp19 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/Help/ProvisioningServiceHelp.txt";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url19));
                                    Response.Write(string.Format("  <a href='{0}' style='color:#ffffff;text-decoration:underline;'>[HELP]</a>", urlHelp19));
                                %>
                                    <br />
                                    Thermal entry point is:
                                <%
                                    string url20 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServiceThermal10/ThermalService.asmx";
                                    string urlHelp20 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/Help/ThermalServiceHelp.txt";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url20));
                                    Response.Write(string.Format("  <a href='{0}' style='color:#ffffff;text-decoration:underline;'>[HELP]</a>", urlHelp20));
                                %>
                                     <br /> 
                                    PACS 12 entry point is:
                                <%
                                    string url21 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/ServicePACS12/PACSService.asmx";
                                    Response.Write(string.Format("<a href='{0}' style='color:#ffffff;text-decoration:underline;'>{0}</a>", url21));
                                %>
                                    <br />
                                </p>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
