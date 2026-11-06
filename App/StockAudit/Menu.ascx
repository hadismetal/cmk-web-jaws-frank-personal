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
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Register
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A3" runat="server" href="~/Page/Register/Bulk_LocationType.aspx">All</a></li>
            <li><a runat="server" href="~/Page/Register/DJ_LocationType.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Register/PG_LocationType.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Register/GJ_LocationType.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Register/LD_LocationType.aspx">Loose Diamond</a></li>
		</ul>
	</li>
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Report/StockAuditDJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Report/StockAuditPG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Report/StockAuditGJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Report/StockAuditLD.aspx">Loose Diamond</a></li>
		</ul>
	</li>
</ul>

