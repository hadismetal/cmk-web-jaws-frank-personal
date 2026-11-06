<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print_Certificate_PG.aspx.cs" Inherits="Page_Report_Print_Certificate_PG" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print  PG Certificate</title>
      <asp:Literal ID="css" runat="server">

    </asp:Literal>
</head>
<body>
   <table style="margin: 0px">
        <tbody>
            <tr>
                <td valign="center">
                    <asp:Literal ID="img" runat="server" />
                </td>
                <td valign="top">
                    <table>
                        <tbody>
                            <tr>
                                <td colspan="5" style="font-size: 16px; font-weight: bold">
                                    <asp:Literal ID="plu" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="5" style="font-size: 12px;">
                                    <asp:Literal ID="kadar" runat="server" />%
                                    <asp:Literal ID="weight" runat="server" />gr</td>
                            </tr>
                            <tr>
                                <td align="center" valign="top" style="font-family: Arial, Helvetica, sans-serif; font-size: 8px; line-height: 12px">
                                    <asp:Literal ID="kodekaret" runat="server" /></td>

                                </td>
                            </tr>
                        </tbody>

                    </table>
                </td>
            </tr>
            <tr style="margin-top: 0px">
                <td colspan="2" align="center" valign="top" style="font-family: 'Monotype Corsiva'; font-size: 22px; line-height: 70%">
                    <asp:Literal ID="item" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" valign="top" style="font-family: Arial, Helvetica, sans-serif; font-size: 8px; line-height: 70%">
                    
                    <asp:Literal ID="txcolor" runat="server" />
            </tr>

        </tbody>
    </table>

</body>
     <script>
        window.print();
    </script>
</html>
