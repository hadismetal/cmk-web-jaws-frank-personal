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
        <ul class="dropdown-menu multi-level">
            <li><a runat="server" href="~/Page/Register/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Register/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Register/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Register/LD.aspx">Loose Diamond</a></li>
            <%--<li id="Li4" runat="server" class="dropdown-submenu">
                <a href="#">Bulk
                    <ul class="dropdown-menu">
                        <li><a id="A11" runat="server" href="~/Page/Register/Bulk/DJ.aspx">Diamond Jewellery</a></li>
                        <li><a id="A12" runat="server" href="~/Page/Register/Bulk/PG.aspx">Plain Gold</a></li>
                        <li><a id="A13" runat="server" href="~/Page/Register/Bulk/GJ.aspx">Gold Jewellery</a></li>
                        <li><a id="A14" runat="server" href="~/Page/Register/Bulk/LD.aspx">Loose Diamond</a></li>
                    </ul>
                </a>
            </li>--%>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Cross Brand
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Register/CrossBrand/DJ.aspx">Diamond Jawelry</a></li>
                        <li><a runat="server" href="~/Page/Register/CrossBrand/PG.aspx">Plain Gold</a></li>
                        <li><a runat="server" href="~/Page/Register/CrossBrand/LD.aspx">Loose Diamond</a></li>
                    </ul>
                </a>
            </li>
            <li id="Li1" runat="server" class="dropdown-submenu">
                <a href="#">Packaging
                    <ul class="dropdown-menu">
                        <li><a id="A3" runat="server" href="~/Page/Register/Packaging/Product.aspx">Reguler</a></li>
                        <li><a id="A6" runat="server" href="~/Page/Register/Packaging/ProductBroken.aspx">Broken</a></li>
                    </ul>
                </a>
            </li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Souvenir
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Register/Souvenir/Product.aspx">Reguler</a></li>
                        <li><a runat="server" href="~/Page/Register/Souvenir/ProductBroken.aspx">Broken</a></li>
                    </ul>
                </a>
            </li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Stock Retirement
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Register/StockRetire/DJ.aspx">Diamond Jewellery</a></li>
                        <li><a runat="server" href="~/Page/Register/StockRetire/PG.aspx">Plain Gold</a></li>
                        <li><a runat="server" href="~/Page/Register/StockRetire/GJ.aspx">Gold Jewellery</a></li>
                    </ul>
                </a>
            </li>
			<li><a runat="server" href="~/Page/Register/DOKRM">DO KRM</a></li>
        </ul>
    </li>
    <li id="a5" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Update
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a runat="server" href="~/Page/Update/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Update/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Update/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Update/LD.aspx">Loose Diamond</a></li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Cross Brand</a>
                <ul class="dropdown-menu">
                    <li><a runat="server" href="~/Page/Update/CrossBrand/DJ.aspx">Diamond Jewellery</a></li>
                </ul>                
            </li>
            <li><a runat="server" id="DOKERAM" href="~/Page/Update/DOKRM">DO KRM</a></li>
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
            <li runat="server" class="dropdown-submenu">
                <a href="#">Cross Brand</a>
                <ul class="dropdown-menu">
                    <li><a runat="server" href="~/Page/Approval/DJCrossBrand.aspx">Diamond Jewellery</a></li>
                    <li><a runat="server" href="~/Page/Approval/PGCrossBrand.aspx">Plain Gold</a></li>
                    <li><a runat="server" href="~/Page/Approval/LDCrossBrand.aspx">Loose Diamond</a></li>
                </ul>
            </li>
            <li id="Li2" runat="server" class="dropdown-submenu" >
                <a href="#">Packaging
                    <ul class="dropdown-menu">
                        <li><a id="A7" runat="server" href="~/Page/Approval/Packaging/Product.aspx">Reguler</a></li>
                        <li><a id="A8" runat="server" href="~/Page/Approval/Packaging/ProductBroken.aspx">Broken</a></li>
                    </ul>
                </a>
            </li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Souvenir
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Approval/Souvenir/Product.aspx">Reguler</a></li>
                        <li><a runat="server" href="~/Page/Approval/Souvenir/ProductBroken.aspx">Broken</a></li>
                    </ul>
                </a>
            </li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Stock Retirement
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Approval/StockRetire/DJ.aspx">Diamond Jewellery</a></li>
                        <li><a runat="server" href="~/Page/Approval/StockRetire/PG.aspx">Plain Gold</a></li>
                        <li><a runat="server" href="~/Page/Approval/StockRetire/GJ.aspx">Gold Jewellery</a></li>
                    </ul>
                </a>
            </li>
			<li><a runat="server" id="ANJ4Y" href="~/Page/Approval/DOKRM">DO KRM</a></li>
        </ul>
    </li>
    <%--<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">History
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/History/DJ.aspx">Diamond Jewelry</a></li>
			<li><a runat="server" href="~/Page/History/PG.aspx">Plain Gold</a></li>
		</ul>
	</li>--%>
    <li id="a4" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a runat="server" href="~/Page/Report/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Report/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Report/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Report/LD.aspx">Loose Diamond</a></li>
            </li><li id="Li99" runat="server" class="dropdown-submenu">
                <a href="#">Cross Brand
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Report/DJ_CrossBrand.aspx">Diamond Jewellery</a></li>
                        <li><a runat="server" href="~/Page/Report/PG_CrossBrand.aspx">Plain Gold</a></li>
                        <li><a runat="server" href="~/Page/Report/LD_CrossBrand.aspx">Loose Diamond</a></li>
                    </ul>
                </a>
            </li>
            <li id="Li3" runat="server" class="dropdown-submenu">
                <a href="#">Packaging
                    <ul class="dropdown-menu">
                        <li><a id="A9" runat="server" href="~/Page/Report/Packaging/Product.aspx">Reguler</a></li>
                        <li><a id="A11" runat="server" href="~/Page/Report/Packaging/Product_Outgoing.aspx">Detail</a></li>
                        <li><a id="A10" runat="server" href="~/Page/Report/Packaging/ProductBroken.aspx">Broken</a></li>
                    </ul>
                </a>
            </li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Souvenir
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Report/Souvenir/Product.aspx">Reguler</a></li>
                        <li><a runat="server" href="~/Page/Report/Souvenir/Product_Outgoing.aspx">Detail</a></li>
                        <li><a runat="server" href="~/Page/Report/Souvenir/ProductBroken.aspx">Broken</a></li>
                    </ul>
                </a>
            </li>
			<li><a runat="server" href="~/Page/Report/DOKRM">DO KRM</a></li>
        </ul>
    </li>
</ul>
