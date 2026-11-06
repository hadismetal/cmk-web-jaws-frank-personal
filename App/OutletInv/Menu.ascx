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
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A25" runat="server" href="~/Page/Report/Stock/ItemRekap.aspx">Item Rekap</a></li>
            <li><a id="A31" runat="server" href="~/Page/Report/Stock/ItemRekapOutlet.aspx">Item Per Store</a></li>
            <li><a id="A26" runat="server" href="~/Page/Report/Stock/ItemDetail.aspx">Item Detail</a></li>
            <li id="Li1" runat="server" class="dropdown-submenu">
                
                <a href="#">Stock
                    <ul class="dropdown-menu">
                        <li><a id="A3" runat="server" href="~/Page/Report/Stock/DJ_Rekap.aspx">Diamond Jewellery Rekap</a></li>
			            <li><a id="A4" runat="server" href="~/Page/Report/Stock/DJ_List.aspx">Diamond Jewellery List</a></li>
                        <li><a id="A27" runat="server" href="~/Page/Report/Stock/DJ_Saldo.aspx">Diamond Jewellery Saldo</a></li>
                        <li><a id="A6" runat="server" href="~/Page/Report/Stock/DJ_Detail.aspx">Diamond Jewellery Detail</a></li>
                        <li><a id="A7" runat="server" href="~/Page/Report/Stock/GJ_Rekap.aspx">Gold Jewellery Rekap</a></li>
			            <li><a id="A8" runat="server" href="~/Page/Report/Stock/GJ_List.aspx">Gold Jewellery List</a></li>
                        <li><a id="A28" runat="server" href="~/Page/Report/Stock/GJ_Saldo.aspx">Gold Jewellery Saldo</a></li>
                        <li><a id="A9" runat="server" href="~/Page/Report/Stock/LD_List.aspx">Loose Diamond List</a></li>
                        <li><a id="A29" runat="server" href="~/Page/Report/Stock/LD_Saldo.aspx">Loose Diamond Saldo</a></li>
                        <li><a id="A10" runat="server" href="~/Page/Report/Stock/PG.aspx">Plain Gold</a></li>
                        <li><a id="A30" runat="server" href="~/Page/Report/Stock/PG_Saldo.aspx">Plain Gold Saldo</a></li>
                        <li><a id="A23" runat="server" href="~/Page/Report/Stock/Repair.aspx">Diamond Jewellery Repair</a></li>
                        
                    </ul>
                </a>
            </li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Packaging
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Report/Stock/Packaging/Product_Rekap.aspx">Packaging Actual</a></li>
                        <li><a runat="server" href="~/Page/Report/Stock/Packaging/Product_Summary.aspx">Packaging Summary</a></li>
                    </ul>   
                </a>
            </li>
            <li runat="server" class="dropdown-submenu">
                <a href="#">Souvenir
                    <ul class="dropdown-menu">
                        <li><a runat="server" href="~/Page/Report/Stock/Souvenir/Product_Rekap.aspx">Souvenir Actual</a></li>
                        <li><a runat="server" href="~/Page/Report/Stock/Souvenir/Product_Summary.aspx">Souvenir Summary</a></li>
                    </ul>
                </a>
            </li>
            <li id="Li2" runat="server" class="dropdown-submenu">
                <a href="#">Stone Rekap
                    <ul class="dropdown-menu">
                        <li><a id="A5" runat="server" href="~/Page/Report/Stock/Stone1ARekap.aspx">Diamond Reguler</a></li>
			            <li><a id="A11" runat="server" href="~/Page/Report/Stock/Stone1BRekap.aspx">Diamond Rapaport</a></li>
                        <li><a id="A12" runat="server" href="~/Page/Report/Stock/Stone2Rekap.aspx">Colour Diamond</a></li>
                        <li><a id="A13" runat="server" href="~/Page/Report/Stock/Stone3Rekap.aspx">Precious Stone</a></li>
                        <li><a id="A14" runat="server" href="~/Page/Report/Stock/Stone4Rekap.aspx">Semi Preciouse</a></li>
                        <li><a id="A15" runat="server" href="~/Page/Report/Stock/Stone5Rekap.aspx">Pearl</a></li>
                    </ul>
                </a>
            </li>
            <li id="Li3" runat="server" class="dropdown-submenu">
                <a href="#">Stone Detail
                    <ul class="dropdown-menu">
                        <li><a id="A17" runat="server" href="~/Page/Report/Stock/Stone1A.aspx">Diamond Reguler</a></li>
			            <li><a id="A18" runat="server" href="~/Page/Report/Stock/Stone1B.aspx">Diamond Rapaport</a></li>
                        <li><a id="A19" runat="server" href="~/Page/Report/Stock/Stone2.aspx">Colour Diamond</a></li>
                        <li><a id="A20" runat="server" href="~/Page/Report/Stock/Stone3.aspx">Precious Stone</a></li>
                        <li><a id="A21" runat="server" href="~/Page/Report/Stock/Stone4.aspx">Semi Preciouse</a></li>
                        <li><a id="A22" runat="server" href="~/Page/Report/Stock/Stone5.aspx">Pearl</a></li>
                    </ul>
                </a>
            </li>
            <li><a id="A24" runat="server" href="~/Page/Report/Stock/Stock_Ledger.aspx">Stock Ledger</a></li>
		</ul>
	</li>
    <li id="Li4" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Promo Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A35" runat="server" href="~/Page/ReportPromo/DJ/DJ_List.aspx">Promo Diamond Jewellery List</a></li>
            <li><a id="A34" runat="server" href="~/Page/ReportPromo/PG/PG_List.aspx">Promo Plain Gold List</a></li>
            <li><a id="A37" runat="server" href="~/Page/ReportPromo/GJ/GJ_List.aspx">Promo Gold Jewellery List</a></li>
        </ul>
    </li>
</ul>
