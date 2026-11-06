<%@ control language="C#" autoeventwireup="true" codefile="Menu.ascx.cs" inherits="Menu" %>
<style>
	.scrollable-dropdown {
		max-height: 500px;  /* Set the desired height */
		overflow-y: auto;   /* Enable vertical scrolling */
	}
</style>
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
		<ul class="dropdown-menu scrollable-dropdown">
			<li><a target="_blank" runat="server" href="~/Page/Create/Sales/Sales.aspx">Sales</a></li>
            <li><a target="_blank" runat="server" href="~/Page/Create/Resell/Resell.aspx">Resell</a></li>
            <li><a target="_blank" runat="server" href="~/Page/Create/TradeIn/TradeIn.aspx">Trade In</a></li>
            <li><a runat="server" href="~/Page/Create/Payment/Payment.aspx">Payment Sales Order</a></li>
            <li><a runat="server" href="~/Page/Create/Payment/PaymentRepair.aspx">Payment Repair</a></li>
            <li><a target="_blank" id="A21" runat="server" href="~/Page/Create/Payment/PaymentDP.aspx">Down Payment</a></li>
            <li><a target="_blank" runat="server" href="~/Page/Create/QC/DJ.aspx">QC DJ</a></li>
            <li><a target="_blank" runat="server" href="~/Page/Create/QC/DJTitipan.aspx">QC DJ (Titipan)</a></li>
            <li><a target="_blank" runat="server" href="~/Page/Create/QC/PG.aspx">QC PG</a></li>
            <li><a id="A17" runat="server" href="~/Page/Create/QC/GJ.aspx">QC GJ</a></li>
            <li><a id="A18" runat="server" href="~/Page/Create/QC/LD.aspx">Form Resell & QC LD</a></li>
            <li><a id="A11" target="_blank" runat="server" href="~/Page/Create/Repair/Repair.aspx">Repair</a></li>
            <li><a target="_blank" runat="server" href="~/Page/Create/Repair/RepairInvoice.aspx">Repair Invoice</a></li>
            <li><a id="A12" target="_blank" runat="server" href="~/Page/Create/Titipan/Titipan.aspx">Titipan Customer</a></li>
            <li><a id="A15" target="_blank" runat="server" href="~/Page/Create/TitipanReturn/TitipanReturn.aspx">Pengembalian Titipan Customer</a></li>
            <li><a id="A14" target="_blank" runat="server" href="~/Page/Create/Book/LD.aspx">Book Loose Diamond</a></li>
            <li><a id="A19" target="_blank" runat="server" href="~/Page/Create/Packaging/Sales.aspx">Packaging</a></li>
            <li><a id="A20" target="_blank" runat="server" href="~/Page/Create/Souvenir/Sales.aspx">Souvenir</a></li>
			<li><a id="AB1" target="_blank" runat="server" href="~/Page/Create/LainLain/Promo.aspx">Event</a></li>
			<li><a id="RegisterWhitelistTradeIn" target="_blank" runat="server" href="~/Page/Create/WhitelistTradeIn/index.aspx">Whitelist Trade-In</a></li>
			<li><a id="AB2" target="_blank" runat="server" href="~/Page/Create/VoucherLEI/Sales.aspx">Voucher LEI</a></li>
		</ul>
	</li>
	<li id="ax5" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a id="AA4" runat="server" href="~/Page/Approval/QCLDSM.aspx">Form Resell & QC LD By SM</a></li>
			<li><a id="AA5" runat="server" href="~/Page/Approval/QCLDAM.aspx">Form Resell & QC LD By AM</a></li>
			<li><a id="AA6" runat="server" href="~/Page/Approval/QCLD.aspx">Form Resell & QC LD By Procurement</a></li>
			<li><a id="AA7" runat="server" href="~/Page/Approval/QCLDFinal.aspx">Form Resell & QC LD By Merch. Director</a></li>
			<li><a id="ANJ4Y" runat="server" href="~/Page/Approval/PackagingSouvenir/PKSV.aspx">Approval Packaging & Souvenir</a></li>
			<li><a id="andriPriyana" runat="server" href="~/Page/Approval/SOVoid.aspx">Approval Void SalesOrder</a></li>
		</ul>
	</li>
    <li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Sales Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a id="A7" runat="server" href="~/Page/ReportSales/SalesOrderReport2.aspx">Sales Order</a></li>
            <li><a id="A22" runat="server" href="~/Page/ReportSales/SalesSummary.aspx">Sales Summary</a></li>
            <li><a id="A24" runat="server" href="~/Page/ReportSales/SalesDetail.aspx">Sales Detail</a></li>
            <li><a id="A25" runat="server" href="~/Page/ReportSales/SalesDetailDossier.aspx">Sales Detail (Dossier)</a></li>
            <li><a id="A26" runat="server" href="~/Page/ReportSales/DPPO_Report.aspx">Down Payment</a></li>

        </ul>
    </li>
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a runat="server" href="~/Page/Report/Payment.aspx">Payment</a></li>
            <li><a id="A8" runat="server" href="~/Page/Report/Resell.aspx">Resell</a></li>
            <li><a runat="server" href="~/Page/Report/ResellSummary.aspx">Resell Summary</a></li>
            <li><a id="A36" runat="server" href="~/Page/Report/ResellRekap.aspx">Resell Detail</a></li>
			<%--<li><a id="A4" runat="server" href="~/Page/Report/ResellButikEmasRekap.aspx">Resell Butik Emas Detail</a></li>--%>
            <li><a id="A5" runat="server" href="~/Page/Report/ResellGoldTunaiRekap.aspx">Resell Gold Tunai Detail</a></li>
            <li><a id="A9" runat="server" href="~/Page/Report/TradeIn.aspx">Trade In</a></li>
            <li><a runat="server" href="~/Page/Report/TradeInSummary.aspx">Trade In Summary</a></li>
            <li><a id="A37" runat="server" href="~/Page/Report/TradeInRekap.aspx">Trade In Detail</a></li>
            <li><a runat="server" href="~/Page/Report/QCDJ.aspx">QC DJ</a></li>
            <li><a runat="server" href="~/Page/Report/QCDJTitipan.aspx">QC DJ (Titipan)</a></li>
            <li><a runat="server" href="~/Page/Report/QCPG.aspx">QC PG</a></li>
			<li><a runat="server" href="~/Page/Report/QCLD.aspx">Form Resell & QC LD</a></li>
            <li><a id="A16" runat="server" href="~/Page/Report/Titipan.aspx">Titipan</a></li>
            <li><a id="A23" runat="server" href="~/Page/Report/Repair.aspx">Repair</a></li>
            <%--<li><a id="A34" runat="server" href="~/Page/Report/DP_Rekap.aspx">DP Rekap</a></li>
            <li><a runat="server" href="~/Page/Report/ItemDP.aspx">DP Item</a></li>
            <li><a id="A10" runat="server" href="~/Page/Report/DPPO.aspx">DP PO Detail</a></li>--%>
            <li><a id="A13" runat="server" href="~/Page/Report/CustomerPurchase.aspx">Customer Purchase</a></li>
            <li><a id="A30" runat="server" href="~/Page/Report/Souvenir.aspx">Laporan Hadiah &amp; Souvenir</a></li>
            <li><a id="A31" runat="server" href="~/Page/Report/Packaging.aspx">Packaging</a></li>
			<li><a id="A32" runat="server" href="~/Page/Report/Stamps/SalesOrderTransaction.aspx">Stamps Report</a></li>
			<li><a id="A33" runat="server" href="~/Page/Report/Bank/Report_EDC.aspx">Report Approval Code</a></li>
			<li><a id="AA3" runat="server" href="~/Page/Report/LainLain/PromoReport.aspx">Report Event</a></li>
			<li><a id="ReportWhitelistTradein" runat="server" href="~/Page/Report/WhitelistTradein/index.aspx">Report Whitelist Tradein</a></li>
		</ul>
	</li>
    <li id="ax7" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Kalkulator
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Report/Kalkulator/Kalkulator_Buyback.aspx">Kalkulator Buyback</a></li>
			<li><a id="AA12" runat="server" href="~/Page/Report/Kalkulator/CalcPricingStone">Kalkulator Pricing Stone</a></li>
			<%--<li><a runat="server" href="~/Page/Report/Kalkulator/Test.aspx">Test</a></li>--%>
		</ul>
	</li>
	<li id="a8x" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Customer Service
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Report/CustomerService/ReportEreceipt.aspx">Report E-Receipt</a></li>
		</ul>
	</li>
</ul>
