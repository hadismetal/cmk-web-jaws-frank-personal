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
            <li><a runat="server" href="~/Page/Register/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Register/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Register/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Register/LD.aspx">Loose Diamond</a></li>
		</ul>
	</li>
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a runat="server" href="~/Page/Approval/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Approval/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Approval/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Approval/LD.aspx">Loose Diamond</a></li>
		</ul>
	</li>
    <li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Report/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Report/Rekap/DJ.aspx">Diamond Jewellery Rekap</a></li>
            <li><a runat="server" href="~/Page/Report/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Report/Rekap/PG.aspx">Plain Gold Rekap</a></li>
            <li><a runat="server" href="~/Page/Report/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Report/Rekap/GJ.aspx">Gold Jewellery Rekap</a></li>
            <li><a runat="server" href="~/Page/Report/LD.aspx">Loose Diamond</a></li>
            <li><a runat="server" href="~/Page/Report/LD.aspx">Loose Diamond Rekap</a></li>
		</ul>
	</li>
</ul>

