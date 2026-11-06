<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Menu" %>
<ul class="nav navbar-nav">
    <!-- Summary -->
    <li id="a0" runat="server">
        <a runat="server" href="~/Page/Summary.aspx" class="visible-sm visible-md visible-lg" title="Summary" style="margin-left: -15px;">
            <i class="icon-info icon-large"></i>
        </a>
        <a runat="server" href="~/Page/Summary.aspx" class="visible-xs">Summary</a>
    </li>
    <%--<li id="a1" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Register
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a id="A17" target="_blank" runat="server" href="~/Page/Create/Sales/Sales.aspx">Sales</a></li>
            <li><a id="A18" target="_blank" runat="server" href="~/Page/Create/Resell/Resell.aspx">Resell</a></li>
            <li><a id="A19" target="_blank" runat="server" href="~/Page/Create/TradeIn/TradeIn.aspx">Trade In</a></li>
            <li><a id="A21" runat="server" href="~/Page/Create/Payment/Payment.aspx">Payment Sales Order</a></li>
            <li><a target="_blank" id="A23" runat="server" href="~/Page/Create/Payment/PaymentDP.aspx">Down Payment PO</a></li>
            <li><a id="A24" target="_blank" runat="server" href="~/Page/Create/QC/DJ.aspx">QC DJ</a></li>
            <li><a id="A25" target="_blank" runat="server" href="~/Page/Create/QC/PG.aspx">QC PG</a></li>
            <li><a id="A26" runat="server" href="~/Page/Create/QC/GJ.aspx">QC GJ</a></li>
            <li><a id="A27" runat="server" href="~/Page/Create/QC/LD.aspx">QC LD</a></li>
        </ul>
    </li>--%>
    <!-- Module Menu -->
    <li id="Li1" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Register
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a id="A1" runat="server" href="~/Page/Register/BackdateSalesOrder.aspx">Backdate Sales Order</a></li>
            <li><a id="A5" runat="server" href="~/Page/Register/PaymentMethod.aspx">Payment Method</a></li>
            <li><a id="A4" runat="server" href="~/Page/Register/SubStorage.aspx">Update Substorage</a></li>
			<li><a id="AA1" runat="server" href="~/Page/Register/ApprovalCode.aspx">Update Approval Code EDC</a></li>
        </ul>
    </li>
    <li id="Li6" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a id="A2" runat="server" href="~/Page/Approval/BackdateSalesOrder.aspx">Backdate Sales Order</a></li>
			<li><a id="A2X" runat="server" href="~/Page/Approval/PackagingPusat.aspx">Packaging PK by Accounting</a></li>
			<li><a id="A3X" runat="server" href="~/Page/Approval/PackagingPusatLSP.aspx">Packaging PK by Finance</a></li>
        </ul>
    </li>
    <li id="Li2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Sales Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a id="A46" runat="server" href="~/Page/ReportSales/SalesOrderReport.aspx">Sales Order</a></li>
            <li><a id="A47" runat="server" href="~/Page/ReportSales/SalesSummary.aspx">Sales Summary</a></li>
            <li><a id="A48" runat="server" href="~/Page/ReportSales/SalesDetail.aspx">Sales Detail</a></li>
            <li><a id="A49" runat="server" href="~/Page/ReportSales/SalesDetailDossier.aspx">Sales Detail (Dossier)</a></li>
            <li><a id="A51" runat="server" href="~/Page/ReportSales/RepairDetail.aspx">Sales Detail Repair</a></li>

        </ul>
    </li>
    <li id="Li3" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li class="dropdown-submenu"><a href="#">Sales</a>
                <ul class="dropdown-menu">
                    <%--<li><a id="A30" runat="server" href="~/Page/Report/Sales_Order.aspx">Sales Order</a></li>--%>
                    <li><a runat="server" href="~/Page/Report/Sales.aspx">Sales Summary</a></li>
                    <li><a id="A7" runat="server" href="~/Page/Report/SalesRekap2.aspx">Sales Detail</a></li>
					<li><a runat="server" id="Aing" href="~/Page/Report/SalesRekapLEI.aspx">Sales Detail LEI</a></li>
					<li><a runat="server" id="AingMaung" href="~/Page/Report/SalesRekapLM.aspx">Sales Detail LM</a></li>
                    <li><a id="A29" runat="server" href="~/Page/Report/SalesRekapDossier.aspx">Sales Detail Dossier &amp; Carat Size</a></li>
                    <li><a id="A42" runat="server" href="~/Page/Report/SalesRekapSNI.aspx">Sales Detail SNI</a></li>
                    <li><a id="A37" runat="server" href="~/Page/Report/InvoiceDJ.aspx">Sales DJ Detail</a></li>
                    <li><a id="A38" runat="server" href="~/Page/Report/InvoiceDJ_Rekap.aspx">Sales DJ Group By</a></li>
                    <li><a id="A39" runat="server" href="~/Page/Report/InvoicePG.aspx">Sales PG Detail</a></li>
                    <li><a id="A40" runat="server" href="~/Page/Report/InvoicePG_Rekap.aspx">Sales PG Group By</a></li>
                </ul>
            </li>

            <li><a id="A8" runat="server" href="~/Page/Report/ItemRekap.aspx">Stock Item Rekap</a></li>
            <li><a id="A9" runat="server" href="~/Page/Report/DP_Rekap.aspx">DP Rekap</a></li>
            <li><a id="A10" runat="server" href="~/Page/Report/DPPO.aspx">DPPO Rekap</a></li>
            <li><a runat="server" href="~/Page/Report/Payment.aspx">Payment</a></li>
			<li><a id="ASIX" runat="server" href="~/Page/Report/Resell.aspx">Resell</a></li>
            <li><a id="A11" runat="server" href="~/Page/Report/ResellRekap.aspx">Resell Detail</a></li>
			<%--<li><a id="AX4" runat="server" href="~/Page/Report/ResellButikEmasRekap.aspx">Resell Butik Emas Detail</a></li>--%>
            <%--<li><a id="AX5" runat="server" href="~/Page/Report/ResellGoldTunaiRekap.aspx">Resell Gold Tunai Detail</a></li>--%>
            <li><a id="A16" runat="server" href="~/Page/Report/TradeInRekap.aspx">Trade In Detail</a></li>
            <li><a id="A100" runat="server" href="~/Page/Report/ProductOrder.aspx">Outstanding SPK</a></li>
            <li><a id="A300" runat="server" href="~/Page/Report/Profit.aspx">Gross Profit</a></li>
            <li><a id="A43" runat="server" href="~/Page/Report/ProfitRekap.aspx">Gross Profit Rekap</a></li>
            <li><a id="A35" runat="server" href="~/Page/Report/HPPDetail.aspx">HPP Detail</a></li>
            <li><a id="A44" runat="server" href="~/Page/Report/HPPRekap.aspx">HPP Rekap</a></li>
            <%--<li><a id="A4" runat="server" href="~/Page/Report/Omset.aspx">Omset</a></li>--%>
            <li><a id="A6" runat="server" href="~/Page/Report/BudgetStock.aspx">Budget Stock</a></li>
            <li><a id="A22" runat="server" href="~/Page/Report/SummaryStock.aspx">Summary Stock</a></li>
            <li><a id="A28" runat="server" href="~/Page/Report/Souvenir.aspx">Laporan Hadiah dan Souvenir</a></li>
            <li><a id="A36" runat="server" href="~/Page/Report/MDR.aspx">MDR</a></li>
			<li><a id="A50" runat="server" href="~/Page/Report/Bank/Report_EDC.aspx">Report Approval Code</a></li>
            <li><a id="A229" runat="server" href="~/Page/Report/DJ_Mutasi.aspx">Mutasi Barang</a></li>
			<li><a id="AS1" runat="server" href="~/Page/Report/TGP.aspx">TGP</a></li>
        </ul>
    </li>
    <li id="Li4" runat="server">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Report Outgoing
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a id="A12" runat="server" href="~/Page/ReportOutgoing/DJ.aspx">Diamond Jewellery</a></li>
            <li><a id="A31" runat="server" href="~/Page/ReportOutgoing/DJ_Detail_List.aspx">Diamond Jewellery Detail</a></li>
            <li><a id="A13" runat="server" href="~/Page/ReportOutgoing/PG.aspx">Plain Gold</a></li>
            <li><a id="A32" runat="server" href="~/Page/ReportOutgoing/PG_Detail_List.aspx">Plain Gold Detail</a></li>
            <li><a id="A14" runat="server" href="~/Page/ReportOutgoing/GJ.aspx">Gold Jewellery</a></li>
            <li><a id="A33" runat="server" href="~/Page/ReportOutgoing/GJ_Detail_List.aspx">Gold Jewellery List</a></li>
            <li><a id="A15" runat="server" href="~/Page/ReportOutgoing/LD.aspx">Loose Diamond</a></li>
            <li><a id="A34" runat="server" href="~/Page/ReportOutgoing/LD_Detail_List.aspx">Loose Diamond Detail</a></li>
        </ul>
    </li>
    <li id="Li5" runat="server">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Report Incoming
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a runat="server" href="~/Page/ReportIncoming/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/ReportIncoming/DJ_Detail_List.aspx">Diamond Jewellery Detail</a></li>
			<li><a runat="server" href="~/Page/ReportIncoming/PG.aspx">Plain Gold</a></li>
            <li><a runat="server" href="~/Page/ReportIncoming/PG_Detail_List.aspx">Plain Gold Detail</a></li>
            <li><a runat="server" href="~/Page/ReportIncoming/GJ.aspx">Gold Jewellery</a></li>
            <li><a runat="server" href="~/Page/ReportIncoming/GJ_Detail_List.aspx">Gold Jewellery Detail</a></li>
            <li><a id="A41" runat="server" href="~/Page/ReportIncoming/LD.aspx">Loose Diamond</a></li>
            <li><a id="A45" runat="server" href="~/Page/ReportIncoming/LD_Detail_List.aspx">Loose Diamond Detail</a></li>
        </ul>
    </li>
</ul>
