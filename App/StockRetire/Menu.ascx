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
			<li><a runat="server" href="~/Page/Create/DJ/DJ.aspx">Diamond Jewellery Retirement</a></li>
			<li><a runat="server" href="~/Page/Create/DJ/DJ_WorkOrder.aspx">DO Diamond Jewellery Retirement</a></li>
           <%-- <li><a runat="server" href="~/Page/Create/PG/PG.aspx">Plain Gold Retirement</a></li>
            <li><a runat="server" href="~/Page/Create/GJ/GJ.aspx">Gold Jewellery Retirement</a></li>--%>
		</ul>
	</li>
    <li id="a4" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Response
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Status/DJ.aspx">Diamond Jewellery Retirement</a></li>
           <%-- <li><a runat="server" href="~/Page/Status/PG.aspx">Plain Gold Retirement</a></li>
            <li><a runat="server" href="~/Page/Status/GJ.aspx">Gold Jewellery Retirement</a></li>--%>
		</ul>
	</li>
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Approve/DJ.aspx">Diamond Jewellery</a></li>
           <%-- <li><a runat="server" href="~/Page/Approve/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Approve/GJ.aspx">Gold Jewellery</a></li>--%>
		</ul>
	</li>
    <li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Report/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Report/DJ_Detail.aspx">Diamond Jewellery Detail</a></li>
            <li><a runat="server" href="~/Page/Report/DJ_RekapDetail.aspx">Diamond Jewellery Rekap Lebur</a></li>
			<li><a runat="server" href="~/Page/Report/DJ_WorkOrder.aspx">DO Diamond Jewellery</a></li>
            <%--<li><a runat="server" href="~/Page/Report/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Report/GJ.aspx">Gold Jewellery</a></li>--%>
		</ul>
	</li>
</ul>
