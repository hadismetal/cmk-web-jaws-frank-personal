﻿<%@ control language="C#" autoeventwireup="true" codefile="Menu.ascx.cs" inherits="Menu" %>
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
			<li><a runat="server" href="~/Page/Register/DJ/DJ.aspx">Diamond Jewellery</a></li>
			<%--<li><a id="a80" runat="server" href="~/Page/Register/DJ/DJDO.aspx">Diamond Jewellery (DO)</a></li>--%>
			<li><a runat="server" href="~/Page/Register/DJ/DJDONoSPK/DJDO.aspx">Diamond Jewellery (DO Tanpa SPK)</a></li>
            <li><a runat="server" href="~/Page/Register/PG/PG.aspx">PG</a></li>
			<li><a runat="server" href="~/Page/Register/PG/Outs/PG_Outs.aspx">PG Outs New</a></li>
            <li><a runat="server" href="~/Page/Register/PG/PGSKK/PG_Form.aspx">PG SKK</a></li>
            <li><a runat="server" href="~/Page/Register/Mounting/Mounting.aspx">Mounting</a></li>
            <li><a runat="server" href="~/Page/Register/GJ/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Register/LD/LD.aspx">Loose Diamond</a></li>
            <li><a runat="server" href="~/Page/Register/CZ/CZ.aspx">Cubic Zirconia</a></li>
			<li><a runat="server" href="~/Page/Register/Reborn/PLU_Reborn.aspx">PLU Reborn</a></li>
			<li><a runat="server" href="~/Page/Register/Duplicate/PLU_Duplicate.aspx">PLU Duplicate</a></li>
			<li><a runat="server" href="~/Page/Register/PreLEI/PreLEI.aspx">PreLEI</a></li>
			<li><a runat="server" href="~/Page/Register/MasterKaret/index.aspx">Master Material PG SKK</a></li>
		</ul>
	</li>
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Update
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Update/DJ/DJ.aspx">Diamond Jewelry</a></li>
            <li><a runat="server" href="~/Page/Update/PG/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Update/Mounting/Mounting.aspx">Mounting</a></li>
            <li><a runat="server" href="~/Page/Update/GJ/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Update/CZ/CZ.aspx">Cubic Zirconia</a></li>
		</ul>
	</li>
	<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Approval/DJ.aspx">Diamond Jewelry</a></li>
			<li><a runat="server" href="~/Page/Approval/PG.aspx">Plain Gold</a></li>
			<li><a id="konsimenjaja" runat="server" href="~/Page/Approval/StockConsignment.aspx">Stock Consignment SKK</a></li>
            <li><a runat="server" href="~/Page/Approval/Mounting.aspx">Mounting</a></li>
            <li><a runat="server" href="~/Page/Approval/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Approval/LD.aspx">Loose Diamond</a></li>
            <li><a runat="server" href="~/Page/Approval/CZ.aspx">Cubic Zirconia</a></li>
			<li><a runat="server" href="~/Page/Approval/PreLEI/PreLEI.aspx">PreLEI</a></li>
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
            <%--li><a runat="server" href="~/Page/Report/DJ_Rekap_LocationType.aspx">Diamond Jewellery Rekap</a></li>
			<li><a runat="server" href="~/Page/Report/DJ_List_LocationType.aspx">Diamond Jewellery List</a></li>
            <li><a runat="server" href="~/Page/Report/DJ_Detail_LocationType.aspx">Diamond Jewellery Detail</a></li>
            <li><a runat="server" href="~/Page/Report/GJ_Rekap_LocationType.aspx">Gold Jewellery Rekap</a></li>
			<li><a runat="server" href="~/Page/Report/GJ_List_LocationType.aspx">Gold Jewellery List</a></li>
            <li><a runat="server" href="~/Page/Report/LD_List_LocationType.aspx">Loose Diamond List</a></li--%>
			
			<li><a runat="server" href="~/Page/Report/DJ_List_Audit.aspx?tipe=0">Diamond Jewellery</a></li>
			<li><a runat="server" href="~/Page/Report/DJ_Rekap.aspx?tipe=0">Diamond Jewellery Rekap</a></li>
			<li><a runat="server" href="~/Page/Report/DJ_List.aspx?tipe=0">Diamond Jewellery List</a></li>
			<li><a runat="server" href="~/Page/Report/DJ_Detail.aspx?tipe=0">Diamond Jewellery Detail</a></li>
            <li><a runat="server" href="~/Page/Report/GJ_Rekap.aspx?tipe=0">Gold Jewellery Rekap</a></li>
			<li><a runat="server" href="~/Page/Report/GJ_List.aspx?tipe=0">Gold Jewellery List</a></li>
            <li><a runat="server" href="~/Page/Report/LD_List.aspx?tipe=0">Loose Diamond List</a></li>
			<%--<li><a runat="server" href="~/Page/Report/CZ_Detail.aspx?tipe=0">Cubic Zirconia Detail</a></li>--%>
            <%--li><a runat="server" href="~/Page/Report/PG.aspx">Plain Gold</a></--%>
            <li><a id="A4" runat="server" href="~/Page/Report/PG.aspx">Plain Gold Rekap</a></li>
            <li><a id="A12" runat="server" href="~/Page/Report/PG_Detail.aspx?tipe=0">Plain Gold Detail</a></li>
            <li><a runat="server" href="~/Page/Report/Mounting_List.aspx">Mounting</a></li>

            <li id="Li4" runat="server" class="dropdown-submenu">
                <a href="#">Stone Rekap
                    <ul class="dropdown-menu">
                        <li><a id="A13" runat="server" href="~/Page/Report/Stone1ARekap.aspx">Diamond Reguler</a></li>
			            <li><a id="A14" runat="server" href="~/Page/Report/Stone1BRekap.aspx">Diamond Rapaport</a></li>
                        <li><a id="A15" runat="server" href="~/Page/Report/Stone2Rekap.aspx">Colour Diamond</a></li>
                        <li><a id="A16" runat="server" href="~/Page/Report/Stone3Rekap.aspx">Precious Stone</a></li>
                        <li><a id="A17" runat="server" href="~/Page/Report/Stone4Rekap.aspx">Semi Preciouse</a></li>
                        <li><a id="A18" runat="server" href="~/Page/Report/Stone5Rekap.aspx">Pearl</a></li>
                    </ul>
                </a>
            </li>
            <li id="Li5" runat="server" class="dropdown-submenu">
                <a href="#">Stone Detail
                    <ul class="dropdown-menu">
                        <li><a id="A19" runat="server" href="~/Page/Report/Stone1A.aspx">Diamond Reguler</a></li>
			            <li><a id="A20" runat="server" href="~/Page/Report/Stone1B.aspx">Diamond Rapaport</a></li>
                        <li><a id="A21" runat="server" href="~/Page/Report/Stone2.aspx">Colour Diamond</a></li>
                        <li><a id="A22" runat="server" href="~/Page/Report/Stone3.aspx">Precious Stone</a></li>
                        <li><a id="A23" runat="server" href="~/Page/Report/Stone4.aspx">Semi Preciouse</a></li>
                        <li><a id="A24" runat="server" href="~/Page/Report/Stone5.aspx">Pearl</a></li>
                    </ul>
                </a>
            </li>
			<li><a runat="server" href="~/Page/Report/PreLEI/PreLEI.aspx">PreLEI</a></li>

            <%--li><a runat="server" href="~/Page/Report/DJ_Detail_LocationType.aspx">Plain Gold</a></--%>
		</ul>
	</li>
    <li id="a6" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Summary
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Summary/DJ.aspx">Diamond Jewelry</a></li>
			<li><a runat="server" href="~/Page/Summary/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/Summary/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/Summary/LD.aspx">Loose Diamond</a></li>
            <li><a runat="server" href="~/Page/Summary/Mounting.aspx">Mounting</a></li>
		</ul>
	</li>
</ul>
