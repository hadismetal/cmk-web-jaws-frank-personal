<%@ page language="C#" masterpagefile="~/MasterError.master" autoeventwireup="true" codefile="CustomErrors.aspx.cs" inherits="CustomErrors" %>

<asp:content contentplaceholderid="icon" runat="server">
	<img src="../Images/png/dead-robot.png" style="height: 30%"> </img>
</asp:content>

<asp:content contentplaceholderid="h1" runat="server">
	<p style="color: white; font-family: Trebuchet MS; margin:10px 0 30px 0; font-size:62px">Don't Worry...</p>
</asp:content>

<asp:content contentplaceholderid="message" runat="server">
	<p style="color: white; font-family: Trebuchet MS; font-size:20px; margin: 0;">
		Currently, you may facing network glitch. Please wait max 3 minutes then please retry or re-login.
	</p>
	<p style="color: white; font-family: Trebuchet MS;  font-size:20px">
		If the issue still there, please contact IT support, we are love to help.
	</p>
	<a href="javascript:closeBack();" style="margin-top: 30px; font-size: 20px; color: #345B63;" >
		<i class="icon-arrow-left icon-large"></i>
		Go back
	</a>
</asp:content>

