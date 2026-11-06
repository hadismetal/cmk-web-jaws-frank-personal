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
			<li class="dropdown-header">Exchange</li>
			<li><a runat="server" href="~/Page/Status/TG.aspx">Gold Rate</a></li>
			<li><a runat="server" href="~/Page/Status/TGP.aspx">Today Gold Price (Stock Receive)</a></li>
            <li><a runat="server" href="~/Page/Status/TGP_V2.aspx">Today Gold Price (Sales Order)</a></li>
			<li><a runat="server" href="~/Page/Status/US.aspx">USD Rate</a></li>
			<li class="divider"></li>
			<li class="dropdown-header">Service Fee</li>
			<li><a runat="server" href="~/Page/Status/FM.aspx">Mounting Fee</a></li>
			<li><a runat="server" href="~/Page/Status/FS.aspx">Setting Fee</a></li>
			<li><a runat="server" href="~/Page/Status/FF.aspx">Finishing Fee</a></li>
            <li><a id="A4" runat="server" href="~/Page/Status/FR.aspx">Repair Fee</a></li>
			<li><a id="ASW" runat="server" href="~/Page/Status/RZ.aspx">Extra Ring Size Fee</a></li>
			<li class="divider"></li>
			<li class="dropdown-header">System</li>
			<li><a runat="server" href="~/Page/Status/MU.aspx">Mark-Up System</a></li>
			<li><a runat="server" href="~/Page/Status/SS.aspx">Segmen System</a></li>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Update
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li class="dropdown-header">Exchange</li>
			<li><a runat="server" href="~/Page/Update/TG.aspx">Gold Rate</a></li>
			<li><a runat="server" href="~/Page/Update/TGP.aspx">Today Gold Price (Stock Receive)</a></li>
            <li><a runat="server" href="~/Page/Update/US.aspx">USD Rate</a></li>
			<li class="divider"></li>
			<li class="dropdown-header">Service Fee</li>
			<li><a runat="server" href="~/Page/Update/FM.aspx">Mounting Fee</a></li>
			<li><a runat="server" href="~/Page/Update/FS.aspx">Setting Fee</a></li>
			<li><a runat="server" href="~/Page/Update/FF.aspx">Finishing Fee</a></li>
            <li><a id="A5" runat="server" href="~/Page/Update/FR.aspx">Repair Fee</a></li>
			<li class="divider"></li>
			<li class="dropdown-header">System</li>
			<li><a runat="server" href="~/Page/Update/MU.aspx">Mark-Up System</a></li>
			<li><a runat="server" href="~/Page/Update/SS.aspx">Segmen System</a></li>
		</ul>
	</li>
	<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li class="dropdown-header">Exchange</li>
			<li><a runat="server" href="~/Page/Approval/TG.aspx">Gold Rate</a></li>
			<li><a runat="server" href="~/Page/Approval/TGP.aspx">Today Gold Price (Stock Receive)</a></li>
            <li><a runat="server" href="~/Page/Approval/US.aspx">USD Rate</a></li>
			<li class="divider"></li>
			<li class="dropdown-header">Service Fee</li>
			<li><a runat="server" href="~/Page/Approval/FM.aspx">Mounting Fee</a></li>
			<li><a runat="server" href="~/Page/Approval/FS.aspx">Setting Fee</a></li>
			<li><a runat="server" href="~/Page/Approval/FF.aspx">Finishing Fee</a></li>
            <li><a id="A6" runat="server" href="~/Page/Approval/FR.aspx">Repair Fee</a></li>
			<li class="divider"></li>
			<li class="dropdown-header">System</li>
			<li><a runat="server" href="~/Page/Approval/MU.aspx">Mark-Up System</a></li>
			<li><a runat="server" href="~/Page/Approval/SS.aspx">Segmen System</a></li>
		</ul>
	</li>
</ul>
