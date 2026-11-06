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
    <li id="Li6" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Register
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            
            <li><a id="A40" runat="server" href="~/Page/Register/KodeKaretCleansing.aspx">Kode Karet Cleansing</a></li>
		</ul>
	</li>
    <li id="Li7" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            
            <li><a id="A41" runat="server" href="~/Page/Approval/KodeKaretCleansing.aspx">Kode Karet Cleansing</a></li>
		</ul>
	</li>
    <li id="a2" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li class="dropdown-submenu"><a href="#">Sales</a>
                <ul class="dropdown-menu">
                    <li><a id="A39" runat="server" href="~/Page/Report/Sales.aspx">Sales</a></li>
                    <li><a id="A29" runat="server" href="~/Page/Report/SalesRekapDossier.aspx">Sales Detail SNI</a></li>
                    <li><a id="A37" runat="server" href="~/Page/Report/InvoicePG.aspx">Sales PG Detail</a></li>
                    <li><a id="A38" runat="server" href="~/Page/Report/SalesRekap.aspx">Laporan Penjualan</a></li>
                </ul>
            </li>
            <li id="Li1" runat="server" class="dropdown-submenu">
                <a href="#">Diamond Jewellery
                    <ul class="dropdown-menu">
                        <li><a id="A1" runat="server" href="~/Page/Report/DJ_Rekap_LocationType.aspx">Diamond Jewellery Rekap</a></li>
                        <li><a id="A3" runat="server" href="~/Page/Report/DJ_Saldo_LocationType.aspx">Diamond Jewellery Saldo</a></li>
                        <li><a id="A4" runat="server" href="~/Page/Report/DJ_List_LocationType.aspx">Diamond Jewellery List</a></li>
                        <li><a id="A5" runat="server" href="~/Page/Report/DJ_Detail_LocationType.aspx">Diamond Jewellery Detail</a></li>
                        <li><a id="A9" runat="server" href="~/Page/Report/DJ_LifeCycle.aspx">Diamond Jewellery Life Cycle</a></li>
                    </ul>
                </a>
            </li>
            <li id="Li2" runat="server" class="dropdown-submenu">
                <a href="#">Gold Jewellery
                    <ul class="dropdown-menu">
                        <li><a id="A6" runat="server" href="~/Page/Report/GJ_Rekap_LocationType.aspx">Gold Jewellery Rekap</a></li>
                        <li><a id="A7" runat="server" href="~/Page/Report/GJ_Saldo_LocationType.aspx">Gold Jewellery Saldo</a></li>
                        <li><a id="A8" runat="server" href="~/Page/Report/GJ_List_LocationType.aspx">Gold Jewellery List</a></li>
                    </ul>
                </a>
            </li>
            <li id="Li3" runat="server" class="dropdown-submenu">
                <a href="#">Loose Diamond
                    <ul class="dropdown-menu">
                        <li><a id="A18" runat="server" href="~/Page/Report/LD_Rekap_LocationType.aspx">Loose Diamond Rekap</a></li>
                        <li><a id="A10" runat="server" href="~/Page/Report/LD_Saldo_LocationType.aspx">Loose Diamond Saldo</a></li>
                        <li><a id="A11" runat="server" href="~/Page/Report/LD_List.aspx">Loose Diamond List</a></li>
                        <li><a id="A12" runat="server" href="~/Page/Report/LD_Booked.aspx">Booked Loose Diamond</a></li>
                    </ul>
                </a>
            </li>
            <li><a id="A13" runat="server" href="~/Page/Report/PG_Rekap_LocationType.aspx">Plain Gold Rekap</a></li>
            <li><a id="A14" runat="server" href="~/Page/Report/PG_Saldo_LocationType.aspx">Plain Gold Saldo</a></li>
            <li><a id="A17" runat="server" href="~/Page/Report/PG_List_LocationType.aspx">Plain Gold List</a></li>
            <li><a id="A15" runat="server" href="~/Page/Report/Mounting_List.aspx">Mounting</a></li>
            <li><a id="A16" runat="server" href="~/Page/Report/Packaging/Product_Rekap_LocationType.aspx">Packaging</a></li>
            <li><a id="A19" runat="server" href="~/Page/Report/Souvenir/Product_Rekap_LocationType.aspx">Souvenir</a></li>
            <li><a id="A20" runat="server" href="~/Page/Report/ItemRekap.aspx">Item Rekap</a></li>
            <li id="Li4" runat="server" class="dropdown-submenu">
                <a href="#">Stone Rekap
                    <ul class="dropdown-menu">
                        <li><a id="A21" runat="server" href="~/Page/Report/Stone1ARekap.aspx">Diamond Reguler</a></li>
                        <li><a id="A22" runat="server" href="~/Page/Report/Stone1BRekap.aspx">Diamond Rapaport</a></li>
                        <li><a id="A23" runat="server" href="~/Page/Report/Stone2Rekap.aspx">Colour Diamond</a></li>
                        <li><a id="A24" runat="server" href="~/Page/Report/Stone3Rekap.aspx">Precious Stone</a></li>
                        <li><a id="A25" runat="server" href="~/Page/Report/Stone4Rekap.aspx">Semi Preciouse</a></li>
                        <li><a id="A26" runat="server" href="~/Page/Report/Stone5Rekap.aspx">Pearl</a></li>
                    </ul>
                </a>
            </li>
            <li id="Li5" runat="server" class="dropdown-submenu">
                <a href="#">Stone Detail
                    <ul class="dropdown-menu">
                        <li><a id="A27" runat="server" href="~/Page/Report/Stone1A.aspx">Diamond Reguler</a></li>
                        <li><a id="A28" runat="server" href="~/Page/Report/Stone1B.aspx">Diamond Rapaport</a></li>
                        <li><a id="A30" runat="server" href="~/Page/Report/Stone2.aspx">Colour Diamond</a></li>
                        <li><a id="A31" runat="server" href="~/Page/Report/Stone3.aspx">Precious Stone</a></li>
                        <li><a id="A32" runat="server" href="~/Page/Report/Stone4.aspx">Semi Preciouse</a></li>
                        <li><a id="A33" runat="server" href="~/Page/Report/Stone5.aspx">Pearl</a></li>
                    </ul>
                </a>
            </li>
            <li><a id="A34" runat="server" href="~/Page/Report/DJLD.aspx">Kombinasi DJ & LD</a></li>
            <%--li><a runat="server" href="~/Page/Report/DJ_Detail_LocationType.aspx">Plain Gold</a></--%>
            <li><a id="A35" runat="server" href="~/Page/Report/Stock_Ledger.aspx">Stock Ledger</a></li>
            <li><a id="A36" runat="server" href="~/Page/Report/StockBasic.aspx">Basic Item</a></li>
        </ul>
    </li>
</ul>
