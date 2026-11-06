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
			<li><a runat="server" href="~/Page/Create/1A.aspx">Diamond Reguler</a></li>
			<li><a runat="server" href="~/Page/Create/1B.aspx">Diamond Rapaport</a></li>
			<li><a runat="server" href="~/Page/Create/2.aspx">Colour Diamond</a></li>
			<li><a runat="server" href="~/Page/Create/3.aspx">Precious Stone</a></li>
			<li><a runat="server" href="~/Page/Create/4.aspx">Semi Precious</a></li>
			<li><a runat="server" href="~/Page/Create/5.aspx">Pearl</a></li>
            <li><a runat="server" href="~/Page/Create/Repair.aspx">Diamond Repair</a></li>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Account
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Account/1A.aspx">Diamond Reguler</a></li>
			<li><a runat="server" href="~/Page/Account/1B.aspx">Diamond Rapaport</a></li>
			<li><a runat="server" href="~/Page/Account/2.aspx">Colour Diamond</a></li>
			<li><a runat="server" href="~/Page/Account/3.aspx">Precious Stone</a></li>
			<li><a runat="server" href="~/Page/Account/4.aspx">Semi Precious</a></li>
			<li><a runat="server" href="~/Page/Account/5.aspx">Pearl</a></li>
            <li><a runat="server" href="~/Page/Account/Repair.aspx">Diamond Repair</a></li>
		</ul>
	</li>
</ul>
