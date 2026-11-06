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
    <li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Register
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">			
			<li><a runat="server" href="~/Page/Register/DJ/DJ.aspx">Promo Diamond Jewellery</a></li>
			<li><a runat="server" href="~/Page/Register/DJ/DJ_Bulk.aspx">Promo Diamond Jewellery (Upload Bulk One Price)</a></li>
            <li><a id="A14" runat="server" href="~/Page/Register/PG/PG.aspx">Promo Plain Gold</a></li>
            <li><a id="A13" runat="server" href="~/Page/Register/GJ/GJ.aspx">Promo Gold Jewellery</a></li>
            <li><a id="A21" runat="server" href="~/Page/Register/Bundling/Bundling.aspx">Promo Bundling</a></li>
            <li><a id="A24" runat="server" href="~/Page/Register/MasterPoint/MasterPoint.aspx">Master Point</a></li>    
            <li><a id="A27" runat="server" href="~/Page/Register/Souvenir/Souvenir.aspx">Souvenir</a></li>    
            <li><a id="A31" runat="server" href="~/Page/Register/MDR/MDRRate.aspx">MDR Rate</a></li>    
            <li><a id="A33" runat="server" href="~/Page/Register/Voucher/Voucher.aspx">Voucher</a></li> 
			<li><a id="B1" runat="server" href="~/Page/Register/LainLain/Promo.aspx">Promo Lain Lain</a></li>
		</ul>
	</li>
	<li id="a1" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Status
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li class="dropdown-header">ASP</li>
			<li><a runat="server" href="~/Page/Status/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Status/PG.aspx">Plain Gold</a></li>
            <li><a id="A9" runat="server" href="~/Page/Status/GJ.aspx">Gold Jewellery</a></li>
            <li><a id="A10" runat="server" href="~/Page/Status/LD.aspx">Loose Diamond</a></li>
            <li class="dropdown-header">Resell Mark Up</li>
			<li><a runat="server" href="~/Page/Status/MU.aspx">Mark Up</a></li>
            <li class="dropdown-header">Point Reward</li>
			<li><a id="A5" runat="server" href="~/Page/Status/PointRewardDJ.aspx">Point Reward Diamond Jewellery</a></li>
            <li><a id="A8" runat="server" href="~/Page/Status/PointRewardGJ.aspx">Point Reward Gold Jewellery</a></li>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Update
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li class="dropdown-header">ASP</li>
			<li><a runat="server" href="~/Page/Update/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Update/PG.aspx">Plain Gold</a></li>
            <li><a id="A11" runat="server" href="~/Page/Update/GJ.aspx">Gold Jewellery</a></li>
            <li><a id="A12" runat="server" href="~/Page/Update/LD.aspx">Loose Diamond</a></li>
            <li class="dropdown-header">Resell Mark Up</li>
			<li><a runat="server" href="~/Page/Update/MU.aspx">Diamond Jewellery Mark Up</a></li>
            <li><a id="A17" runat="server" href="~/Page/Update/MULD.aspx">Loose Diamond Mark Up</a></li>
            <li class="dropdown-header">Point Reward</li>
			<li><a id="A6" runat="server" href="~/Page/Update/PointRewardDJ.aspx">Point Reward Diamond Jewellery</a></li>
            <li><a id="A7" runat="server" href="~/Page/Update/PointRewardGJ.aspx">Point Reward Gold Jewellery</a></li>
            <li><a id="A23" runat="server" href="~/Page/Update/Bundling/Home.aspx">Promo Bundling</a></li>
            <li><a id="A26" runat="server" href="~/Page/Update/MasterPoint/Home.aspx">Master Point</a></li>
            <li><a id="A29" runat="server" href="~/Page/Update/Souvenir/Home.aspx">Souvenir</a></li>
            <li><a id="B4" runat="server" href="~/Page/Update/LainLain/Promo.aspx">Promo Lain Lain</a></li>
		</ul>
	</li>
    <li id="a4" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">			
			<li><a runat="server" href="~/Page/Approval/DJ/DJ.aspx">Promo Diamond Jewellery</a></li>
            <li><a id="A16" runat="server" href="~/Page/Approval/PG/PG.aspx">Promo Plain Gold</a></li>
            <li><a id="A15" runat="server" href="~/Page/Approval/GJ/GJ.aspx">Promo Gold Jewellery</a></li>
            <li><a id="A22" runat="server" href="~/Page/Approval/Bundling/Bundling.aspx">Promo Bundling</a></li>          
            <li><a id="A25" runat="server" href="~/Page/Approval/MasterPoint/MasterPoint.aspx">Master Point</a></li>  
            <li><a id="A28" runat="server" href="~/Page/Approval/Souvenir/Souvenir.aspx">Souvenir</a></li>
			<li><a id="B2" runat="server" href="~/Page/Approval/LainLain/Promo.aspx">Promo Lain Lain</a></li>
		</ul>
	</li>
    <li id="Li1" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			
			<li><a id="A18" runat="server" href="~/Page/Report/DJ/DJ.aspx">Promo Diamond Jewellery</a></li>
            <li><a id="A35" runat="server" href="~/Page/Report/DJ/DJ_List.aspx">Promo Diamond Jewellery List</a></li>
            <li><a id="A20" runat="server" href="~/Page/Report/PG/PG.aspx">Promo Plain Gold</a></li>
            <li><a id="A37" runat="server" href="~/Page/Report/PG/PG_List.aspx">Promo Plain Gold List</a></li>
            <li><a id="A19" runat="server" href="~/Page/Report/GJ/GJ.aspx">Promo Gold Jewellery</a></li>
            <li><a id="A36" runat="server" href="~/Page/Report/GJ/GJ_List.aspx">Promo Gold Jewellery List</a></li>
            <li><a id="A30" runat="server" href="~/Page/Report/Souvenir/Souvenir.aspx">Souvenir</a></li>
            <li><a id="A32" runat="server" href="~/Page/Report/MDR/MDR.aspx">MDR</a></li>
            <li><a id="A34" runat="server" href="~/Page/Report/Voucher/Voucher.aspx">Voucher</a></li>
			<li><a id="B3" runat="server" href="~/Page/Report/LainLain/PromoReport.aspx">Promo Lain Lain</a></li>
		</ul>
	</li>
</ul>
