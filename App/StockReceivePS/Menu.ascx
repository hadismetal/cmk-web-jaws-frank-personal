<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Menu" %>
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

            <li id="Li1" runat="server" class="dropdown-submenu">
                <a href="#">Packaging
                    <ul class="dropdown-menu">
                        <li><a id="A6" runat="server" href="~/Page/Register/Packaging/Product_LocationType.aspx">Receiving</a></li>
                        <li><a id="A7" runat="server" href="~/Page/Register/Packaging/ProductAdjustment_LocationType.aspx">Stock Adjustment</a></li>
                    </ul>
                </a>
            </li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Souvenir
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Register/Souvenir/Product_LocationType.aspx">Receiving</a></li>
                        <li><a runat="server" href="~/Page/Register/Souvenir/ProductAdjustment_LocationType.aspx">Stock Adjustment</a></li>
                    </ul>
                </a>
            </li>
        </ul>
    </li>
    <li id="a3" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">

            <li id="Li2" runat="server" class="dropdown-submenu">
                <a href="#">Packaging
                    <ul class="dropdown-menu">
                        <li><a id="A8" runat="server" href="~/Page/Approval/Packaging/Product.aspx">Receiving</a></li>
                        <li><a id="A9" runat="server" href="~/Page/Approval/Packaging/ProductAdjustment.aspx">Stock Adjustment</a></li>
                    </ul>
                </a>
            </li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Souvenir
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Approval/Souvenir/Product.aspx">Receiving</a></li>
                        <li><a runat="server" href="~/Page/Approval/Souvenir/ProductAdjustment.aspx">Stock Adjustment</a></li>
                    </ul>
                </a>
            </li>
        </ul>
    </li>
    <%-->li id="a4" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">History
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/History/DJ.aspx">Diamond Jewelry</a></li>
			<li><a runat="server" href="~/Page/History/PG.aspx">Plain Gold</a></li>
		</ul>
	</--%>
    <li id="a5" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li id="Li3" runat="server" class="dropdown-submenu">
                <a href="#">Packaging
                    <ul class="dropdown-menu">
                        <li><a id="A10" runat="server" href="~/Page/Report/Packaging/Product_Rekap_LocationType.aspx">Receiving</a></li>
                        <li><a id="A11" runat="server" href="~/Page/Report/Packaging/ProductAdjustment.aspx">Packaging Adjustment</a></li>
                        <li><a runat="server" href="~/Page/Report/InvPackaging/Product_Rekap_LocationType.aspx">Inventory Packaging</a></li>
                        <li><a runat="server" href="~/Page/Report/Packaging.aspx">Pemakaian Packaging</a></li>
                        <li><a runat="server" href="~/Page/Report/Packaging/Product_Summary.aspx">Summary</a></li>
                    </ul>
                </a>
            </li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Souvenir
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Report/Souvenir/Product_Rekap_LocationType.aspx">Receiving</a></li>
                        <li><a runat="server" href="~/Page/Report/Souvenir/ProductAdjustment.aspx">Souvenir Adjustment</a></li>
                        <li><a runat="server" href="~/Page/Report/InvSouvenir/Product_Rekap_LocationType.aspx">Inventory Souvenir</a></li>
                        <li><a runat="server" href="~/Page/Report/Souvenir/Souvenir.aspx">Pemakaian Souvenir</a></li>
                        <li><a runat="server" href="~/Page/Report/Souvenir/Product_Summary.aspx">Summary</a></li>
                    </ul>
                </a>
            </li>
        </ul>
    </li>
</ul>
