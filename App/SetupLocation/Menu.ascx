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
			<li><a runat="server" href="~/Page/Create/Warehouse.aspx">Warehouse</a></li>
			<li><a runat="server" href="~/Page/Create/Outlet.aspx">Outlet</a></li>
			<li><a runat="server" href="~/Page/Create/Exhibition.aspx">Exhibition</a></li>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Account
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Account/Warehouse.aspx">Warehouse</a></li>
			<li><a runat="server" href="~/Page/Account/Outlet.aspx">Outlet</a></li>
			<li><a runat="server" href="~/Page/Account/Exhibition.aspx">Exhibition</a></li>
		</ul>
	</li>
	<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Group
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Group/Warehouse.aspx">Warehouse</a></li>
			<li><a runat="server" href="~/Page/Group/Outlet.aspx">Outlet</a></li>
			<li><a runat="server" href="~/Page/Group/Exhibition.aspx">Exhibition</a></li>
		</ul>
	</li>
</ul>
