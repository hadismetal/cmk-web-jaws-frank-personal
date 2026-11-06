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
	<li id="a1" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Create New
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Create/Designer.aspx">Designer</a></li>
            <li><a id="A4" runat="server" href="~/Page/Create/StockKeeper_LocationType.aspx">Stock Keeper</a></li>
            <li><a id="A7" runat="server" href="~/Page/Create/Sales.aspx">Sales</a></li>
            <li><a id="A10" runat="server" href="~/Page/Create/AdminSales_LocationType.aspx">Admin Sales</a></li>
            <li><a id="A12" runat="server" href="~/Page/Create/AM.aspx">Area Manager</a></li>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Account
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Account/Designer.aspx">Designer</a></li>
            <li><a id="A5" runat="server" href="~/Page/Account/StockKeeper.aspx">Stock Keeper</a></li>
            <li><a id="A8" runat="server" href="~/Page/Account/Sales.aspx">Sales</a></li>
            <li><a id="A11" runat="server" href="~/Page/Account/AdminSales.aspx">Admin Sales</a></li>
            <li><a id="A13" runat="server" href="~/Page/Account/AM.aspx">Area Manager</a></li>
		</ul>
	</li>
	<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Group
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Group/Designer.aspx">Designer</a></li>
            <li><a id="A6" runat="server" href="~/Page/Group/StockKeeper.aspx">StockKeeper</a></li>
            <li><a id="A9" runat="server" href="~/Page/Group/Sales.aspx">Sales</a></li>
            
		</ul>
	</li>
</ul>
