<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print_Certificate_DJ.aspx.cs" Inherits="Page_Report_Print_Certificate_DJ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Print Certificate</title>
    <asp:Literal ID="css" runat="server">

    </asp:Literal>
</head>

<body>
	<div style="margin-left:66.3px;margin-top:80px">
		<table style="float:left" id="tsertifimg" runat="server">
			<tbody>
				<tr>
					<td style="align:center">
						<asp:Literal ID="img" runat="server" />
					</td>
				</tr>
			</tbody>
		</table>
		<table style="" id="tsertif" runat="server">
			<tbody>
				<tr>
					<td valign="top">
						<table style="margin-left:10px">
							<tbody>
								<tr>
									<td style="font-size:12px;font-family:'Baskerville Old Face';font-weight:600">
										<asp:Literal ID="plu" runat="server" />
									</td>
								</tr>
								<tr>
									<td style="font-size:8px;font-family:'Avenir Black'">
										<asp:Literal ID="kadar" runat="server" />%
										<asp:Literal ID="weight" runat="server" />gr
									</td>
								</tr>
								<tr rowspan="2">
								</tr>
								<tr>
									<td align="left" valign="top" style="font-family:'Baskerville Old Face';font-size: 7.3px;">
										<asp:Literal ID="txtstone" runat="server" />
									</td>
								</tr>
								<tr style="margin-top: 0px">
									<td align="left" valign="top" style=";font-size:12px;">
										<i><asp:Literal ID="item" runat="server" /></i>
									</td>
								</tr>
								<tr>
									<td align="left" valign="top" style="font-family:'Avenir Book'; font-size: 7.3px; line-height: 70%">
										<asp:Literal ID="kodekaret" runat="server" />
									</td>
								</tr>
							</tbody>

						</table>
					</td>
				</tr>
			</tbody>
		</table>
	</div>
    <label id="lbwarning" runat="server"></label>
</body>
    <script>
        window.print();
    </script>
</html>
