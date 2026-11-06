<%@ control language="C#" autoeventwireup="true" codefile="Menu.ascx.cs" inherits="Menu" %>
<ul class="nav navbar-nav">
	<!-- Summary -->
	<li id="a0" runat="server">
		<a runat="server" href="~/Page/Summary.aspx" class="visible-sm visible-md visible-lg" title="Summary" style="margin-left: -15px;">
			<i class="icon-info icon-large"></i>
		</a>
		<a runat="server" href="~/Page/Summary.aspx" class="visible-xs">Summary</a>
	</li>

	<!-- Module Menu -->
	<li id="a1" runat="server"><a runat="server" href="~/Page/Create.aspx">Create New User</a></li>
	<li id="a2" runat="server"><a runat="server" href="~/Page/User.aspx">User Account</a></li>
	<li id="a3" runat="server"><a runat="server" href="~/Page/Role.aspx">Role Settings</a></li>
	<li id="a4" runat="server"><a runat="server" href="~/Page/AccessLog.aspx">Access Log</a></li>
	<li id="a5" runat="server"><a runat="server" href="~/Page/WhitelistTradeIn/Approver/index.aspx">Whitelist Trade In</a></li>
	<li id="a6" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Whitelist TradeIn Old
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="AX1" runat="server" href="~/Page/WhitelistTradeInOld/Register/WhitelistTradeIn.aspx">Register</a></li>
			<li><a id="AX2" runat="server" href="~/Page/WhitelistTradeInOld/Approval/WhitelistTradeIn.aspx">Approval</a></li>
		</ul>
	</li>
</ul>
