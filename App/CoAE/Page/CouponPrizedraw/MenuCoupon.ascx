<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuCoupon.ascx.cs" Inherits="WS_MenuCoupon" %>
<ul class="nav navbar-nav">
	<!-- Summary -->
	<li id="a0" runat="server">
		<a runat="server" href="/WS/Summary.aspx" class="visible-sm visible-md visible-lg" title="Summary" style="margin-left: -15px;">
			<i class="icon-info icon-large"></i>
		</a>
		<a runat="server" href="/WS/Summary.aspx" class="visible-xs">Summary</a>
	</li>

	<!-- Module Menu -->
	<li id="a1" runat="server" class="dropdown">
		<a class="dropdown-toggle" data-toggle="dropdown" href="#<%--/WS/Page/CreateNew/CouponPrizedraw.aspx--%>" >Coupon
            <b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="#"></a></li>
			<li><a runat="server" href="/WS/Page/Update/CouponPrizedrawUpdate.aspx" >Create New / Update</a></li>
			<li><a runat="server" href="/WS/Page/Approve/CouponPrizedrawApprove.aspx">Approve</a></li>
			<li><a runat="server" href="/WS/Page/Report/CouponPrizedrawReport.aspx">Report</a></li>
		</ul>
	</li>
    <%--<li id="a2" runat="server">
		<a href="/WS/Page/Update/CouponPrizedrawUpdate.aspx" >Update
		</a>
	</li>
    <li id="a3" runat="server">
        <a href="/WS/Page/Approve/CouponPrizedrawApprove.aspx">Approve
        </a>
    </li>
	<li id="a4" runat="server">
		<a href="/WS/Page/Report/CouponPrizedrawReport.aspx" >Report
		</a>
	</li>--%>
</ul>
