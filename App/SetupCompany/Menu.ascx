<%@ control language="C#" autoeventwireup="true" inherits="Menu, App_Web_orddg5pn" %>
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
			<li><a runat="server" href="~/Page/Create/Company.aspx">Company</a></li>
			<li><a runat="server" href="~/Page/Create/Brand.aspx">Market Brand</a></li>
			<li><a runat="server" href="~/Page/Create/Div.aspx">Business Division</a></li>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Account
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Account/Company.aspx">Company</a></li>
			<li><a runat="server" href="~/Page/Account/Brand.aspx">Market Brand</a></li>
			<li><a runat="server" href="~/Page/Account/Div.aspx">Business Division</a></li>
		</ul>
	</li>
</ul>
