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
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Create
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A7" runat="server" href="~/Page/Create/Sales.aspx">Sales</a></li>
            <li><a id="A10" runat="server" href="~/Page/Create/AdminSales_LocationType.aspx">Admin Sales</a></li>
			<li><a id="A5" target="_blank" runat="server" href="~/Page/Create/Create_Certificate.aspx">Certificate</a></li>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Account
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A8" runat="server" href="~/Page/Account/Sales.aspx">Sales</a></li>
            <li><a id="A11" runat="server" href="~/Page/Account/AdminSales.aspx">Admin Sales</a></li>
		</ul>
	</li>
    <li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A4" runat="server" href="~/Page/Report/Report_Certificate.aspx">Print Certificate</a></li>
		</ul>
	</li>
    <li id="Li1" runat="server" class="dropdown">
		<a class="dropdown-toggle" data-toggle="dropdown" href="#<%--/WS/Page/CreateNew/CouponPrizedraw.aspx--%>" >Coupon
            <b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="#"></a></li>
			<li><a runat="server" href="~/Page/CouponPrizedraw/Page/Update/CouponPrizedrawUpdate.aspx" >Create New / Update</a></li>
			<li><a runat="server" href="~/Page/CouponPrizedraw/Page/Approve/CouponPrizedrawApprove.aspx">Approve</a></li>
			<li><a runat="server" href="~/Page/CouponPrizedraw/Page/Report/CouponPrizedrawReport.aspx">Report</a></li>
		</ul>
	</li>
</ul>
