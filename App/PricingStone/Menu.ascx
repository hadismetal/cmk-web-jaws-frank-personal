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
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Status
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Status/1A.aspx">Diamond Reguler</a></li>
			<li><a runat="server" href="~/Page/Status/1B.aspx">Diamond Dossier</a></li>
			<li><a runat="server" href="~/Page/Status/2.aspx">Colour Diamond</a></li>
			<li><a runat="server" href="~/Page/Status/3.aspx">Precious Stone</a></li>
			<li><a runat="server" href="~/Page/Status/4.aspx">Semi Precious</a></li>
			<li><a runat="server" href="~/Page/Status/5.aspx">Pearl</a></li>
            <li><a runat="server" href="~/Page/Status/Repair.aspx">Diamond Repair</a></li>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Update
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Update/1A.aspx">Diamond Reguler</a></li>
			<li><a runat="server" href="~/Page/Update/1B.aspx">Diamond Dossier</a></li>
			<li><a runat="server" href="~/Page/Update/2.aspx">Colour Diamond</a></li>
			<li><a runat="server" href="~/Page/Update/3.aspx">Precious Stone</a></li>
			<li><a runat="server" href="~/Page/Update/4.aspx">Semi Precious</a></li>
			<li><a runat="server" href="~/Page/Update/5.aspx">Pearl</a></li>
            <li><a runat="server" href="~/Page/Update/Repair.aspx">Diamond Repair</a></li>
		</ul>
	</li>
	<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Approval/1A.aspx">Diamond Reguler</a></li>
			<li><a runat="server" href="~/Page/Approval/1B.aspx">Diamond Dossier</a></li>
			<li><a runat="server" href="~/Page/Approval/2.aspx">Colour Diamond</a></li>
			<li><a runat="server" href="~/Page/Approval/3.aspx">Precious Stone</a></li>
			<li><a runat="server" href="~/Page/Approval/4.aspx">Semi Precious</a></li>
			<li><a runat="server" href="~/Page/Approval/5.aspx">Pearl</a></li>
            <li><a runat="server" href="~/Page/Approval/Repair.aspx">Diamond Repair</a></li>
		</ul>
	</li>

    <li id="a4" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Sync
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Sync/1A.aspx">Diamond Reguler</a></li>	
            <li><a runat="server" href="~/Page/Sync/1B.aspx">Diamond Dossier</a></li>
            <li><a runat="server" href="~/Page/Sync/2.aspx">Colour Diamond</a></li>
            <li><a runat="server" href="~/Page/Sync/3.aspx">Precious Stone</a></li>
            <li><a runat="server" href="~/Page/Sync/4.aspx">Semi Precious</a></li>
            <li><a runat="server" href="~/Page/Sync/5.aspx">Pearl</a></li>
		</ul>
	</li>
</ul>
