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
    <%--<li id="li2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Register
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A3" runat="server" href="~/Page/Register/WhitelistTradeIn.aspx">Whitelist TradeIn</a></li>
            
		</ul>
	</li>
    <li id="li1" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A1" runat="server" href="~/Page/Approval/WhitelistTradeIn.aspx">Whitelist TradeIn</a></li>
		</ul>
	</li>--%>
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a runat="server" href="~/Page/Report/Sales.aspx">Sales</a></li>
            <li><a runat="server" href="~/Page/Report/SalesRekap.aspx">Sales Rekap</a></li>
            <li><a id="A42" runat="server" href="~/Page/Report/SalesRekapSNI.aspx">Sales Detail SNI</a></li>
            
		</ul>
	</li>
</ul>
